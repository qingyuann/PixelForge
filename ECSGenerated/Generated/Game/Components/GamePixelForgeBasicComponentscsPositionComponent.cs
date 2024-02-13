//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public PixelForge.BasicComponents.cs.Position pixelForgeBasicComponentscsPosition { get { return (PixelForge.BasicComponents.cs.Position)GetComponent(GameComponentsLookup.PixelForgeBasicComponentscsPosition); } }
    public bool hasPixelForgeBasicComponentscsPosition { get { return HasComponent(GameComponentsLookup.PixelForgeBasicComponentscsPosition); } }

    public void AddPixelForgeBasicComponentscsPosition(float newX, float newY) {
        var index = GameComponentsLookup.PixelForgeBasicComponentscsPosition;
        var component = (PixelForge.BasicComponents.cs.Position)CreateComponent(index, typeof(PixelForge.BasicComponents.cs.Position));
        component.X = newX;
        component.Y = newY;
        AddComponent(index, component);
    }

    public void ReplacePixelForgeBasicComponentscsPosition(float newX, float newY) {
        var index = GameComponentsLookup.PixelForgeBasicComponentscsPosition;
        var component = (PixelForge.BasicComponents.cs.Position)CreateComponent(index, typeof(PixelForge.BasicComponents.cs.Position));
        component.X = newX;
        component.Y = newY;
        ReplaceComponent(index, component);
    }

    public void RemovePixelForgeBasicComponentscsPosition() {
        RemoveComponent(GameComponentsLookup.PixelForgeBasicComponentscsPosition);
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

    static Entitas.IMatcher<GameEntity> _matcherPixelForgeBasicComponentscsPosition;

    public static Entitas.IMatcher<GameEntity> PixelForgeBasicComponentscsPosition {
        get {
            if (_matcherPixelForgeBasicComponentscsPosition == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.PixelForgeBasicComponentscsPosition);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherPixelForgeBasicComponentscsPosition = matcher;
            }

            return _matcherPixelForgeBasicComponentscsPosition;
        }
    }
}