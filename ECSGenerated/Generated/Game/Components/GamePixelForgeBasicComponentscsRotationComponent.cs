//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public PixelForge.BasicComponents.cs.Rotation pixelForgeBasicComponentscsRotation { get { return (PixelForge.BasicComponents.cs.Rotation)GetComponent(GameComponentsLookup.PixelForgeBasicComponentscsRotation); } }
    public bool hasPixelForgeBasicComponentscsRotation { get { return HasComponent(GameComponentsLookup.PixelForgeBasicComponentscsRotation); } }

    public void AddPixelForgeBasicComponentscsRotation(float newRot) {
        var index = GameComponentsLookup.PixelForgeBasicComponentscsRotation;
        var component = (PixelForge.BasicComponents.cs.Rotation)CreateComponent(index, typeof(PixelForge.BasicComponents.cs.Rotation));
        component.Rot = newRot;
        AddComponent(index, component);
    }

    public void ReplacePixelForgeBasicComponentscsRotation(float newRot) {
        var index = GameComponentsLookup.PixelForgeBasicComponentscsRotation;
        var component = (PixelForge.BasicComponents.cs.Rotation)CreateComponent(index, typeof(PixelForge.BasicComponents.cs.Rotation));
        component.Rot = newRot;
        ReplaceComponent(index, component);
    }

    public void RemovePixelForgeBasicComponentscsRotation() {
        RemoveComponent(GameComponentsLookup.PixelForgeBasicComponentscsRotation);
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

    static Entitas.IMatcher<GameEntity> _matcherPixelForgeBasicComponentscsRotation;

    public static Entitas.IMatcher<GameEntity> PixelForgeBasicComponentscsRotation {
        get {
            if (_matcherPixelForgeBasicComponentscsRotation == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.PixelForgeBasicComponentscsRotation);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherPixelForgeBasicComponentscsRotation = matcher;
            }

            return _matcherPixelForgeBasicComponentscsRotation;
        }
    }
}