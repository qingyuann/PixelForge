//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Component.SizeComponent componentSize { get { return (Component.SizeComponent)GetComponent(GameComponentsLookup.ComponentSize); } }
    public bool hasComponentSize { get { return HasComponent(GameComponentsLookup.ComponentSize); } }

    public void AddComponentSize(float newX, float newY) {
        var index = GameComponentsLookup.ComponentSize;
        var component = (Component.SizeComponent)CreateComponent(index, typeof(Component.SizeComponent));
        component.X = newX;
        component.Y = newY;
        AddComponent(index, component);
    }

    public void ReplaceComponentSize(float newX, float newY) {
        var index = GameComponentsLookup.ComponentSize;
        var component = (Component.SizeComponent)CreateComponent(index, typeof(Component.SizeComponent));
        component.X = newX;
        component.Y = newY;
        ReplaceComponent(index, component);
    }

    public void RemoveComponentSize() {
        RemoveComponent(GameComponentsLookup.ComponentSize);
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

    static Entitas.IMatcher<GameEntity> _matcherComponentSize;

    public static Entitas.IMatcher<GameEntity> ComponentSize {
        get {
            if (_matcherComponentSize == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ComponentSize);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherComponentSize = matcher;
            }

            return _matcherComponentSize;
        }
    }
}
