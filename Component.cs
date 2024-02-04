using System.Numerics;

namespace PixelForge
{
    struct PositionComponent
    {
        public Vector2 Position { get; set; }
    }

    struct AutoMoveComponent
    {
        public bool AutoMove { get; set; }
    }
    
    struct VelocityComponent
    {
        public Vector2 Velocity { get; set; }
    }
    
    struct TransformComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
    }
    
    struct TranslationComponent
    {
        public Vector2 Translation { get; set; }
    }
    
    struct RotationComponent
    {
        public float Rotation { get; set; }
    }
    
    /// <summary>
    /// use this component to declare that this body will participate in the collision queries
    /// </summary>
    struct PhysicsColliderComponent
    {
        
    }
}

