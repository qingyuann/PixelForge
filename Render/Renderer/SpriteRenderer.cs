using System.Numerics;
using PixelForge.Tools;

namespace Render;

public class SpriteRenderer
{
    float[] _positions;

    public SpriteRenderer()
    {
        _positions = new float[10];
        _positions = _positions.Select(x => RandomTool.Range(0f, 1f)).ToArray();
    }
}