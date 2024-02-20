//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly Component.CellAutoTexture componentCellAutoTextureComponent = new Component.CellAutoTexture();

    public bool isComponentCellAutoTexture {
        get { return HasComponent(GameComponentsLookup.ComponentCellAutoTexture); }
        set {
            if (value != isComponentCellAutoTexture) {
                var index = GameComponentsLookup.ComponentCellAutoTexture;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : componentCellAutoTextureComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherComponentCellAutoTexture;

    public static Entitas.IMatcher<GameEntity> ComponentCellAutoTexture {
        get {
            if (_matcherComponentCellAutoTexture == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ComponentCellAutoTexture);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherComponentCellAutoTexture = matcher;
            }

            return _matcherComponentCellAutoTexture;
        }
    }
}
