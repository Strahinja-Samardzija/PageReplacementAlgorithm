using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PageReplacementAlgorithm.Algorithms;

public class FIFO : AbstractAlgorithm
{
    public FIFO()
    {
        AlgorithmName = nameof(AlgorithmType.FIFO);
    }

    protected override void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references)
    {
        for (int i = 0; i < FrameCount; i++)
        {
            timeTable[iteration, i] = timeTable[iteration - 1, i];
        }
    }

    protected override void UpdateFramesMiss(int[,] timeTable, int iteration, List<int> references)
    {
        if (iteration < FrameCount)
        {
            FillAnEmptyFrame(timeTable, iteration, references);
        }
        else
        {
            ReplaceSwapOutReference(timeTable, iteration, references, references[iteration]);
        }
    }
}
