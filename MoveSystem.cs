using System.Numerics;

namespace PixelForge
{
    /// <summary>
    /// basic movement, inherits this class to implement game movement system
    /// </summary>
    class MoveSystem:ISystem
    {
        public void Update(EntityManager entityManager, double deltaTime)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponent<PositionComponent>())
            {
                var position = entityManager.GetComponent<PositionComponent>(entity);
                var velocity = entityManager.GetComponent<VelocityComponent>(entity);
                var autoMove = entityManager.GetComponent<AutoMoveComponent>(entity);
                
                if (position == null || velocity == null || !autoMove.AutoMove)
                {
                    continue;
                }
                
                position.Position += velocity.Velocity * (float)deltaTime;
            }
        }
        
    }
    
}