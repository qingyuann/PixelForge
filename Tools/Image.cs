using System.Numerics;

namespace PixelForge.Tools;

public static partial class Image
{
    public static Vector4 GetColorPixelRGBA(byte[] data, int pixelX, int pixelY, int width)
    {
        var index = GetIndex(pixelX, pixelY, width);
        return new Vector4(data[index * 4], data[index * 4 + 1], data[index * 4 + 2], data[index * 4 + 3]);
    }

    public static void SetColorRGBA(ref byte[] data, int pixelX, int pixelY, int width, Vector4 color)
    {
        var index = GetIndex(pixelX, pixelY, width);
        data[index * 4] = (byte)color.X;
        data[index * 4 + 1] = (byte)color.Y;
        data[index * 4 + 2] = (byte)color.Z;
        data[index * 4 + 3] = (byte)color.W;
    }

    public static int GetIndex(int pixelX, int pixelY, int width)
    {
        return pixelX + pixelY * width;
    }
}