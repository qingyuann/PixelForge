using System.Numerics;
using Entitas;
using pp;

namespace Light;

[Game]
public class ShadowLightComponent : IComponent,ILightComponent
{
    public bool Enabled { get; set; }
    public int[] Layers { get; set; }
    public int LightOrder { get; set; }
    public float Radius;
    public float Intensity;
    //how much the background will be lighted
    public float Volume;
    public Vector3 Color;
    //attenuation of light with respect to distance
    public float RadialFallOff;
    //how much the edge of pic will be lighted
    public float EdgeInfringe;
    // algorithm:https://github.com/mattdesl/lwjgl-basics/wiki/2D-Pixel-Perfect-Shadows
}