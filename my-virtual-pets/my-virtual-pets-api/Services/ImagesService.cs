using ImageRecognition;
using my_virtual_pets_api.Cloud;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library;
using System.Drawing;

namespace my_virtual_pets_api.Services;

public class ImagesService : IImagesService
{
    private IStorageService _storageService;
    private IRecognitionService _recognitionService;
    private IPixelate _pixelateService;
    private IRemoveBackgroundService _removeBackgroundService;
    private IImageRepository _imageRepository;

    public ImagesService(IStorageService storageService, IRecognitionService recognitionService, IPixelate pixelateService, IRemoveBackgroundService removeBackgroundService, IImageRepository imageRepository)
    {
        _storageService = storageService;
        _recognitionService = recognitionService;
        _pixelateService = pixelateService;
        _removeBackgroundService = removeBackgroundService;
        _imageRepository = imageRepository;
    }

    public async Task<ImagesResponseDto?> ProcessImageAsync(byte[] inputImage)
    {
        //Recognise image
        //TO DO: Ask Callum about error handling in recognitionService
        var recognitionResult = await _recognitionService.CheckImageInput(inputImage);

        if (recognitionResult == null) return null;


        //Remove backround
        var removeBgResult = await _removeBackgroundService.RemoveBackgroundAsync(inputImage);
        if (removeBgResult == null) return null;

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
        string imageId = Guid.NewGuid().ToString();
        var uploadResult = await _storageService.UploadObjectAsync(pixelResult, imageId);
        //return string image url and string pet type
        if (!uploadResult.Item1) return null;
        return new ImagesResponseDto { imageUrl = uploadResult.imageUrl, animalType = recognitionResult.displayName };
    }

    public Guid AddImage(string uimageUrl)
    {
        return _imageRepository.AddImage(uimageUrl);
    }
}
