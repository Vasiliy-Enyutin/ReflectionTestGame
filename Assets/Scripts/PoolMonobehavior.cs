using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolMonobehavior<T> where T : MonoBehaviour
{
    private List<T> _pool;
    private readonly T _prefab;
    private readonly Transform _container;
    private readonly bool _autoExpand;
    
    
    public PoolMonobehavior(T prefab, int count, bool autoExpand = true, Transform container = null)
    {
        _prefab = prefab;
        _container = container;
        _autoExpand = autoExpand;
        _container = container;
        
        CreatePool(count);
    }
    

    public T GetFreeElement()
    {
        if (TryGetFreeElement(out T element) == true)
            return element;

        if (_autoExpand == true)
            return CreateObject(true);

        throw new Exception($"No free elements in pull. Type: {typeof(T)}");
    }

    public void DeactivateObjects()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            _pool[i].gameObject.SetActive(false);
        }
    }
    
    private bool TryGetFreeElement(out T element)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (_pool[i].gameObject.activeInHierarchy == false)
            {
                element = _pool[i];
                element.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        T createdObject = Object.Instantiate(_prefab, _container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }
}
