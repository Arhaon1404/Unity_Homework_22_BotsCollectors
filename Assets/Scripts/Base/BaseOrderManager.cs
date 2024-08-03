using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOrderManager : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private BaseResourceDatabase _baseResourceDatabase;

    private void OnEnable()
    {
        _baseResourceDatabase.ResourceAppeared += SendWorker;
    }

    private void OnDisable()
    {
        _baseResourceDatabase.ResourceAppeared -= SendWorker;
    }

    private void SendWorker()
    {
        Worker currentWorker = _base.ProvideFreeWorker();

        if (currentWorker != null)
        {
            currentWorker.FreeWorkerAppeared += SendFreeWorker;

            Resource currentFreeResource = _baseResourceDatabase.ProvideFreeResource();

            currentWorker.SetResourceTarget(currentFreeResource);
            currentWorker.ChangeIsFreeStatus();
            _baseResourceDatabase.AddNewProcessCollectionResource(currentFreeResource);
        }
    }

    private void SendFreeWorker(Worker freeWorker)
    {
        freeWorker.FreeWorkerAppeared -= SendFreeWorker;

        SendWorker();
    }
}
