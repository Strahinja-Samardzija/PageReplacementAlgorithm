namespace PageReplacementAlgorithm.Algorithms;

internal class SecondChance : AbstractAlgorithm
{
    internal SecondChance()
    {
        AlgorithmName = nameof(AlgorithmType.SecondChance);
    }

    private Dictionary<int, bool> secondChance = new();

    protected override void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references)
    {
        secondChance[references[iteration]] = true;

        for (int i = 0; i < FrameCount; i++)
        {
            if (Math.Abs(timeTable[iteration - 1, i]) == references[iteration])
                timeTable[iteration, i] = -Math.Abs(timeTable[iteration - 1, i]);
            else
                timeTable[iteration, i] = timeTable[iteration - 1, i];
        }
    }

    protected override void UpdateFramesMiss(int[,] timeTable, int iteration, List<int> references)
    {
        secondChance[references[iteration]] = false;

        if (iteration < FrameCount)
        {
            FillAnEmptyFrame(timeTable, iteration, references);
            return;
        }

        for (int i = 0; i < FrameCount; i++)
        {
            timeTable[iteration, i] = timeTable[iteration - 1, i];
        }

        while (true)
        {
            int candidateSwapOut = Math.Abs(timeTable[iteration, FrameCount - 1]);
            if (secondChance[candidateSwapOut])
            {
                secondChance[candidateSwapOut] = false;
                BringToFront(timeTable, iteration, references);
            }
            else break;
        }
        ReplaceSwapOutReference(timeTable, iteration, references, references[iteration]);
    }

    protected override void ReplaceSwapOutReference(int[,] timeTable, int iteration, List<int> references, int swapOutReference)
    {
        int[] tableRow = new int[FrameCount];
        Buffer.BlockCopy(timeTable, FrameCount * iteration * sizeof(int), tableRow, 0, FrameCount * sizeof(int));

        for (int i = 1; i < FrameCount; i++)
        {
            timeTable[iteration, i] = tableRow[i - 1];
        }
        timeTable[iteration, 0] = references[iteration];
    }

    private void BringToFront(int[,] timeTable, int iteration, List<int> references)
    {
        int[] tableRow = new int[FrameCount - 1];
        Buffer.BlockCopy(timeTable, FrameCount * iteration * sizeof(int), tableRow, 0, (FrameCount - 1) * sizeof(int));

        timeTable[iteration, 0] = Math.Abs(timeTable[iteration, FrameCount - 1]);
        for (int i = 1; i < FrameCount; i++)
        {
            timeTable[iteration, i] = tableRow[i - 1];
        }
    }
}