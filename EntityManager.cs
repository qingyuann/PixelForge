namespace PixelForge
{
    class EntityManager
    {
        //private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        //private Dictionary<Type, Dictionary<int, object>> components = new Dictionary<Type, Dictionary<int, object>>();
        
        private int _nextEntityId = 0;
        private Dictionary<Type, Object> _componentStores = new Dictionary<Type, Object>();
        
        
        public void CreateEntity()
        {
            _nextEntityId++;
        }
        
        /// <summary>
        /// Add component T to entity
        /// </summary>
        public void AddComponent<T>(int entityId, T component)
        {
            var type = typeof(T);
            // if no component of type T exists, create new storage
            if (!_componentStores.TryGetValue(type, out var storage))
            {
                storage = new ComponentStorage<T>();
                _componentStores[type] = storage;
            }
            
            var typedStorage = (ComponentStorage<T>)storage;
            typedStorage.AddComponent(entityId, component);
        }
        
        /// <summary>
        /// remove component T from entity if it exists
        /// </summary>
        public void RemoveComponent<T>(int entityId)
        {
            var type = typeof(T);
            if (_componentStores.TryGetValue(type, out var storage))
            {
                var typedStorage = (ComponentStorage<T>)storage;
                typedStorage.RemoveComponent(entityId);
            }
        }

        
        /// <summary>
        /// get component T from entity if it exists
        /// </summary>
        public T GetComponent<T>(int entityId) where T : struct
        {
            var type = typeof(T);
            if (_componentStores.TryGetValue(type, out var storage))
            {
                var typedStorage = (ComponentStorage<T>)storage;
                if (typedStorage.TryGetComponent(entityId, out var component))
                {
                    return component;
                }
            }
            return default;
        }
        
        /// <summary>
        /// return all entities with certain component T
        /// </summary>
        public IEnumerable<int> GetEntitiesWithComponent<T>()
        {
            var type = typeof(T);
            if (_componentStores.TryGetValue(type, out var storage))
            {
                var typedStorage = (ComponentStorage<T>)storage;
                foreach (var id in typedStorage.GetAllEntityIds())
                {
                    yield return id;
                }
            }
        }

        public void DestoryEntity(int entityId)
        {
            foreach (var storage in _componentStores.Values)
            {
                (storage as IComponentStorage).Remove(entityId);
            }
        }
        
    }
}

