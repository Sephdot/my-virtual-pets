using ImageRecognition;
using Moq;
using my_virtual_pets_api.Cloud;
using my_virtual_pets_api.Repositories.Interfaces;
using my_virtual_pets_api.Services;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.Enums;
namespace my_virtual_pets.Tests;

internal class ImageServiceTests
{
    private ImagesService imageService;
    private Mock<IStorageService> _storageService;
    private Mock<IRecognitionService> _recognitionService;
    private Mock<IPixelateService> _pixelateService;
    private Mock<IRemoveBackgroundService> _removeBackgroundService;
    private Mock<IImageRepository> _imageRepository;
    private byte[] inputImage;
    private byte[] noBackgroundImage;
    private byte[] pixelatedImage;
    private string key = "key";
    private (bool, string) uploadResult = (true, "url");
    private IPredicted recognitionResult;

    [SetUp]
    public void Setup()
    {
        _storageService = new Mock<IStorageService>();
        _recognitionService = new Mock<IRecognitionService>();
        _pixelateService = new Mock<IPixelateService>();
        _removeBackgroundService = new Mock<IRemoveBackgroundService>();
        _imageRepository = new Mock<IImageRepository>();

        imageService = new ImagesService(_storageService.Object, _recognitionService.Object, _pixelateService.Object, _removeBackgroundService.Object, _imageRepository.Object);
        inputImage = [1, 1, 1, 1];
        noBackgroundImage = [1, 1, 1, 1];
        pixelatedImage = [1, 2, 3, 4];
        recognitionResult = new Child { id = 717939453480153, type = "category", name = "cat", displayName = "Cat", score = 1.0, uncalibrated_score = 0.1185302734375, children = [] };
    }

    [Test]
    public async Task ProcessImageAsync_CallsRemoveBackroundMethodOnce()
    {

        _recognitionService.Setup(m => m.CheckImageInput(It.IsAny<byte[]>())).ReturnsAsync(recognitionResult);
        _removeBackgroundService.Setup(m => m.RemoveBackgroundAsync(It.IsAny<byte[]>())).ReturnsAsync(noBackgroundImage);
        _pixelateService.Setup(m => m.PixelateImage(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(pixelatedImage);
        _storageService.Setup(m => m.UploadObjectAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(uploadResult);

        var result = await imageService.ProcessImageAsync(inputImage);

        _recognitionService.Verify(m => m.CheckImageInput(It.IsAny<byte[]>()), Times.Once);
        _removeBackgroundService.Verify(m => m.RemoveBackgroundAsync(It.IsAny<byte[]>()), Times.Once);
        _pixelateService.Verify(m => m.PixelateImage(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
        _storageService.Verify(m => m.UploadObjectAsync(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ProcessImageAsync_ReturnImagesResponseDto_WhenSuccessful()
    {
        var expectedResult = new ImagesResponseDTO { ImageUrl = "url", PetType = PetType.CAT };
        _recognitionService.Setup(m => m.CheckImageInput(It.IsAny<byte[]>())).ReturnsAsync(recognitionResult);
        _removeBackgroundService.Setup(m => m.RemoveBackgroundAsync(It.IsAny<byte[]>())).ReturnsAsync(noBackgroundImage);
        _pixelateService.Setup(m => m.PixelateImage(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(pixelatedImage);
        _storageService.Setup(m => m.UploadObjectAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(uploadResult);

        var result = await imageService.ProcessImageAsync(inputImage);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ImageUrl, Is.EqualTo("url"));
        Assert.That(result.PetType, Is.EqualTo(PetType.CAT));
    }

    [Test]
    public void ProcessImageAsync_ThrowsException_WhenRecognitionFails()
    {
        _recognitionService.Setup(s => s.CheckImageInput(It.IsAny<byte[]>())).ReturnsAsync((Child)null!);
        Assert.ThrowsAsync<Exception>(() => imageService.ProcessImageAsync(inputImage));
    }

    [Test]
    public void ProcessImageAsync_ThrowsException_WhenBackgroundRemovalFails()
    {
        _recognitionService.Setup(s => s.CheckImageInput(It.IsAny<byte[]>())).ReturnsAsync(recognitionResult);

        _removeBackgroundService.Setup(s => s.RemoveBackgroundAsync(It.IsAny<byte[]>())).ReturnsAsync((byte[])null!);

        Assert.ThrowsAsync<Exception>(() => imageService.ProcessImageAsync(inputImage));
    }

    [Test]
    public void ProcessImageAsync_ThrowsException_WhenUploadFails()
    {
        (bool, string) failedUploadResult = (false, "error");
        var expectedResult = new ImagesResponseDTO { ImageUrl = "url", PetType = PetType.CAT };
        _recognitionService.Setup(m => m.CheckImageInput(It.IsAny<byte[]>())).ReturnsAsync(recognitionResult);
        _removeBackgroundService.Setup(m => m.RemoveBackgroundAsync(It.IsAny<byte[]>())).ReturnsAsync(noBackgroundImage);
        _pixelateService.Setup(m => m.PixelateImage(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(pixelatedImage);
        _storageService.Setup(m => m.UploadObjectAsync(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(failedUploadResult);

        Assert.ThrowsAsync<Exception>(() => imageService.ProcessImageAsync(inputImage));
    }
}
