using System.Xml;

namespace PageReplacementAlgorithm.Algorithms;

internal class LRU : AbstractAlgorithm
{
    internal LRU()
    {
        AlgorithmName = nameof(AlgorithmType.LRU);
    }

    protected override void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references)
    {
        int[] tableRow = new int[FrameCount];
        Buffer.BlockCopy(timeTable, FrameCount * (iteration - 1) * sizeof(int), tableRow, 0, FrameCount * sizeof(int));
        var rotated =
            from reference in tableRow
            where reference != references[iteration]
            select reference;
        timeTable[iteration, 0] = references[iteration];
        for(int i = 1; i < FrameCount; i++)
        {
            timeTable[iteration, i] = rotated.ElementAt(i - 1);
        }
    }

    protected override void UpdateFramesMiss(int[,] timeTable, int iteration, List<int> references)
    {
        if (iteration < FrameCount)
        {
            FillAnEmptyFrame(timeTable, iteration, references);
            return;
        }

        ReplaceSwapOutReference(timeTable, iteration, references, references[iteration]);
    }
}