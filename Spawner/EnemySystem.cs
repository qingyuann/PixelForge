using Entitas;
using PixelForge.BasicComponents;

namespace PixelForge.Spawner;

/// <summary>
/// inherit from IExecuteSystem, execute every frame
/// </summary>
public sealed class EnemySystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly int _enemyCount;
    
    public EnemySystem(Contexts contexts, int enemyCount)
    {
        _contexts = contexts;
        _enemyCount = enemyCount;
    }
    
    public void Initialize()
    {
        for (var i = 0; i < _enemyCount; i++)
        {
            var entity = _contexts.game.CreateEntity();
            entity.AddPixelForgeBasicComponentsPosition(0.0f,0.0f);
        }
        
    }
    
    public void Execute()
    {
        AutoMove();
        
    }
    
    private void AutoMove()
    {
        var entities = _contexts.game.GetEntities();
        foreach (var entity in entities)
        {
            if(entity.hasPixelForgeBasicComponentsPosition)
            {
                var position = entity.pixelForgeBasicComponentsPosition;
                position.X += 0.1f;
                position.Y += 0.1f;
                entity.ReplacePixelForgeBasicComponentsPosition(position.X, position.Y);

                Console.WriteLine("Entity Auto Move!");
            }
        }
    }
}
