using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;

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
    [Route("pipelineTest")]
    public async Task<IActionResult> TestPipeline()
    {
        byte[] inputImage = System.IO.File.ReadAllBytes("Resources/Images/testimage.png");

        var result = await _imagesService.ProcessImageAsync(inputImage);
        if (result == null) return BadRequest();

        return File(result, "image/png");
    }
}
