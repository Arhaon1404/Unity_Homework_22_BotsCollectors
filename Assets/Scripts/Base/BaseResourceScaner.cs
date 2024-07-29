using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceScaner : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private BaseOrderManager _baseOrderManager;

    private List<Resource> _listFreeResource;
    public List<Resource> ListFreeResource => _listFreeResource;

    public event Action ResourceAppeared;

    private void Start()
    {
        _listFreeResource = new List<Resource>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Resource resource))
        {
            if (_listFreeResource.Count == 0)
            {
                AddNewResource(resource);
                ResourceAppeared.Invoke();
            }
            else
            {
                AddNewResource(resource);
            }
                
        }
    }

    private void AddNewResource(Resource resource)
    {
        resource.ResourceCollected += RemoveCollectedResource;
        _listFreeResource.Add(resource);
    }

    private void RemoveCollectedResource(Resource resource)
    {
        resource.ResourceCollected -= RemoveCollectedResource;
        _listFreeResource.Remove(resource);
    }
}
