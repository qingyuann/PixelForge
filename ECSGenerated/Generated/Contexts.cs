//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts : Entitas.IContexts {

    public static Contexts sharedInstance {
        get {
            if (_sharedInstance == null) {
                _sharedInstance = new Contexts();
            }

            return _sharedInstance;
        }
        set { _sharedInstance = value; }
    }

    static Contexts _sharedInstance;

    public GameContext game { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { game }; } }

    public Contexts() {
        game = new GameContext();

        var postConstructors = System.Linq.Enumerable.Where(
            GetType().GetMethods(),
            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))
        );

        foreach (var postConstructor in postConstructors) {
            postConstructor.Invoke(this, null);
        }
    }

    public void Reset() {
        var contexts = allContexts;
        for (int i = 0; i < contexts.Length; i++) {
            contexts[i].Reset();
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EntityIndexGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

    public const string ComponentName = "ComponentName";
    public const string ComponentParent = "ComponentParent";

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices() {
        game.AddEntityIndex(new Entitas.PrimaryEntityIndex<GameEntity, string>(
            ComponentName,
            game.GetGroup(GameMatcher.ComponentName),
            (e, c) => ((Component.NameComponent)c).Name));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, string>(
            ComponentParent,
            game.GetGroup(GameMatcher.ComponentParent),
            (e, c) => ((Component.ParentComponent)c).ParentName));
    }
}

public static class ContextsExtensions {

    public static GameEntity GetEntityWithComponentName(this GameContext context, string Name) {
        return ((Entitas.PrimaryEntityIndex<GameEntity, string>)context.GetEntityIndex(Contexts.ComponentName)).GetEntity(Name);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithComponentParent(this GameContext context, string ParentName) {
        return ((Entitas.EntityIndex<GameEntity, string>)context.GetEntityIndex(Contexts.ComponentParent)).GetEntities(ParentName);
    }
}