namespace PageReplacementAlgorithm.Algorithms;

internal class LRU : AbstractAlgorithm
{
    internal LRU()
    {
        AlgorithmName = nameof(AlgorithmType.LRU);
    }

    private Dictionary<int, int> lastUsage = new();

    protected override void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references)
    {
        lastUsage[references[iteration]] = iteration;

        for (int i = 0; i < FrameCount; i++)
        {
            timeTable[iteration, i] = timeTable[iteration - 1, i];
        }
    }

    protected override void UpdateFramesMiss(int[,] timeTable, int iteration, List<int> references)
    {
        lastUsage[references[iteration]] = iteration;

        if (iteration < FrameCount)
        {
            FillAnEmptyFrame(timeTable, iteration, references);
            return;
        }

        int swapOutReference = -1;
        int oldestUsage = int.MaxValue;
        for (int i = 0; i < FrameCount; i++)
        {
            int candidateSwapOut = timeTable[iteration - 1, i];
            if (lastUsage[candidateSwapOut] < oldestUsage)
            {
                oldestUsage = lastUsage[candidateSwapOut];
                swapOutReference = candidateSwapOut;
            }
        }
        ReplaceSwapOutReference(timeTable, iteration, references, swapOutReference);
    }
}