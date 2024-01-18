using Agnes.Laurencon.FeatureMatching;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/FeatureMatching", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count != 2)
        return Results.BadRequest();
    using var objectSourceStream = files[0].OpenReadStream();
    using var objectMemoryStream = new MemoryStream();
    objectSourceStream.CopyTo(objectMemoryStream);
    var imageObjectData = objectMemoryStream.ToArray();
    using var sceneSourceStream = files[1].OpenReadStream();
    using var sceneMemoryStream = new MemoryStream();
    sceneSourceStream.CopyTo(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();

    var singleImageList = new List<byte[]>();
    singleImageList.Add(imageSceneData);
    var result = await new ObjectDetection().DetectObjectInScenesAsync(imageObjectData, singleImageList);
    
    return Results.File(result[0].ImageData, "image/png");
}).DisableAntiforgery();


app.Run();
