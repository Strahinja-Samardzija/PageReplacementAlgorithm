using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageReplacementAlgorithm;

public class UserInput
{
    public int FrameCount { get; private set; } = 0;
    public int ReferenceCount { get; private set; } = 0;

    public List<int> References { get; private set; } = new();
    public string Algorithm { get; private set; } = nameof(AlgorithmType.FIFO);

    public int InputFrameCount()
    {
    inputFrameCount:
        Console.WriteLine("Unesite broj okvira: ");
        bool _ = int.TryParse(Console.ReadLine(), out int frameCount);
        if (!_) goto inputFrameCount;
        FrameCount = frameCount;
        return FrameCount;
    }

    public int InputReferenceCount()
    {
    inputReferenceCount:
        Console.WriteLine("Unesite broj referenci: ");
        bool _ = int.TryParse(Console.ReadLine(), out int refCount);
        if (!_) goto inputReferenceCount;
        ReferenceCount = refCount;
        return ReferenceCount;
    }

    public List<int> InputReferenceList()
    {
        do
        {
            Console.WriteLine("Unesite reference: ");
            var references = Console.ReadLine()?.Split(' ')
                     .Select(int.Parse)
                     .ToList();
            References = references ??= new();
        } while (References.Count != ReferenceCount);
        return References;
    }

    public string InputAlgorithm()
    {
    inputAlgorithm:
        Console.WriteLine("Unesite algoritam: [FIFO, LRU, LFU, SecondChance, Optimal]");
        string algorithm = Console.ReadLine();
        switch (algorithm)
        {
            case nameof(AlgorithmType.FIFO):
            case nameof(AlgorithmType.LRU):
            case nameof(AlgorithmType.SecondChance):
            case nameof(AlgorithmType.LFU):
            case nameof(AlgorithmType.Optimal):
                return Algorithm = algorithm;
            default:
                goto inputAlgorithm;
        }

    }
}
