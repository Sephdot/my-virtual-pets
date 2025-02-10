using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.Enums;

namespace my_virtual_pets_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{

    private IImagesService _imagesService;
    private IPixelateService _pixelate;

    public ImagesController(IImagesService imagesService, IPixelateService pixelate)
    {
        _imagesService = imagesService;
        _pixelate = pixelate;
    }

    //This attribute is causing issues apparently?
    //[Authorize(AuthenticationSchemes = "loginjwt")]
    [HttpPost]
    public async Task<IActionResult> PostImage(byte[] inputImage)
    {
        try
        {
            ImagesResponseDTO? result = await _imagesService.ProcessImageAsync(inputImage);
            if (result == null) return BadRequest("Invalid pet type.");
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return StatusCode(400, ex.Message); // this will trigger when there are too many animals in a photo, please dont change
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
