using Microsoft.AspNetCore.Mvc;
using my_virtual_pets_api.Services.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PixelateController : ControllerBase
    {
        private IPixelate _pixelateService;

        public PixelateController(IPixelate pixelateService)
        {
            _pixelateService = pixelateService;
        }

        [HttpPost]
        public IActionResult PostPixelatedImage()
        {
            Bitmap testimage = new Bitmap("Resources/Images/testimage.png");

            Bitmap pixelatedImage = _pixelateService.PixelateImage(testimage, 10);

            ImageConverter converter = new ImageConverter();
            var result = (byte[])converter.ConvertTo(pixelatedImage, typeof(byte[]));
            return File(result, "image/png");
        }
    }
}
