//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Component.ExplodeFireComponent componentExplodeFire { get { return (Component.ExplodeFireComponent)GetComponent(GameComponentsLookup.ComponentExplodeFire); } }
    public bool hasComponentExplodeFire { get { return HasComponent(GameComponentsLookup.ComponentExplodeFire); } }

    public void AddComponentExplodeFire(System.Numerics.Vector2 newDirection, int newTravelTime) {
        var index = GameComponentsLookup.ComponentExplodeFire;
        var component = (Component.ExplodeFireComponent)CreateComponent(index, typeof(Component.ExplodeFireComponent));
        component.Direction = newDirection;
        component.TravelTime = newTravelTime;
        AddComponent(index, component);
    }

    public void ReplaceComponentExplodeFire(System.Numerics.Vector2 newDirection, int newTravelTime) {
        var index = GameComponentsLookup.ComponentExplodeFire;
        var component = (Component.ExplodeFireComponent)CreateComponent(index, typeof(Component.ExplodeFireComponent));
        component.Direction = newDirection;
        component.TravelTime = newTravelTime;
        ReplaceComponent(index, component);
    }

    public void RemoveComponentExplodeFire() {
        RemoveComponent(GameComponentsLookup.ComponentExplodeFire);
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

    static Entitas.IMatcher<GameEntity> _matcherComponentExplodeFire;

    public static Entitas.IMatcher<GameEntity> ComponentExplodeFire {
        get {
            if (_matcherComponentExplodeFire == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ComponentExplodeFire);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherComponentExplodeFire = matcher;
            }

            return _matcherComponentExplodeFire;
        }
    }
}
