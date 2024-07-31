using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceDatabase : MonoBehaviour
{
    private List<Resource> _listFreeResource;

    public event Action ResourceAppeared;

    public int ListCount => _listFreeResource.Count;

    private void Start()
    {
        _listFreeResource = new List<Resource>();
    }

    private void RemoveCollectedResource(Resource resource)
    {
        resource.ResourceCollected -= RemoveCollectedResource;
        _listFreeResource.Remove(resource);
    }

    public void AddNewResource(Resource resource)
    {
        if (_listFreeResource.Count == 0)
        {
            ResourceAppeared.Invoke();
        }

        resource.ResourceCollected += RemoveCollectedResource;   

        _listFreeResource.Add(resource);
    }

    public List<Resource> ProvideListFreeResource()
    {
        return _listFreeResource;
    }
}
