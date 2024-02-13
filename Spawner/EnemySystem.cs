using Entitas;

namespace PixelForge.Spawner;

/// <summary>
/// inherit from IExecuteSystem, execute every frame
/// </summary>
public sealed class EnemySystem : IInitializeSystem, IExecuteSystem
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
            entity.AddPixelForgeBasicComponentscsPosition(0.0f + i*0.1f,0.0f);
            //Console.WriteLine("Enemy Created!");
        }
        
    }
    
    public void Execute()
    {
        AutoMove();
    }
    
    private void AutoMove()
    {
        var entities = _contexts.game.GetEntities();
       
        //Console.WriteLine(entities.Length);
        foreach (var entity in entities)
        {
            if(entity.hasPixelForgeBasicComponentscsPosition)
            {
                var position = entity.pixelForgeBasicComponentscsPosition;
                position.X += 0.01f;
                position.Y += 0.01f;
                entity.ReplacePixelForgeBasicComponentscsPosition(position.X, position.Y);
                //Console.WriteLine("position.X" + position.X);
            }
        }
    }
}