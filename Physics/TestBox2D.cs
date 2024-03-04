using System.Numerics;
using Box2DX;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace PixelForge.Physics;


public class TestBox2D
{
    public World MyWorld = new World(new AABB(), new Vec2(0, -10), true);
    
}