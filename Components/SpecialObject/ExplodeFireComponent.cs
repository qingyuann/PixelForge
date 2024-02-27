using System.Numerics;
using Entitas;

namespace Component;

[Game]
public class ExplodeFireComponent : IComponent
{
    public Vector2 Direction;
    public int TravelTime;
}