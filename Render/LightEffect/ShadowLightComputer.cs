using System.Numerics;
using Component;
using Entitas;
using Light;
using PixelForge;
using PixelForge.Tools;
using pp;
using Silk.NET.OpenGL;

namespace Render.PostEffect;

public class ShadowLightComputer : LightEffectComputer
{
    //sooooo expensive...
    const int ShadowLightPrecisionAngular = 360;
    const int ShadowLightPrecisionMarch = 400;
    Vector3 _color;
    float _intensity;
    Vector2 _position;
    float _radius;
    float _volume;
    float _radialFallOff;
    float _angle;
    Texture _lightMap;
    RenderTexture _shadowMap;
    byte[] _screenData = Array.Empty<byte>();
    byte[] _lightData = Array.Empty<byte>();
    byte[] _shadowData = Array.Empty<byte>();
    RenderFullscreen _shadowLightShadowMap;
    RenderFullscreen _ShadowLightDraw;

    public ShadowLightComputer()
    {
        _shadowMap = TexturePool.GetRT(ShadowLightPrecisionAngular, 1, false);
        _shadowLightShadowMap = new RenderFullscreen("Blit_CustomUVScale.vert", "ShadowLightShadowMap.frag");
        _ShadowLightDraw = new RenderFullscreen("Blit.vert", "ShadowLightDraw.frag");
        _shadowData = new byte[ShadowLightPrecisionAngular * 4];
        
        
    }

    public override void Render(RenderTexture rt)
    {
        //////////////////////////////////////////////////////
        //// step1: cut the light map from render texture ////
        //////////////////////////////////////////////////////
        //get the screen data
        if (_screenData.Length != rt.Width * rt.Height * 4)
        {
            _screenData = new byte[rt.Width * rt.Height * 4];
        }

        rt.GetImage(_screenData);

        //get the light data
        int radiusPixelSize = (int)Transform.WorldToPixelSize(_radius);
        if (_lightData.Length != radiusPixelSize * 2 * radiusPixelSize * 2 * 4)
        {
            _lightData = new byte[radiusPixelSize * 2 * radiusPixelSize * 2 * 4];
        }

        //clear the light data
        Array.Fill(_lightData, (byte)0);

        Vector2 posPixCenter = Transform.WorldToPixel(_position, true);

        //copy the screen data to light data within the lightMap
        for (int i = 0; i < radiusPixelSize * 2; i++)
        {
            var screenY = (int)posPixCenter.Y - radiusPixelSize + i;
            if (screenY < 0 || screenY >= rt.Height)
            {
                continue;
            }

            var lightY = i;
            var screenX = (int)posPixCenter.X - radiusPixelSize;
            var length = radiusPixelSize * 2;
            var lightX = 0;
            if (screenX < 0)
            {
                lightX = -screenX;
                length += screenX;
                screenX = 0;
                // Console.WriteLine( "light left < 0, lightX=" + lightX + ", length=" + length );
            }

            if (screenX + length > rt.Width)
            {
                length = rt.Width - screenX;
                // Console.WriteLine( "light right > screen right, length=" + length );
            }

            if (length <= 0)
            {
                // Console.WriteLine( "length < 0" );
                continue;
            }

            var screenIndex = Image.TryGetIndex(screenX, screenY, rt.Width, rt.Height);
            if (screenIndex is null)
            {
                Console.WriteLine("screenIndex is null");
                continue;
            }

            var lightIndex = Image.TryGetIndex(lightX, lightY, radiusPixelSize * 2, radiusPixelSize * 2);
            if (lightIndex is null)
            {
                Console.WriteLine("lightIndex is null");
                continue;
            }

            Array.Copy(_screenData, screenIndex.Value * 4, _lightData, lightIndex.Value * 4, length * 4);
            // var color = Image.TryGetColorPixelRGBA( _lightData, radiusPixelSize*2-1, 0, radiusPixelSize * 2, radiusPixelSize * 2 );
            // Console.WriteLine( color );
        }

        /////////////////////////////////////////////////////
        //// step2: render the shadow map from light map ////
        /////////////////////////////////////////////////////
        _lightMap = TexturePool.GetTex((uint)radiusPixelSize * 2, (uint)radiusPixelSize * 2);
        _lightMap.UpdateImageContent(_lightData, (uint)radiusPixelSize * 2, (uint)radiusPixelSize * 2);
        var uvScale = GameSetting.WindowWidth / (float)ShadowLightPrecisionAngular;
        _shadowMap.RenderToRt();
        _shadowLightShadowMap.SetTexture("_BlitTexture", _lightMap);
        //maximum marching is 400
        _shadowLightShadowMap.SetUniform("resolution", Math.Min(ShadowLightPrecisionMarch*_radius,ShadowLightPrecisionMarch));
        _shadowLightShadowMap.SetUniform("_UVScale", uvScale);
        _shadowLightShadowMap.Draw();
        GlobalVariable.GL.Finish();
        TexturePool.ReturnTex(_lightMap);

        // #region testShadowMap
        //  _shadowMap.GetImage(_shadowData);
        //  var col = new List<Vector4>();
        //  for (int i = 0; i < ShadowLightPrecisionAngular; i++)
        //  {
        //  	col.Add(Image.TryGetColorPixelRGBA( _shadowData, i, 0, ShadowLightPrecisionAngular, 1 ));
        //  }
        //  col.Select((i,j)=>new {i,j}).ToList().ForEach(i=>Console.WriteLine(i.j + " " + i.i));
        // #endregion

        ////////////////////////////////////
        //// step3: render the 2d light ////
        ////////////////////////////////////
        RenderTexture tempRt = TexturePool.GetRT((uint)rt.Width, (uint)rt.Height, false);
    	tempRt.RenderToRt();
        _ShadowLightDraw.SetTexture("_ShadowMap", _shadowMap);
        _ShadowLightDraw.SetUniform("screenW", rt.Width);
        _ShadowLightDraw.SetUniform("screenH", rt.Height);
        _ShadowLightDraw.SetUniform("lightPosPix", posPixCenter);
        _ShadowLightDraw.SetUniform("lightRadiusPix", radiusPixelSize);
        _ShadowLightDraw.SetUniform("lightColor", _color);
        _ShadowLightDraw.SetUniform("falloff", _radialFallOff);
        _ShadowLightDraw.SetUniform("intensity",    _intensity);
        Blitter.Blit(rt, tempRt, _ShadowLightDraw);
        Blitter.Blit(tempRt, rt);
        TexturePool.ReturnRT(tempRt);
    }

    public override void SetParams(IComponent param)
    {
        if (param is ShadowLightComponent globalLightComponent)
        {
            _color = globalLightComponent.Color;
            _intensity = globalLightComponent.Intensity;
            _radius = globalLightComponent.Radius;
            _volume = globalLightComponent.Volume;
            _radialFallOff = globalLightComponent.RadialFallOff;
            _angle = globalLightComponent.Angle;
        }

        if (param is PositionComponent positionComponent)
        {
            _position =new Vector2(positionComponent.X, positionComponent.Y);
        }
    }

    public override void Dispose()
    {
    }
}