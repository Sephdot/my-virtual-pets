using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services;

namespace my_virtual_pets_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private IImagesService imagesService;

    public ImagesController(IImagesService imagesService)
    {
        this.imagesService = imagesService;
    }

    [HttpGet]
    [Route("test")]
    public async Task<IActionResult> GetTestImage(string inputImageUrl)
    {
        //string inputImagePath = "Resources/TestImages/DogWithBackground.jpg";
        //byte[] inputImage = await System.IO.File.ReadAllBytesAsync(inputImagePath);
        //byte[]? outputImage = await imagesService.RemoveBackground(inputImage);
        //if (outputImage == null) return BadRequest("Output image was null");
        //return File(outputImage, "image/png");

        string? outputImageUrl = await imagesService.RemoveBackground(inputImageUrl);
        if (outputImageUrl == null) return BadRequest("Output image was null");
        return Ok(outputImageUrl);
    }
}
