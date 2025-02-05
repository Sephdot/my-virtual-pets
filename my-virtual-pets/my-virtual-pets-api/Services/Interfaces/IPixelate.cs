using System.Drawing;

namespace my_virtual_pets_api.Services.Interfaces
{
    public interface IPixelate
    {
        public (Color, int) GetAverageColour(Bitmap image, int startX, int startY, int blockSize);

        public Bitmap PixelateImage(Bitmap image, int blockSize, bool usePalette = false);
    }
}
