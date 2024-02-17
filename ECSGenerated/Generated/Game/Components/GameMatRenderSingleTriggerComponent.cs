//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Mat.RenderSingleTriggerComponent matRenderSingleTrigger { get { return (Mat.RenderSingleTriggerComponent)GetComponent(GameComponentsLookup.MatRenderSingleTrigger); } }
    public bool hasMatRenderSingleTrigger { get { return HasComponent(GameComponentsLookup.MatRenderSingleTrigger); } }

    public void AddMatRenderSingleTrigger(bool newIsVisible, int newLayer) {
        var index = GameComponentsLookup.MatRenderSingleTrigger;
        var component = (Mat.RenderSingleTriggerComponent)CreateComponent(index, typeof(Mat.RenderSingleTriggerComponent));
        component.IsVisible = newIsVisible;
        component.Layer = newLayer;
        AddComponent(index, component);
    }

    public void ReplaceMatRenderSingleTrigger(bool newIsVisible, int newLayer) {
        var index = GameComponentsLookup.MatRenderSingleTrigger;
        var component = (Mat.RenderSingleTriggerComponent)CreateComponent(index, typeof(Mat.RenderSingleTriggerComponent));
        component.IsVisible = newIsVisible;
        component.Layer = newLayer;
        ReplaceComponent(index, component);
    }

    public void RemoveMatRenderSingleTrigger() {
        RemoveComponent(GameComponentsLookup.MatRenderSingleTrigger);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMatRenderSingleTrigger;

    public static Entitas.IMatcher<GameEntity> MatRenderSingleTrigger {
        get {
            if (_matcherMatRenderSingleTrigger == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MatRenderSingleTrigger);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMatRenderSingleTrigger = matcher;
            }

            return _matcherMatRenderSingleTrigger;
        }
    }
}
