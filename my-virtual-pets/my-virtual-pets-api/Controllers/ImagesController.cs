using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.Enums;
using System.Drawing;

namespace my_virtual_pets_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{

    private IImagesService _imagesService;
    private IPixelate _pixelate;

    public ImagesController(IImagesService imagesService, IPixelate pixelate)
    {
        _imagesService = imagesService;
        _pixelate = pixelate;
    }

    [HttpPost]
    public async Task<IActionResult> TestPipeline(byte[] inputImage)
    {
        //byte[] inputImage = System.IO.File.ReadAllBytes("Resources/Images/testimage.png");

        ImagesResponseDTO? result = await _imagesService.ProcessImageAsync(inputImage);
        if (result == null) return BadRequest();
        return Ok(result);
    }

    [HttpPost]
    [Route("/test")]
    public async Task<IActionResult> TestNewPipeline()
    {
        byte[] inputImage = System.IO.File.ReadAllBytes("Resources/Images/cat2.jpg");

        ImagesResponseDTO? result = await _imagesService.ProcessImageAsync(inputImage);
        if (result == null) return BadRequest();

        return Ok(result);
    }

    [HttpPost]
    [Route("{blockCount}")]
    public IActionResult TestImage(int blockCount)
    {
        byte[] inputImage = System.IO.File.ReadAllBytes("Resources/Images/cat2.jpg");
        Bitmap inputBitmap;
        using (var ms = new MemoryStream(inputImage))
        {
            inputBitmap = new Bitmap(ms);
        }

        Bitmap pixelatedImage = _pixelate.PixelateImage(inputBitmap, blockCount, true);

        ImageConverter converter = new ImageConverter();
        byte[] pixelResult = (byte[])converter.ConvertTo(pixelatedImage, typeof(byte[]));
        return File(pixelResult, "image/png");

    }
}
