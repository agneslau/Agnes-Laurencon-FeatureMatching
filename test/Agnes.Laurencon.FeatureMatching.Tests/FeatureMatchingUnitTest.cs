using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
namespace Agnes.Laurencon.FeatureMatching.Tests;
public class FeatureMatchingUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath,
                     "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }
        var objectImageData = await File.ReadAllBytesAsync(Path.Combine(executingPath,
            "Laurencon-Agnes-Object.jpg"));
        var detectObjectInScenesResults = await new
            ObjectDetection().DetectObjectInScenesAsync(objectImageData, imageScenesData);

        Assert.Equal("[{\"X\":1501,\"Y\":1732},{\"X\":337,\"Y\":2288},{\"X\":814,\"Y\":3238},{\"X\":1955,\"Y\":2536}]",JsonSerializer.Serialize(detectObjectInScenesResults[0].Points));

        Assert.Equal("[{\"X\":651,\"Y\":1841},{\"X\":1360,\"Y\":2738},{\"X\":1994,\"Y\":2230},{\"X\":1353,\"Y\":1367}]",JsonSerializer.Serialize(detectObjectInScenesResults[1].Points));
    }
    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}