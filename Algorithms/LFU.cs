namespace PageReplacementAlgorithm.Algorithms;

internal class LFU : AbstractAlgorithm
{
    internal LFU()
    {
        AlgorithmName = nameof(AlgorithmType.LFU);
    }

    private Dictionary<int, int> frequencies = new();

    protected override void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references)
    {
        frequencies[references[iteration]]++;

        for (int i = 0; i < FrameCount; i++)
        {
            timeTable[iteration, i] = timeTable[iteration - 1, i];
        }
    }

    protected override void UpdateFramesMiss(int[,] timeTable, int iteration, List<int> references)
    {
        if (frequencies.TryGetValue(references[iteration], out int freq))
            frequencies[references[iteration]] = freq + 1;
        else frequencies[references[iteration]] = 1;

        if (iteration < FrameCount)
        {
            FillAnEmptyFrame(timeTable, iteration, references);
            return;
        }

        int swapOutReference = -1;
        int minFrequency = int.MaxValue;
        for (int i = 0; i < FrameCount; i++)
        {
            int candidateSwapOut = timeTable[iteration - 1, i];
            if (frequencies[candidateSwapOut] < minFrequency)
            {
                minFrequency = frequencies[candidateSwapOut];
                swapOutReference = candidateSwapOut;
            }
        }
        ReplaceSwapOutReference(timeTable, iteration, references, swapOutReference);
    }
}