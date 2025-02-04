using ImageRecognition;
using my_virtual_pets_api.Cloud;
using my_virtual_pets_api.Services.Interfaces;
using System.Drawing;

namespace my_virtual_pets_api.Services;

public class ImagesService : IImagesService
{
    private IStorageService _storageService;
    private IRecognitionService _recognitionService;
    private IPixelate _pixelateService;
    private IRemoveBackgroundService _removeBackgroundService;

    public ImagesService(IStorageService storageService, IRecognitionService recognitionService, IPixelate pixelateService, IRemoveBackgroundService removeBackgroundService)
    {
        _storageService = storageService;
        _recognitionService = recognitionService;
        _pixelateService = pixelateService;
        _removeBackgroundService = removeBackgroundService;
    }

    public async Task<byte[]?> ProcessImageAsync(byte[] inputImage)
    {
        //Recognise image
        //TO DO: Ask Callum about error handling in recognitionService
        var recognitionResult = await _recognitionService.CheckImageInput(inputImage);

        if (recognitionResult == null)
        {
            return null;
        }

        //Remove backround
        var removeBgResult = await _removeBackgroundService.RemoveBackgroundAsync(inputImage);
        Bitmap inputBitmap;

        //Pixelate image
        using (var ms = new MemoryStream(removeBgResult))
        {
            inputBitmap = new Bitmap(ms);
        }

        Bitmap pixelatedImage = _pixelateService.PixelateImage(inputBitmap, 6, true);

        ImageConverter converter = new ImageConverter();
        byte[] pixelResult = (byte[])converter.ConvertTo(pixelatedImage, typeof(byte[]));


        //TO DO: upload image to bucket
        //TO DO: Save image to database
        //return image id to Pets controller? Also needs IPredicted

        return pixelResult;
    }
}
