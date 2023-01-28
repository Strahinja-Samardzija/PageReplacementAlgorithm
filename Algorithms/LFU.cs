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

        SortByFrequencyDescending(timeTable, iteration - 1, iteration);
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

        ReplaceSwapOutReference(timeTable, iteration, references, timeTable[iteration - 1, FrameCount - 1]);

        SortByFrequencyDescending(timeTable, iteration, iteration);
    }

    private void SortByFrequencyDescending(int[,] timeTable, int srcIteration, int destIteration)
    {
        int[] tableRow = new int[FrameCount];
        Buffer.BlockCopy(timeTable, FrameCount * srcIteration * sizeof(int), tableRow, 0, FrameCount * sizeof(int));
        var sorted =
            from reference in tableRow
            orderby frequencies[reference] descending
            select reference;

        for (int i = 0; i < FrameCount; i++)
        {
            timeTable[destIteration, i] = sorted.ElementAt(i);
        }
    }
}