using Entitas;

namespace PixelForge.Light;

public class LightSystem : IExecuteSystem, IInitializeSystem
{
    IGroup _circleLights;
    IGroup _globalLights;
    private Contexts _contexts;

    public LightSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        _circleLights = _contexts.game.GetGroup(GameMatcher.LightCircleLight);
        _globalLights = _contexts.game.GetGroup(GameMatcher.LightGlobalLight);
    }


    public void Execute()
    {
    }

    void RenderLights()
    {
        RenderGlobalLights();
        RenderCircleLights();
    }
    
    void RenderGlobalLights()
    {
        
    }

    void RenderCircleLights()
    {
        
    }
}