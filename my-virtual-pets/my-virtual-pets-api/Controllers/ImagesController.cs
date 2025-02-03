using ImageRecognition;
using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Cloud;
using my_virtual_pets_api.Services;
using my_virtual_pets_api.Services.Interfaces;
using System.Drawing;

namespace my_virtual_pets_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private IImagesService _imagesService;
    private IStorageService _storageService;
    private IRecognitionService _recognitionService;
    private IPixelate _pixelateService;

    public ImagesController(IImagesService imagesService, IStorageService storageService, IRecognitionService recognitionService, IPixelate pixelateService)
    {
        _imagesService = imagesService;
        _storageService = storageService;
        _recognitionService = recognitionService;
        _pixelateService = pixelateService;
    }

    [HttpPost]
    [Route("test")]
    public async Task<IActionResult> GetTestImage(string inputImageUrl, string key)
    {
        try
        {
            byte[]? outputImage = await _imagesService.RemoveBackgroundAsync(inputImageUrl);
            if (outputImage == null) return BadRequest("Output image was null");
            await _storageService.UploadObject(outputImage, key);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpPost]
    [Route("scanTest")]
    public async Task<IActionResult> Recognizer()
    {
        var result = await _recognitionService.CheckImageInput("https://i.natgeofe.com/n/4cebbf38-5df4-4ed0-864a-4ebeb64d33a4/NationalGeographic_1468962_16x9.jpg");
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result.name);
    }

    [HttpPost]
    [Route("pipelineTest")]
    public async Task<IActionResult> TestPipeline()
    {
        byte[] inputImage = System.IO.File.ReadAllBytes("Resources/Images/testimage.png");

        var recognitionResult = await _recognitionService.CheckImageInput(inputImage);

        if (recognitionResult == null)
        {
            return BadRequest();
        }

        var removeBgResult = await _imagesService.RemoveBackground(inputImage);
        Bitmap inputBitmap;

        using (var ms = new MemoryStream(removeBgResult))
        {
            inputBitmap = new Bitmap(ms);
        }

        Bitmap pixelatedImage = _pixelateService.PixelateImage(inputBitmap, 5);

        ImageConverter converter = new ImageConverter();
        byte[] pixelResult = (byte[])converter.ConvertTo(pixelatedImage, typeof(byte[]));

        return File(pixelResult, "image/png");
    }

    [HttpPost]
    [Route("transparencyTest")]
    public IActionResult TestTransparency()
    {
        byte[] inputImage = System.IO.File.ReadAllBytes("Resources/Images/DogWithBackground.jpg");
        Bitmap inputBitmap;

        using (var ms = new MemoryStream(inputImage))
        {
            inputBitmap = new Bitmap(ms);
        }

        Bitmap pixelatedImage = _pixelateService.PixelateImage(inputBitmap, 12, false);
        ImageConverter converter = new ImageConverter();
        byte[] pixelResult = (byte[])converter.ConvertTo(pixelatedImage, typeof(byte[]));

        return File(pixelResult, "image/png");
    }
}
