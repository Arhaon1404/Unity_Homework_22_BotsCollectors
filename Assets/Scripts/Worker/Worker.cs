using System;
using UnityEngine;


public class Worker : MonoBehaviour
{
    [SerializeField] private Base _motherBase;
    [SerializeField] private WorkerOrderExecutor _workerOrderExecutor;
    [SerializeField] private WorkerMover _workerMover;
    [SerializeField] private WorkerNewBaseBuilder _workerNewBaseBuilder;

    [SerializeField] private Target _target;
    private Resource _resourceInHands;

    [SerializeField] private bool _isFree;

    public event Action<Worker> FreeWorkerAppeared;

    public bool IsFree => _isFree;
    public Target Target => _target;

    private void Awake()
    {
        _isFree = true;
    }

    private void OnEnable()
    {
        _workerOrderExecutor.OrderComplited += SetTargetMotherBase;
        _workerOrderExecutor.WorkerReturned += GiveResourceToBase;
        _workerNewBaseBuilder.BuildingFinished += BecameUnitFree;
    }

    private void OnDisable()
    {
        _workerOrderExecutor.OrderComplited -= SetTargetMotherBase;
        _workerOrderExecutor.WorkerReturned -= GiveResourceToBase;
        _workerNewBaseBuilder.BuildingFinished -= BecameUnitFree;
    }

    public void SetResourceTarget(Resource resource)
    {
        _target = resource;

        _resourceInHands = resource;

        _workerMover.CarryOrder(_target);
    }

    public void SetNewBaseTarget(BuildFlag buildFlag)
    {
        _target = buildFlag;

        _workerMover.CarryOrder(_target);
    }

    public void SetMotherBase(Base newBase)
    {
        _motherBase = newBase;
    }

    public void BecameUnitBusy()
    {
        _isFree = false;
    }

    public void BecameUnitFree()
    {
        _isFree = true;
    }

    private void GiveResourceToBase()
    {
        _motherBase.IncreaseCountCollectedResources();

        _resourceInHands.Collect();

        _resourceInHands.transform.SetParent(null);

        BecameUnitFree();

        FreeWorkerAppeared.Invoke(this);
    }

    private void SetTargetMotherBase()
    {
        _target = _motherBase;

        _workerMover.CarryOrder(_target);
    }
}
