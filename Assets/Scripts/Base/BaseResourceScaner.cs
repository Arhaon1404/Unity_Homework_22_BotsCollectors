using System;
using UnityEngine;

public class BaseResourceScaner : MonoBehaviour
{
    [SerializeField] private BaseResourceDatabase _baseResourceDatabase;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Resource resource))
        {
            _baseResourceDatabase.AddNewFreeResource(resource);
        }
    }
}
