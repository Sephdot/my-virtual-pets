using my_virtual_pets_api.Services.Interfaces;
using SkiaSharp;
namespace PixelationTest
{

    public class PixelateService : IPixelateService
    {
        private List<SKColor> palette = [];

        public byte[] PixelateImage(byte[] input, int blockCount, bool isShiny)
        {
            SKBitmap image;
            using (var stream = new SKMemoryStream(input))
            {
                image = SKBitmap.Decode(stream);
            }
            int blockSize = Math.Min(image.Width, image.Height) / blockCount;

            if (isShiny)
            {
                palette.Add(new SKColor(223, 200, 200));
                palette.Add(new SKColor(180, 126, 152));
                palette.Add(new SKColor(123, 50, 104));
                palette.Add(new SKColor(58, 29, 53));
                palette.Add(new SKColor(5, 4, 4));
            }
            else
            {
                palette.Add(new SKColor(186, 178, 166));
                palette.Add(new SKColor(129, 114, 106));
                palette.Add(new SKColor(96, 75, 67));
                palette.Add(new SKColor(32, 23, 20));
                palette.Add(new SKColor(17, 9, 9));
            }

            SKBitmap result = new SKBitmap(image.Width, image.Height);

            // Iterate over the image in blocks of 'blockSize'
            for (int x = 0; x < image.Width; x += blockSize)
            {
                for (int y = 0; y < image.Height; y += blockSize)
                {
                    // Calculate the average color of the block
                    (SKColor averageColor, byte averageAlpha) = GetAverageColour(image, x, y, blockSize);

                    //Apply palette
                    averageColor = FindClosestPaletteColor(averageColor);

                    //Hard cutoff for alpha
                    byte finalAlpha = averageAlpha >= 128 ? (byte)255 : (byte)0;

                    SKColor finalColor = new SKColor(averageColor.Red, averageColor.Green, averageColor.Blue, finalAlpha);

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

            byte[] output;
            using (var stream = new SKDynamicMemoryWStream())
            {
                result.Encode(stream, SKEncodedImageFormat.Png, 100);
                SKData data = stream.DetachAsData();
                output = data.ToArray();
            }
            return output;
        }

        // average colour of a block within the image
        private (SKColor, byte) GetAverageColour(SKBitmap image, int startX, int startY, int blockSize)
        {
            int r = 0, g = 0, b = 0, totalAlpha = 0, count = 0;

            // Loop over each pixel in the block
            for (int x = startX; x < startX + blockSize && x < image.Width; x++)
            {
                for (int y = startY; y < startY + blockSize && y < image.Height; y++)
                {
                    SKColor pixel = image.GetPixel(x, y);
                    r += pixel.Red;
                    g += pixel.Green;
                    b += pixel.Blue;
                    totalAlpha += pixel.Alpha;
                    count++;
                }
            }

            if (count == 0) return (new SKColor(0, 0, 0, 0), 0);

            //  average for each colour channel
            byte avgAlpha = (byte)(totalAlpha / count);
            byte avgR = (byte)(r / count);
            byte avgG = (byte)(g / count);
            byte avgB = (byte)(b / count);

            return (new SKColor(avgR, avgG, avgB, avgAlpha), avgAlpha);
        }

        private SKColor FindClosestPaletteColor(SKColor target)
        {
            SKColor closest = palette[0];
            double minDistance = double.MaxValue;

            foreach (SKColor color in palette)
            {
                double distance = Math.Pow(color.Red - target.Red, 2) +
                                  Math.Pow(color.Green - target.Green, 2) +
                                  Math.Pow(color.Blue - target.Blue, 2);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = color;
                }
            }

            return new SKColor(closest.Red, closest.Green, closest.Blue, target.Alpha);
        }
    }
}
