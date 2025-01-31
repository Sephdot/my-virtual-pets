using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services;
using my_virtual_pets_api.Cloud;

namespace my_virtual_pets_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private IImagesService _imagesService;
    private IStorageService _storageService;

    public ImagesController(IImagesService imagesService, IStorageService storageService)
    {
        _imagesService = imagesService;
        _storageService = storageService;
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
}
