using System;
using UnityEngine;


public class Worker : MonoBehaviour
{
    [SerializeField] private Base _motherBase;
    private Vector3 _startPosition;
    private Target _target;
    private Resource _resourceInHands;

    private bool _isFree;

    public bool IsFree => _isFree;
    public Vector3 StartPosition => _startPosition;
    public Target Target => _target;

    public event Action TargetReceived;

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

        _resourceInHands.ProceedToBase();
    }

    public void ResetTarget()
    {
        _target = _motherBase;
    }

    public void ChangeStatus()
    {
        _isFree = _isFree ? false : true; 
    }
}
