using System.Numerics;
using Entitas;
using pp;

namespace Light;

[Game]
public class GlobalLightComponent : IComponent, ILightComponent
{
    public bool Enabled { get; set; }
    public int[] Layers { get; set; }
    public int LightOrder { get; set; }
    public Vector3 Color;
    public Vector3 Intensity;
}