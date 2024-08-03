using System;
using UnityEngine;


public class Worker : MonoBehaviour
{
    [SerializeField] private Base _motherBase;
    private Vector3 _startPosition;
    private Target _target;
    private Resource _resourceInHands;

    private bool _isFree;

    public event Action TargetReceived;
    public event Action<Worker> FreeWorkerAppeared;

    public bool IsFree => _isFree;
    public Vector3 StartPosition => _startPosition;
    public Target Target => _target;

    private void Start()
    {
        _startPosition = transform.position;
        _isFree = true;
    }

    public void SetResourceTarget(Resource resource)
    {
        _target = resource;

        _resourceInHands = resource;

        TargetReceived.Invoke();
    }

    public void GiveResourceToBase()
    {
        _motherBase.IncreasecountCollectedResources();

        _resourceInHands.Collect();
    }

    public void SetTargetMotherBase()
    {
        _target = _motherBase;
    }

    public void ChangeIsFreeStatus()
    {
        _isFree = !_isFree;

        if (_isFree == true)
        {
            FreeWorkerAppeared.Invoke(this);
        }
    }
}
