//using System;
using my_virtual_pets_api.Services.Interfaces;
using System.Drawing;

namespace PixelationTest
{

    public class Pixelate : IPixelate
    {
        private List<Color> palette = [];

        public Bitmap PixelateImage(Bitmap image, int blockSize, bool usePalette)
        {
            palette.Add(Color.FromArgb(241, 228, 232));
            palette.Add(Color.FromArgb(226, 220, 222));
            palette.Add(Color.FromArgb(206, 177, 190));
            palette.Add(Color.FromArgb(185, 115, 117));
            palette.Add(Color.FromArgb(45, 45, 52));

            Bitmap result = new Bitmap(image.Width, image.Height);

            // Iterate over the image in blocks of 'blockSize'
            for (int x = 0; x < image.Width; x += blockSize)
            {
                for (int y = 0; y < image.Height; y += blockSize)
                {
                    // Calculate the average color of the block
                    (Color averageColor, int averageAlpha) = GetAverageColour(image, x, y, blockSize);

                    //Apply palette
                    if (usePalette && palette.Count > 0)
                    {
                        averageColor = FindClosestPaletteColor(averageColor);
                    }

                    //Hard cutoff for alpha
                    int finalAlpha = averageAlpha >= 128 ? 255 : 0;

                    Color finalColor = Color.FromArgb(finalAlpha, averageColor);

                    // Apply the block color to every pixel in the block
                    for (int bx = 0; bx < blockSize; bx++)
                    {
                        for (int by = 0; by < blockSize; by++)
                        {
                            if (x + bx < image.Width && y + by < image.Height)
                            {
                                result.SetPixel(x + bx, y + by, finalColor);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // average colour of a block within the image
        public (Color, int) GetAverageColour(Bitmap image, int startX, int startY, int blockSize)
        {
            int r = 0, g = 0, b = 0, totalAlpha = 0, count = 0;

            // Loop over each pixel in the block
            for (int x = startX; x < startX + blockSize && x < image.Width; x++)
            {
                for (int y = startY; y < startY + blockSize && y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;
                    totalAlpha += pixel.A;
                    count++;
                }
            }

            //  average for each colour channel
            int avgAplha = totalAlpha / count;
            r /= count;
            g /= count;
            b /= count;
            totalAlpha /= count;

            return (Color.FromArgb(255, r, g, b), avgAplha);
        }

        private Color FindClosestPaletteColor(Color target)
        {
            Color closest = palette[0];
            double minDistance = double.MaxValue;

            foreach (Color color in palette)
            {
                double distance = Math.Pow(color.R - target.R, 2) +
                                  Math.Pow(color.G - target.G, 2) +
                                  Math.Pow(color.B - target.B, 2);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = color;
                }
            }

            return Color.FromArgb(target.A, closest.R, closest.G, closest.B);
        }
    }
}
