using System.Numerics;
using Entitas;
using pp;

namespace Light;

[Game]
public class CircleLightComponent : IComponent,ILightComponent
{
    public bool Enabled { get; set; }
    public int[] Layers { get; set; }
    public int LightOrder { get; set; }
    public float Radius;
    public float Intensity;
    public float Volume;
    public Vector3 Color;
    public float RadialFallOff;
    public float Angle;
}