// See https://aka.ms/new-console-template for more information
using PageReplacementAlgorithm;

Console.WriteLine("Hello, World!");

UserInput input = new ();
int frameCount = input.InputFrameCount();
int referenceCount = input.InputReferenceCount();
var references = input.InputReferenceList();
var algorithmType = input.InputAlgorithm();
AlgorithmFactory.Get(algorithmType).RunSimulation(frameCount, referenceCount, references);