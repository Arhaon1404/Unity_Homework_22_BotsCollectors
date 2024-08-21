using System;
using System.Collections;
using UnityEngine;

public class BaseWorkerManager : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private BaseResourceDatabase _baseResourceDatabase;
    [SerializeField] private float _waitingDelay;

    private bool _isNewBaseBuilding;
    private bool _isWorkerSend;

    public event Action<Worker> BuilderDispatched;
    public event Action<Worker> RequestBuildFlagSent;
    public event Action RequestFreeWorkerSent;

    public bool IsNewBaseBuilding => _isNewBaseBuilding;

    private void OnEnable()
    {
        _base.NewWorkerAppeared += SendRequest;
        _baseResourceDatabase.ResourceAppeared += SendRequest;
    }

    private void OnDisable()
    {
        _base.NewWorkerAppeared -= SendRequest;
        _baseResourceDatabase.ResourceAppeared -= SendRequest;
    }

    private void Start()
    {
        _isNewBaseBuilding = false;
    }

    public void ChangeNewBaseBuildingStatus(bool currentBool)
    {
        _isNewBaseBuilding = currentBool;

        if (_isNewBaseBuilding == false)
        {
            _isWorkerSend = false;

            SendRequest();
        }
    }

    public void SelectAction(Worker worker)
    {
        if (worker != null)
        {
            if (_isNewBaseBuilding == false)
            {
                SendWorkerForResource(worker);
            }
            else
            {
                RequestBuildFlagSent.Invoke(worker);
            }
        }
    }

    public void SendWorkerForBuildNewBase(Worker currentWorker, BuildFlag target)
    {
        if (_isWorkerSend == false)
        {
            currentWorker.SetNewBaseTarget(target);

            currentWorker.BecameUnitBusy();

            BuilderDispatched.Invoke(currentWorker);

            _isWorkerSend = true;
        }
    }

    private void SendRequest()
    {
        RequestFreeWorkerSent.Invoke();
    }

    private void SendWorkerForResource(Worker currentWorker)
    {
        Resource currentFreeResource = _baseResourceDatabase.ProvideFreeResource();

        if (currentFreeResource != null)
        {
            currentWorker.FreeWorkerAppeared += SendFreeWorker;

            _baseResourceDatabase.AddNewProcessCollectionResource(currentFreeResource);

            currentWorker.SetResourceTarget(currentFreeResource);

            currentWorker.BecameUnitBusy();

            SendRequest();
        }
    }

    private void SendFreeWorker(Worker freeWorker)
    {
        freeWorker.FreeWorkerAppeared -= SendFreeWorker;

        SelectAction(freeWorker);
    }
}
