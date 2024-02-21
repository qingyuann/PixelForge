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
    public Vector2 Position;
    public float Radius;
    public float Intensity;
    public float Volume;
    public Vector3 Color;
    public float RadialFallOff;
    public float Angle;
    //radialFalloff=pow(1-distance,radialFalloff)
    //angle=smoothstep(maxAngle,minAngle,angle) (symmetric) 
    //todo: support normal map
    //normal map:n·l
    //final=intensity*color*radialFalloff*angle*normalEffect
    //shadeCol=baseCol*final
    //shadeCol+=Volume*Color
    //store the shadow in stencil
    
    // algorithm:https://github.com/mattdesl/lwjgl-basics/wiki/2D-Pixel-Perfect-Shadows
    //https://ahamnett.blogspot.com/2013/05/2d-shadows-shader.html
    // https://www.youtube.com/watch?v=eyDUco5zzLU
}