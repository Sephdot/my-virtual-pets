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

    [HttpPost]
    [Route("test")]
    public async Task<IActionResult> GetTestImage(string inputImageUrl)
    {
        //string inputImagePath = "Resources/TestImages/DogWithBackground.jpg";
        //byte[] inputImage = await System.IO.File.ReadAllBytesAsync(inputImagePath);
        //if (outputImage == null) return BadRequest("Output image was null");
        //return File(outputImage, "image/png");

        //string? outputImageUrl = await imagesService.RemoveBackground(inputImageUrl);
        byte[]? outputImage = await imagesService.RemoveBackgroundAsync(inputImageUrl);
        if (outputImage == null) return BadRequest("Output image was null");
        Cloud.S3StorageService s3 = new();
        s3.UploadObject(outputImage, "testKey");
        return Ok(outputImage);
    }
}
