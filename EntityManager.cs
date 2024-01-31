namespace PixelForge
{
    class EntityManager
    {
        private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        private Dictionary<Type, Dictionary<int, object>> components = new Dictionary<Type, Dictionary<int, object>>();
        
        public Entity CreateEntity(int id)
        {
            var entity = new Entity(id);
            entities[id] = entity;
            return entity;
        }
        
        public void AddComponent<T>(Entity entity, T component)
        {
            var type = typeof(T);
            if (!components.ContainsKey(type))
            {
                components[type] = new Dictionary<int, object>();
            }
            components[type][entity.Id] = component;
        }
        
        /// <summary>
        /// get component T from entity if it exists
        /// </summary>
        public T GetComponent<T>(Entity entity) where T : class
        {
            var type = typeof(T);
            if (components.ContainsKey(type) && components[type].ContainsKey(entity.Id))
            {
                return (T)components[type][entity.Id];
            } 
            return null;
        }
        
        /// <summary>
        /// remove component T from entity if it exists
        /// </summary>
        public void RemoveComponent<T>(Entity entity) where T : class
        {
            var type = typeof(T);
            if (components.ContainsKey(type) && components[type].ContainsKey(entity.Id))
            {
                components[type].Remove(entity.Id);
            }
        }
        
        /// <summary>
        /// return all entities with certain component T
        /// </summary>
        public IEnumerable<Entity> GetEntitiesWithComponent<T>()
        {
            var type = typeof(T);
            if (components.ContainsKey(type))
            {
                foreach (var entity in components[type].Keys)
                {
                    yield return entities[entity];
                }
            }
        }
        
        
    }
}

