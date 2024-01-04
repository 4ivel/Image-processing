using System;
using System.Drawing;

class Filter
{
    public static int totalProgress;
    public static int currentProgress;
    public static Bitmap ApplyEdgeDetection(Bitmap inputImage)
    {
        int width = inputImage.Width;
        int height = inputImage.Height;

        totalProgress = inputImage.Width * inputImage.Height;

        Bitmap outputImage = new Bitmap(width, height);

        int[,] sobelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        int[,] sobelY = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                int gxR = 0, gyR = 0;
                int gxG = 0, gyG = 0;
                int gxB = 0, gyB = 0;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Color pixel = inputImage.GetPixel(x + i, y + j);

                        gxR += sobelX[i + 1, j + 1] * pixel.R;
                        gyR += sobelY[i + 1, j + 1] * pixel.R;

                        gxG += sobelX[i + 1, j + 1] * pixel.G;
                        gyG += sobelY[i + 1, j + 1] * pixel.G;

                        gxB += sobelX[i + 1, j + 1] * pixel.B;
                        gyB += sobelY[i + 1, j + 1] * pixel.B;
                    }
                }

                int edgeR = Math.Min(255, Math.Max(0, Math.Abs(gxR) + Math.Abs(gyR)));
                int edgeG = Math.Min(255, Math.Max(0, Math.Abs(gxG) + Math.Abs(gyG)));
                int edgeB = Math.Min(255, Math.Max(0, Math.Abs(gxB) + Math.Abs(gyB)));

                outputImage.SetPixel(x, y, Color.FromArgb(edgeR, edgeG, edgeB));
                currentProgress++;
            }
        }
        currentProgress = 0;
        return outputImage;
    }
}
