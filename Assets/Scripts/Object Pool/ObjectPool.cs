using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pooling.Object_Pool
{
    public class ObjectPool<TObject> where TObject : MonoBehaviour, IPoolableObject
    {
        public int Count => _pool.Count;

        private TObject _poolableObjectPrefab;

        private readonly List<TObject> _pool;

        private Transform _container;


        public ObjectPool(TObject prefab, string poolName = "")
        {
            _pool = new List<TObject>();

            _container = new GameObject().transform;
            
            _poolableObjectPrefab  = prefab;
            
            _container.name = poolName != "" ? poolName : "Object Pool";
        }

        /// <summary>
        /// Get's an item from the pool.
        /// If the pool is empty, it will instantiate a new item.
        /// If the pool is not empty, it will return the first available item.
        /// If the pool is not empty, it will return the first available item that matches the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="callOnPoolActivate"></param>
        /// <returns></returns>
        public TObject GetItem(Func<TObject, bool> predicate = null, bool callOnPoolActivate = true)
        {
            var item = predicate == null
                ? _pool.FirstOrDefault(i => i.IsAvailable)
                : _pool.FirstOrDefault(i => i.IsAvailable && predicate(i));
            
            if (item == null)
            {
                item = Object.Instantiate(_poolableObjectPrefab, _container, true);
                _pool.Add(item);
            }


            if (callOnPoolActivate)
            {
                item.OnActivate();
            }

            return item;
        }

        /// <summary>
        /// Returns an item even **if it's not available**.
        /// This function will NOT generate any items. It will only GET based on a predicate
        /// This method isn't safe and can mess with the Object Pool, only use if you know what to do.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="predicate">This is required</param>
        /// <returns></returns>
        public bool ForceGetItem( out TObject item,Func<TObject, bool> predicate)
        {
            item = null;

            if (predicate == null) return false;
            
            item = _pool.FirstOrDefault(predicate);
                
            if(item)
                return true;
            return false;
        }
        
        /// <summary>
        /// Deactivates the item and makes it available again.
        /// </summary>
        /// <param name="itemToRemove"></param>
        public void Remove(TObject itemToRemove)
        {
            TObject item = _pool.FirstOrDefault(i => i == itemToRemove);

            //Block flow if the item does not exist in the pool.
            if (item is null) return;

            item.IsAvailable = true;
            item.OnDeactivate();
        }

        /// <summary>
        /// Disables every single item in this pool
        /// </summary>
        public void DisableAll()
        {
            foreach (var item in _pool)
            {
                item.OnDeactivate();
                item.IsAvailable = true;
            }
        }

        /// <summary>
        /// Clear the objects in the pool.
        /// </summary>
        public void Clear()
        {
            foreach (var obj in _pool)
                Object.Destroy(obj);

            _pool.Clear();
        }
    }
}