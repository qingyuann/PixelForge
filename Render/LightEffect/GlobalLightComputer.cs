using System.Numerics;
using Entitas;
using Light;
using pp;

namespace Render.PostEffect;

public class GlobalLightComputer : LightEffectComputer
{
    Vector3 _color;
    Vector3 _intensity;
    private RenderTexture _tempRt1;
    public override void Render(RenderTexture rt)
    {
        _tempRt1 = RenderTexturePool.Get((uint)rt.Width, (uint)rt.Height);
        Blitter.Blit(rt, _tempRt1);
    }

    public override void SetParams(IComponent param)
    {
        if (param is GlobalLightComponent globalLightComponent)
        {
            _color = globalLightComponent.Color;
            _intensity = globalLightComponent.Intensity;
        }
    }

    public override void Dispose()
    {
    }
}