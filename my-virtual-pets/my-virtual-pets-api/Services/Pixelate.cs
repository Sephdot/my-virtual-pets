//using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace PixelationTest
{
    public class Pixelate
    {

        public static Bitmap PixelateImage(Bitmap image, int blockSize)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            // Iterate over the image in blocks of 'blockSize'
            for (int x = 0; x < image.Width; x += blockSize)
            {
                for (int y = 0; y < image.Height; y += blockSize)
                {
                    // Calculate the average color of the block
                    Color averageColor = GetAverageColor(image, x, y, blockSize);

                    Color blockColor = averageColor;

                    // Apply the block color to every pixel in the block
                    for (int bx = 0; bx < blockSize; bx++)
                    {
                        for (int by = 0; by < blockSize; by++)
                        {
                            if (x + bx < image.Width && y + by < image.Height)
                            {
                                result.SetPixel(x + bx, y + by, blockColor);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // average colour of a block within the image
        public static Color GetAverageColor(Bitmap image, int startX, int startY, int blockSize)
        {
            int r = 0, g = 0, b = 0, count = 0;

            // Loop over each pixel in the block
            for (int x = startX; x < startX + blockSize && x < image.Width; x++)
            {
                for (int y = startY; y < startY + blockSize && y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;
                    count++;
                }
            }

            //  average for each colour channel
            r /= count;
            g /= count;
            b /= count;

            return Color.FromArgb(r, g, b);
        }
    }
}
