using Entitas;
using Light;
using pp;
using Render;
using Render.PostEffect;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace PixelForge.Light;

public class LightSystem : IExecuteSystem, IInitializeSystem
{
    public static LightSystem Instance { get; private set; }

    Dictionary<Type, Type> _lightComputers = new Dictionary<Type, Type>()
    {
        { typeof(GlobalLightComponent), typeof(GlobalLightComputer) },
        { typeof(CircleLightComponent), typeof(CircleLightComputer) },
        { typeof(ShadowLightComponent), typeof(ShadowLightComputer) },
    };

    List<IMatcher<GameEntity>> _matchers = new List<IMatcher<GameEntity>>()
    {
        GameMatcher.LightGlobalLight,
        GameMatcher.LightCircleLight,
        GameMatcher.LightShadowLight,
    };

    /************************************************************************/
    IGroup<GameEntity> _lightsGroup;

    Contexts _contexts;

    //lightComponent,  computer
    static Dictionary<Type, List<(IComponent, LightEffectComputer)>> _computers =
        new Dictionary<Type, List<(IComponent, LightEffectComputer)>>();

    public LightSystem(Contexts contexts)
    {
        _contexts = contexts;
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception("LightSystem is a singleton");
        }
    }

    public void Initialize()
    {
        _lightsGroup = _contexts.game.GetGroup(GameMatcher.AnyOf(_matchers.ToArray()));
        foreach (var e in _lightsGroup)
        {
            AddNewComputer(e);
        }

        _lightsGroup.OnEntityAdded += (_, entity, _, _) => { AddNewComputer(entity); };
    }

    void AddNewComputer(GameEntity e)
    {
        foreach (var c in e.GetComponents())
        {
            foreach (var light in _lightComputers)
            {
                if (light.Key == c.GetType())
                {
                    var computer = (LightEffectComputer)Activator.CreateInstance(light.Value)!;
                    if (_computers.ContainsKey(c.GetType()))
                    {
                        var list = _computers[c.GetType()];
                        list.Add((c, computer));
                        list = list.OrderBy(c => ((ILightComponent)c.Item1).LightOrder).ToList();
                        _computers[c.GetType()] = list;
                    }
                    else
                    {
                        _computers.Add(c.GetType(), new List<(IComponent, LightEffectComputer)>() { (c, computer) });
                    }

                    break;
                }
            }
        }
    }

    public void Execute()
    {
    }


    public static void RenderLights(int layer, RenderTexture rt)
    {
        // //Render Global Lights
        // RenderLight( rt, layer, typeof( GlobalLightComponent ) );
        //
        // //Render Other Lights
        // var otherLights = _computers.Where( c => c.Key != typeof( GlobalLightComponent ) && c.Key != typeof( ShadowLightComponent ) );
        // foreach( var light in otherLights ) {
        // 	RenderLight( rt, layer, light.Key );
        // }

        //Render Shadow Lights
        RenderLight(rt, layer, typeof(ShadowLightComponent));
    }

    static void RenderLight(RenderTexture rt, int layer, Type type)
    {
        _computers.TryGetValue(type, out var lights);
        if (lights?.Count > 0)
        {
            var computerInLayer = lights.Where(c => ((ILightComponent)c.Item1).Layers.Contains(layer)).ToList();

            for (int i = 0; i < computerInLayer.Count; i++)
            {
                var c = computerInLayer[i];
                c.Item2.SetParams(c.Item1);
                c.Item2.Render(rt);
            }
        }
    }
}