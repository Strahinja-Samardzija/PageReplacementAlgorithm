namespace PageReplacementAlgorithm.Algorithms;

internal class SecondChance : AbstractAlgorithm
{
    internal SecondChance()
    {
        AlgorithmName = nameof(AlgorithmType.SecondChance);
    }

    private Dictionary<int, bool> secondChance = new();
    private int order = 0;

    protected override void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references)
    {
        secondChance[references[iteration]] = true;

        for (int i = 0; i < FrameCount; i++)
        {
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

        int swapOutReference = -1;
        for (int i = order++ % FrameCount, j = 0;
            j < FrameCount;
            i = order++ % FrameCount, j++)
        {
            int candidateSwapOut = timeTable[iteration - 1, i];
            if (secondChance[candidateSwapOut])
            {
                secondChance[candidateSwapOut] = false;
            }
            else
            {
                swapOutReference = candidateSwapOut;
                break;
            }
        }
        if (swapOutReference == -1) swapOutReference = timeTable[iteration - 1, order++ % FrameCount];
        ReplaceSwapOutReference(timeTable, iteration, references, swapOutReference);
    }
}