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

    private WaitForSeconds _coroutineDelay;

    public event Action<Worker> BuilderDispatched;

    public bool IsNewBaseBuilding => _isNewBaseBuilding;

    private void OnEnable()
    {
        _base.NewWorkerAppeared += SelectAction;
        _baseResourceDatabase.ResourceAppeared += SelectAction;
    }

    private void OnDisable()
    {
        _base.NewWorkerAppeared -= SelectAction;
        _baseResourceDatabase.ResourceAppeared -= SelectAction;
    }

    private void Start()
    {
        _isNewBaseBuilding = false;
        _coroutineDelay = new WaitForSeconds(_waitingDelay);
    }

    public void ChangeNewBaseBuildingStatus(bool currentBool)
    {
        _isNewBaseBuilding = currentBool;

        if (_isNewBaseBuilding == false)
        {
            _isWorkerSend = false;
        }
    }

    private void SelectAction()
    {
        if (_isNewBaseBuilding == false)
        {
            SendWorkerForResource();
        }
        else
        {
            SendWorkerForBuildNewBase();
        }
    }

    private void SendWorkerForResource()
    {
        Worker currentWorker = _base.ProvideFreeWorker();
        Resource currentFreeResource = _baseResourceDatabase.ProvideFreeResource();

        if (currentWorker != null && currentFreeResource != null)
        {
            currentWorker.FreeWorkerAppeared += SendFreeWorker;

            _baseResourceDatabase.AddNewProcessCollectionResource(currentFreeResource);

            currentWorker.SetResourceTarget(currentFreeResource);
            currentWorker.ChangeIsFreeStatus();

            if (_base.ProvideFreeWorker() == true)
            {
                SendWorkerForResource();
            }
        }
    }

    private void SendWorkerForBuildNewBase()
    {
        if (_isWorkerSend == false)
        {
            StartCoroutine(WaitFreeWorkerCoroutine());

            StopCoroutine(WaitFreeWorkerCoroutine());

            _isWorkerSend = true;
        }
        
    }

    private IEnumerator WaitFreeWorkerCoroutine()
    {
        Worker currentWorker = _base.ProvideFreeWorker();

        while (currentWorker == null)
        {
            yield return _coroutineDelay;

            currentWorker = _base.ProvideFreeWorker();
        }

        currentWorker.FreeWorkerAppeared += SendFreeWorker;

        currentWorker.SetNewBaseTarget(_base.NewBaseFlag);

        currentWorker.ChangeIsFreeStatus();

        BuilderDispatched.Invoke(currentWorker);
    }

    private void SendFreeWorker(Worker freeWorker)
    {
        freeWorker.FreeWorkerAppeared -= SendFreeWorker;

        SelectAction();
    }
}
