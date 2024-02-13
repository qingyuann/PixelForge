namespace PixelForge.Tools;

public static class RandomTool
{
    private static Random _random;
    static RandomTool()
    {
        _random = new Random();
    }
    
    public static float Float()
    {
        return (float)_random.NextDouble();
    }
     
    public static float Range(float min, float max)
    {
        return min + (max - min) * (float)_random.NextDouble();
    }
    
    public static int Range(int min, int max)
    {
        return _random.Next(min, max);
    }

}