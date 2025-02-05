using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library;

namespace my_virtual_pets_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{

    private IImagesService _imagesService;

    public ImagesController(IImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    [HttpPost]
    public async Task<IActionResult> TestPipeline()
    {
        byte[] inputImage = System.IO.File.ReadAllBytes("Resources/Images/testimage.png");

        ImagesResponseDto? result = await _imagesService.ProcessImageAsync(inputImage);
        if (result == null) return BadRequest();
        return Ok(result);
    }
}
