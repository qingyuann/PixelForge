//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Component.FireComponent componentFire { get { return (Component.FireComponent)GetComponent(GameComponentsLookup.ComponentFire); } }
    public bool hasComponentFire { get { return HasComponent(GameComponentsLookup.ComponentFire); } }

    public void AddComponentFire(int newLifeTime, int newSpreadTime) {
        var index = GameComponentsLookup.ComponentFire;
        var component = (Component.FireComponent)CreateComponent(index, typeof(Component.FireComponent));
        component.LifeTime = newLifeTime;
        component.SpreadTime = newSpreadTime;
        AddComponent(index, component);
    }

    public void ReplaceComponentFire(int newLifeTime, int newSpreadTime) {
        var index = GameComponentsLookup.ComponentFire;
        var component = (Component.FireComponent)CreateComponent(index, typeof(Component.FireComponent));
        component.LifeTime = newLifeTime;
        component.SpreadTime = newSpreadTime;
        ReplaceComponent(index, component);
    }

    public void RemoveComponentFire() {
        RemoveComponent(GameComponentsLookup.ComponentFire);
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

    static Entitas.IMatcher<GameEntity> _matcherComponentFire;

    public static Entitas.IMatcher<GameEntity> ComponentFire {
        get {
            if (_matcherComponentFire == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ComponentFire);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherComponentFire = matcher;
            }

            return _matcherComponentFire;
        }
    }
}