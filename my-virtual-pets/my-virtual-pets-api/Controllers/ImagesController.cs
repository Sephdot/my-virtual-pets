using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services;
using my_virtual_pets_api.Cloud;
using ImageRecognition;

namespace my_virtual_pets_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private IImagesService _imagesService;
    private IStorageService _storageService;
    private IRecognitionService _recognitionService;

    public ImagesController(IImagesService imagesService, IStorageService storageService, IRecognitionService recognitionService)
    {
        _imagesService = imagesService;
        _storageService = storageService;
        _recognitionService = recognitionService;
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
}
