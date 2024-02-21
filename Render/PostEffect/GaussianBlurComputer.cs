using Entitas;
using PixelForge;
using pp;

namespace Render.PostEffect;

public class GaussianBlurComputer : PostProcessComputer
{
    RenderFullscreen _gaussianBlurVertical;
    RenderTexture tempRT1;
    RenderTexture tempRT2;
    int _iterations;
    float _offset;
    static RenderFullscreen _renderFullscreen;
    readonly uint _width = GameSetting.WindowWidth;
    readonly uint _height = GameSetting.WindowHeight;
    readonly uint _halfWidth = GameSetting.WindowWidth / 2;
    readonly uint _halfHeight = GameSetting.WindowHeight / 2;
    readonly uint _quarterWidth = GameSetting.WindowWidth / 4;
    readonly uint _quarterHeight = GameSetting.WindowHeight / 4;
    readonly RenderFullscreen _gaussianBlurHorizontal;

    public GaussianBlurComputer()
    {
        _gaussianBlurVertical = new RenderFullscreen("Blit_CustomUVScale.vert", "PPGaussianBlurVer.frag");
        _gaussianBlurHorizontal = new RenderFullscreen("Blit_CustomUVScale.vert", "PPGaussianBlurHor.frag");
        _renderFullscreen = new RenderFullscreen("Blit.vert", "Blit.frag");
    }

    public override void Render(RenderTexture rt)
    {
        _gaussianBlurVertical.SetUniform("offset", _offset);
        _gaussianBlurHorizontal.SetUniform("offset", _offset);
        
        tempRT1 = RenderTexturePool.Get(_width, _height);
        tempRT2 = RenderTexturePool.Get(_width, _height);
        Blitter.Blit(rt, tempRT1);

        //使用opengl自带的blit不会导致纹理坐标变化，但是使用自己的shader去blit半尺寸的纹理时，需要手动将uv坐标放大一倍（0-0.5 -> 0-1）
        


        if (_iterations >= 1)
        {
            CalculateGaussian(_width, _height, 1, ref tempRT1, ref tempRT2);
            if (_iterations >= 3)
            {
                CalculateGaussian(_halfWidth, _halfHeight, 2, ref tempRT1, ref tempRT2);
                if (_iterations >= 5)
                {
                    for (int i = 0; i < _iterations - 4; i++)
                    {
                        CalculateGaussian(_quarterWidth, _quarterHeight, 4, ref tempRT1, ref tempRT2);
                    }
                }
                if (_iterations >= 4)
                {
                    CalculateGaussian(_halfWidth, _halfHeight, 2, ref tempRT1, ref tempRT2);
                }
            }
            
            CalculateGaussian(_halfWidth, _halfHeight, 2, ref tempRT1, ref tempRT2);
        
            if (_iterations >= 2)
            {
                CalculateGaussian(_width, _height, 1, ref tempRT1, ref tempRT2);
            }
        }

        Blitter.Blit(tempRT1, rt);
        RenderTexturePool.Return(tempRT1);
        RenderTexturePool.Return(tempRT2);
    }

    // in rt1, out rt1
    void CalculateGaussian(uint width, uint height, int uvScale, ref RenderTexture rt1, ref RenderTexture rt2)
    {
        _gaussianBlurVertical.SetUniform("screenWidth", width);
        _gaussianBlurVertical.SetUniform("screenHeight", height);
        _gaussianBlurVertical.SetUniform("_UVScale", uvScale);
        _gaussianBlurHorizontal.SetUniform("screenWidth", width);
        _gaussianBlurHorizontal.SetUniform("screenHeight", height);
        _gaussianBlurHorizontal.SetUniform("_UVScale", uvScale);
        RenderTexturePool.Return(rt2);
        rt2 = RenderTexturePool.Get(width, height);
        Blitter.Blit(rt1, rt2);
        RenderTexturePool.Return(rt1);
        rt1 = RenderTexturePool.Get(width, height);
        Blitter.Blit(rt2, rt1, _gaussianBlurVertical);
        Blitter.Blit(rt1, rt2, _gaussianBlurHorizontal);
        Blitter.Blit(rt2, rt1);
    }

    public override void SetParams(IComponent param)
    {
        if (param is GaussianBlurComponent g)
        {
            _iterations = g.Iterations;
            _offset = g.Offset;
        }
    }

    public override void Dispose()
    {
        RenderTexturePool.Return(tempRT1);
        RenderTexturePool.Return(tempRT2);
    }

    ~GaussianBlurComputer()
    {
        Dispose();
    }
}