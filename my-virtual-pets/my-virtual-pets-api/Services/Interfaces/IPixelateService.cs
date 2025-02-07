namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IPixelateService
    {
        public byte[] PixelateImage(byte[] image, int blockSize, bool usePalette = false);
    }
}
