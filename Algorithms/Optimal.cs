using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PageReplacementAlgorithm.Algorithms;

internal class Optimal : AbstractAlgorithm
{
    internal Optimal()
    {
        AlgorithmName = nameof(AlgorithmType.Optimal);
    }

    private bool isFirstPass = true;
    private Dictionary<int, List<int>> nextUsage = new();

    protected override void UpdateFramesHit(int[,] timeTable, int iteration, List<int> references)
    {
        nextUsage[references[iteration]].RemoveAt(0);

        for (int i = 0; i < FrameCount; i++)
        {
            timeTable[iteration, i] = timeTable[iteration - 1, i];
        }
    }

    protected override void UpdateFramesMiss(int[,] timeTable, int iteration, List<int> references)
    {
        if (isFirstPass)
        {
            for (int i = 0; i < references.Count; i++)
            {
                if (nextUsage.TryGetValue(references[i], out var usages))
                {
                    usages.Add(i);
                }
                else
                {
                    nextUsage[references[i]] = new List<int>() { i };
                }
            }
        }
        isFirstPass = false;

        nextUsage[references[iteration]].RemoveAt(0);

        if (iteration < FrameCount)
        {
            FillAnEmptyFrame(timeTable, iteration, references); 
            return;
        }

        int swapOutReference = -1;
        int maxUsageIndex = 0;
        for (int i = 0; i < FrameCount; i++)
        {
            int candidateSwapOut = timeTable[iteration - 1, i];
            if (nextUsage[candidateSwapOut].Count == 0)
            {
                swapOutReference = candidateSwapOut;
                break;
            }
            else if (nextUsage[candidateSwapOut].First() > maxUsageIndex)
            {
                maxUsageIndex = nextUsage[candidateSwapOut].First();
                swapOutReference = candidateSwapOut;
            }
        }
        ReplaceSwapOutReference(timeTable, iteration, references, swapOutReference);
    }    
}
