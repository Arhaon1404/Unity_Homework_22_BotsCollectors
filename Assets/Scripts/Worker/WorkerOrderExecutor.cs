using System;
using UnityEngine;

public class WorkerOrderExecutor : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private WorkerMover _workerMover;
    [SerializeField] private WorkerNewBaseBuilder _workerNewBaseBuilder;
    [SerializeField] private WorkerPickUpper _workerPickUpper;

    public event Action OrderComplited;
    public event Action WorkerReturned;

    private void OnEnable()
    {
        _workerMover.TargetReached += PerformTask;
        _workerPickUpper.OrderComplited += ReturnToBase;
    }

    private void OnDisable()
    {
        _workerMover.TargetReached -= PerformTask;
        _workerPickUpper.OrderComplited -= ReturnToBase;
    }

    private void PerformTask()
    {
        if (_worker.Target.TryGetComponent(out Resource resource))
        {
            _workerPickUpper.InitiateCoroutine();
        }
        
        if (_worker.Target.TryGetComponent(out Base motherBase))
        {
            WorkerReturned.Invoke();
        }
        else if(_worker.Target.TryGetComponent(out BuildFlag buildFlag))
        {
            _workerNewBaseBuilder.InitiateCoroutine();
        }
    }

    private void ReturnToBase()
    {
        OrderComplited.Invoke();
    }
}
