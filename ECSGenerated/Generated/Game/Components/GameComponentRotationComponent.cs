//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Component.RotationComponent componentRotation { get { return (Component.RotationComponent)GetComponent(GameComponentsLookup.ComponentRotation); } }
    public bool hasComponentRotation { get { return HasComponent(GameComponentsLookup.ComponentRotation); } }

    public void AddComponentRotation(float newRot) {
        var index = GameComponentsLookup.ComponentRotation;
        var component = (Component.RotationComponent)CreateComponent(index, typeof(Component.RotationComponent));
        component.Rot = newRot;
        AddComponent(index, component);
    }

    public void ReplaceComponentRotation(float newRot) {
        var index = GameComponentsLookup.ComponentRotation;
        var component = (Component.RotationComponent)CreateComponent(index, typeof(Component.RotationComponent));
        component.Rot = newRot;
        ReplaceComponent(index, component);
    }

    public void RemoveComponentRotation() {
        RemoveComponent(GameComponentsLookup.ComponentRotation);
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

    static Entitas.IMatcher<GameEntity> _matcherComponentRotation;

    public static Entitas.IMatcher<GameEntity> ComponentRotation {
        get {
            if (_matcherComponentRotation == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ComponentRotation);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherComponentRotation = matcher;
            }

            return _matcherComponentRotation;
        }
    }
}
