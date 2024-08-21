using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Resource : Target
{
    private Rigidbody _rigidbody;

    public event Action<Resource> ResourceCollected;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Collect()
    {
        ResourceCollected.Invoke(this);

        DefrostPosition();
    }

    public void FreezePosition()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void DefrostPosition()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
    }
}
