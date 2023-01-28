using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PageReplacementAlgorithm.Algorithms;

namespace PageReplacementAlgorithm;

public static class AlgorithmFactory
{
    public static AbstractAlgorithm Get(string type)
    {
        switch (type)
        {
            case nameof(AlgorithmType.FIFO):
                return new FIFO();
            case nameof(AlgorithmType.LRU):
                return new LRU();
            case nameof(AlgorithmType.SecondChance):
                return new SecondChance();
            case nameof(AlgorithmType.LFU):
                return new LFU();
            case nameof(AlgorithmType.Optimal):
                return new Optimal();

            default:
                throw new Exception();
        }
    }
}
