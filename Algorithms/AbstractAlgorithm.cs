using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageReplacementAlgorithm.Algorithms;

public enum PageFaultValue { PF = -1, EMPTY = -10 }

public enum TimeTableValue { EMPTY = -1 }

public abstract class AbstractAlgorithm
{
    public string AlgorithmName { get; protected set; } = "";
    public int FrameCount { get; private set; }
    public int ReferenceCount { get; private set; }


    public void RunSimulation(int frameCount, int referenceCount, List<int> references)
    {
        FrameCount = frameCount;
        ReferenceCount = referenceCount;

        PageFaultValue[] pageFaultVector = new PageFaultValue[ReferenceCount];
        Array.Fill(pageFaultVector, PageFaultValue.EMPTY);

        // rows represent iterations; columns represent different frames
        int[,] timeTable = new int[ReferenceCount, FrameCount];
        InitializeTimeTable(timeTable);

        for (int i = 0; i < referenceCount; i++)
        {
            if (FoundInFrames(timeTable, i, references[i]))
            {
                UpdateFramesHit(timeTable, i, references);
            }
            else
            {
                pageFaultVector[i] = PageFaultValue.PF;
                UpdateFramesMiss(timeTable, i, references);
            }
        }

        double pfRatio = CalculatePageFaultRatio(pageFaultVector);

        new ConsolePrinter(AlgorithmName, references, pageFaultVector, timeTable, pfRatio).Print();
    }
    protected abstract void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references);
    protected abstract void UpdateFramesMiss(int[,] timeTable, int iteration, List<int> references);

    protected void ReplaceSwapOutReference(int[,] timeTable, int iteration, List<int> references, int swapOutReference)
    {
        for (int i = 0; i < FrameCount; i++)
        {
            timeTable[iteration, i] = swapOutReference == timeTable[iteration - 1, i] ?
                references[iteration] : timeTable[iteration - 1, i];
        }
    }

    protected void FillAnEmptyFrame(int[,] timeTable, int iteration, List<int> references)
    {
        for (int i = 0; i < iteration; i++)
        {
            timeTable[iteration, i] = timeTable[iteration - 1, i];
        }
        timeTable[iteration, iteration] = references[iteration];
    }

    private double CalculatePageFaultRatio(PageFaultValue[] pageFaultVector)
    {
        return (double)pageFaultVector.Count(x => x == PageFaultValue.PF) / pageFaultVector.Length;
    }

    private void InitializeTimeTable(int[,] timeTable)
    {
        for (int i = 0; i < ReferenceCount; i++)
            for (int j = 0; j < FrameCount; j++)
                timeTable[i, j] = (int)TimeTableValue.EMPTY;
    }


    private bool FoundInFrames(int[,] timeTable, int iteration, int reference)
    {
        if (iteration == 0) return false;

        for (int i = 0; i < FrameCount; i++)
        {
            if (reference == timeTable[iteration - 1, i])
            {
                return true;
            }
        }
        return false;
    }
}
