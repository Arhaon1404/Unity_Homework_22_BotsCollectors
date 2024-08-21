using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceDatabase : MonoBehaviour
{
    [SerializeField] private List<Resource> _listFreeResource;
    private List<Resource> _listProcessCollectionResource;

    public event Action ResourceAppeared;

    public int ListCount => _listFreeResource.Count;

    private void Start()
    {
        _listFreeResource = new List<Resource>();
        _listProcessCollectionResource = new List<Resource>();
    }

    public void AddNewFreeResource(Resource resource)
    {
        if (_listFreeResource.Contains(resource) == false && _listProcessCollectionResource.Contains(resource) == false)
        {
            _listFreeResource.Add(resource);

            ResourceAppeared.Invoke();
        }
    }

    public void AddNewProcessCollectionResource(Resource resource)
    {
        _listProcessCollectionResource.Add(resource);

        _listFreeResource.Remove(resource);

        resource.ResourceCollected += RemoveCollectedResource;
    }

    public Resource ProvideFreeResource()
    {
        int firstItem = 0;
        Resource resource = null;

        if (_listFreeResource.Count >= 1)
        {
            resource = _listFreeResource[firstItem];
        }

        return resource;
    }

    private void RemoveCollectedResource(Resource resource)
    {
        resource.ResourceCollected -= RemoveCollectedResource;
        _listProcessCollectionResource.Remove(resource);
    }
}
