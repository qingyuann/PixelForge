//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public pp.GaussianBlurComponent ppGaussianBlur { get { return (pp.GaussianBlurComponent)GetComponent(GameComponentsLookup.ppGaussianBlur); } }
    public bool hasppGaussianBlur { get { return HasComponent(GameComponentsLookup.ppGaussianBlur); } }

    public void AddppGaussianBlur(bool newEnabled, int[] newLayers, float newOffset, int newIterations, Render.PostEffect.GaussianBlurComputer newComputer) {
        var index = GameComponentsLookup.ppGaussianBlur;
        var component = (pp.GaussianBlurComponent)CreateComponent(index, typeof(pp.GaussianBlurComponent));
        component.Enabled = newEnabled;
        component.Layers = newLayers;
        component.Offset = newOffset;
        component.Iterations = newIterations;
        component.Computer = newComputer;
        AddComponent(index, component);
    }

    public void ReplaceppGaussianBlur(bool newEnabled, int[] newLayers, float newOffset, int newIterations, Render.PostEffect.GaussianBlurComputer newComputer) {
        var index = GameComponentsLookup.ppGaussianBlur;
        var component = (pp.GaussianBlurComponent)CreateComponent(index, typeof(pp.GaussianBlurComponent));
        component.Enabled = newEnabled;
        component.Layers = newLayers;
        component.Offset = newOffset;
        component.Iterations = newIterations;
        component.Computer = newComputer;
        ReplaceComponent(index, component);
    }

    public void RemoveppGaussianBlur() {
        RemoveComponent(GameComponentsLookup.ppGaussianBlur);
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

    static Entitas.IMatcher<GameEntity> _matcherppGaussianBlur;

    public static Entitas.IMatcher<GameEntity> ppGaussianBlur {
        get {
            if (_matcherppGaussianBlur == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ppGaussianBlur);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherppGaussianBlur = matcher;
            }

            return _matcherppGaussianBlur;
        }
    }
}