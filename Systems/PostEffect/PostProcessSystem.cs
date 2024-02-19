using Entitas;
using pp;
using Render;
using Render.PostEffect;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PixelForge;

public sealed class PostProcessSystem : IInitializeSystem, IExecuteSystem
{
    //set a singleton
    static PostProcessSystem _postProcessSystem;

    public static PostProcessSystem Instance
    {
        get
        {
            if (_postProcessSystem is null)
            {
                _postProcessSystem = new PostProcessSystem();
            }

            return _postProcessSystem;
        }
    }


    // pair of post process component and its computer
    private static Dictionary<Type, Type> _ppTypes = new Dictionary<Type, Type>()
    {
        { typeof(BloomComponnet), typeof(BloomComputer) },
        { typeof(GaussianBlurComponent), typeof(GaussianBlurComputer) },
    };

    //store all post process components
    private Dictionary<IComponent, PostProcessComputer> _allPP = new Dictionary<IComponent, PostProcessComputer>();


    Contexts _contexts;
    GameEntity _ppEntity;
    Dictionary<IComponent, PostProcessComputer> _computers = new Dictionary<IComponent, PostProcessComputer>();

    public void Initialize()
    {
        _contexts = Contexts.sharedInstance;
        _ppEntity = _contexts.game.ppLightSettingEntity;
        foreach (var c in _ppEntity.GetComponents())
        {
            foreach (var pp in _ppTypes)
            {
                if (pp.Key == c.GetType())
                {
                    Debug.Log("pp" + pp.Key);
                }
            }
        }
    }

    void SetUpPostEffects(GameEntity entity)
    {
    }

    public void Execute()
    {
    }

    public static void RenderPostProcess(int layer, RenderTexture rt)
    {
    }
}