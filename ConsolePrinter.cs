using PageReplacementAlgorithm.Algorithms;

namespace PageReplacementAlgorithm;

internal class ConsolePrinter
{
    public ConsolePrinter(string algorithmName,
        List<int> references,
        PageFaultValue[] pageFaultVector,
        int[,] timeTable,
        double pfRatio)
    {
        AlgorithmName = algorithmName;
        References = references;
        PageFaultVector = pageFaultVector;
        TimeTable = timeTable;
        PfRatio = pfRatio;
    }

    public string AlgorithmName { get; }
    public List<int> References { get; }
    public PageFaultValue[] PageFaultVector { get; }
    public int[,] TimeTable { get; }
    public double PfRatio { get; }

    internal void Print()
    {
        Console.WriteLine("Rezultati simulacije: \n\n");
        Console.WriteLine(AlgorithmName);

        WriteReferences();
        WritePageFaultVector();
        WriteTimeTable();

        Console.WriteLine();
        WritePageFaultRatio();
    }

    private void WritePageFaultRatio()
    {
        Console.WriteLine($"Efikasnost algoritma: PF = {PfRatio * PageFaultVector.Length}" +
            $" => pf = {PfRatio * PageFaultVector.Length} / {PageFaultVector.Length} = {Math.Round(PfRatio * 100)} %");
    }

    private void WriteTimeTable()
    {
        for (int i = 0; i < TimeTable.GetLength(1); i++)
        {
            for (int j = 0; j < TimeTable.GetLength(0); j++)
            {
                if (TimeTable[j, i] == (int)TimeTableValue.EMPTY)
                {
                    Console.Write($" {" ",-2}");
                }
                else
                {
                    Console.Write($" {TimeTable[j, i],-2}");
                }
            }
            Console.WriteLine();
        }
    }

    private void WriteReferences()
    {
        References.ForEach(x => Console.Write($" {x,-2}"));
        Console.WriteLine();
    }

    private void WritePageFaultVector()
    {
        foreach (var x in PageFaultVector)
        {
            string printed = x == PageFaultValue.PF ? "PF" : " ";
            Console.Write($" {printed,-2}");
        }
        Console.WriteLine();
    }

}