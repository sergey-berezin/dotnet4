using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;

public static class MandelbrotSet
{
    public const double BailOut = 5;
    public const int MaxIterations = 100;

    public static int GetNumIterations(double re, double im)
    {
        double bailOutSquare = BailOut * BailOut;
        double z_re = 0;
        double z_im = 0;
        double prev_re = z_re * z_re;
        double prev_im = z_im * z_im;
        for (int i = 0; i < MaxIterations; i++)
        {
            double t_re = prev_re - prev_im + re;
            double t_im = 2 * z_re * z_im + im;
            z_re = t_re;
            z_im = t_im;
            prev_re = z_re * z_re;
            prev_im = z_im * z_im;
            if (prev_re + prev_im > bailOutSquare)
                return i;
        }
        return MaxIterations;
    }

    public static Color GetColor(double re, double im)
    {
        int n = GetNumIterations(re, im);
        if (n < MaxIterations)
        {
            double t = n / (double)MaxIterations;
            return Color.FromRgb(0, (byte)(255 * Math.Sqrt(t)), 0);
        }
        else
            return Colors.Black;
    }


    public static BitmapSource Render(int width,int height,double xmin, double ymin, double xmax, double ymax)
    {
        byte[] pixels = new byte[width * height * 3];
        int p = 0;
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++) {
                Color c = GetColor(xmin + x * (xmax - xmin) / (width - 1),
                                   ymin + y * (ymax - ymin) / (height - 1));
                pixels[p++] = c.R;
                pixels[p++] = c.G;
                pixels[p++] = c.B;
            }

        return BitmapFrame.Create(width, height,
            96, 96,
            PixelFormats.Rgb24, null, pixels, 3 * width);
    }
}