using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerOrderExecutor : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private WorkerMover _workerMover;
    [SerializeField] private WorkerPickUpper _workerPickUpper;

    public event Action SelectionStarted;
    public event Action SelectionComplited;

    private void OnEnable()
    {
        _workerMover.TargetAchieved += PerformTask;
        _workerPickUpper.OrderComplited += ReturnToBase;
    }

    private void OnDisable()
    {
        _workerMover.TargetAchieved -= PerformTask;
        _workerPickUpper.OrderComplited -= ReturnToBase;
    }

    private void PerformTask()
    {
        if (_worker.Target.TryGetComponent(out Resource resource))
        {
            SelectionStarted.Invoke();
        }

        if (_worker.Target.TryGetComponent(out Base motherBase))
        {
            _worker.GiveResourceToBase();

            _worker.ChangeStatus();
        }
    }

    private void ReturnToBase()
    {
        _worker.ResetTarget();

        SelectionComplited.Invoke();
    }
}
