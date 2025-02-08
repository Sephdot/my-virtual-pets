using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostImage(byte[] inputImage)
    {
        try
        {
            ImagesResponseDTO? result = await _imagesService.ProcessImageAsync(inputImage);
            if (result == null) return BadRequest("Invalid pet type.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
