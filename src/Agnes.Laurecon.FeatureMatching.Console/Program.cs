// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Text.Json;
using Agnes.Laurencon.FeatureMatching;

Console.WriteLine("Hello, World!");

// Path: src/Agnes.Laurecon.FeatureMatching.Console/Program.cs

var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
var executingPath = Path.GetDirectoryName(executingAssemblyPath);
var imageScenesData = new List<byte[]>();
foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath, args[1])))
{
    var imageBytes = await File.ReadAllBytesAsync(imagePath);
    imageScenesData.Add(imageBytes);
}
var objectImageData = await File.ReadAllBytesAsync(Path.Combine(executingPath,
    args[0]));

var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(objectImageData, imageScenesData);

foreach (var objectDetectionResult in detectObjectInScenesResults)
{
    Console.WriteLine($"Points:{JsonSerializer.Serialize(objectDetectionResult.Points)}");
}


