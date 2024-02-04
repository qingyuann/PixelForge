namespace PixelForge
{
    interface IComponentStorage
    {
        void Remove(int entityId);
    }
    /// <summary>
    /// A storage for components of type T
    /// </summary>
    class ComponentStorage<T> : IComponentStorage
    {
        private Dictionary<int, T> _components = new Dictionary<int, T>();

        public void AddComponent(int entityId, T component)
        {
            _components[entityId] = component;
        }
        
        /// <summary>
        /// If entityID has component of type T, return true and set component
        /// </summary>
        public bool TryGetComponent(int entityId, out T component)
        {
            return _components.TryGetValue(entityId, out component);
        }

        public void RemoveComponent(int entityId)
        {
            _components.Remove(entityId);
        }
        
        public void RemoveIfContains(int entityId)
        {
            if (_components.ContainsKey(entityId))
            {
                _components.Remove(entityId);
            }
        }
        
        /// <summary>
        /// return all components of same type T
        /// </summary>
        public IEnumerable<T> GetAllComponents()
        {
            return _components.Values;
        }
        
        public IEnumerable<int> GetAllEntityIds()
        {
            return _components.Keys;
        }
        
        void IComponentStorage.Remove(int entityId)
        {
            RemoveComponent(entityId);
        }
    }
}

