using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : Target
{
    [SerializeField] private WorkerSpawner _workerSpawner;
    [SerializeField] private BaseWorkerManager _baseWorkerManager;
    [SerializeField] private BaseFlagCreator _baseFlagCreator;
    [SerializeField] private List<Worker> _listWorkers;
    [SerializeField] private int _maxWorkers;

    private int _countCollectedResources;
    private int _newBasePrice;

    private bool _isSetFlag;

    public event Action<int> CountResourcesChanged;
    public event Action NewWorkerAppeared;

    public int CountCollectedResources => _countCollectedResources;

    private void OnEnable()
    {
        _baseFlagCreator.BuildingFlagCreated += BecameFlagTrue;
        _baseFlagCreator.BuildingFlagRemoved += BecameFlagFalse;
        _workerSpawner.NewWorkerSpawned += AddNewWorker;
        _baseWorkerManager.RequestFreeWorkerSent += ProvideFreeWorker;
        _baseWorkerManager.RequestBuildFlagSent += ProvideNewBaseFlag;
        _baseWorkerManager.BuilderDispatched += DeleteWorker;
    }

    private void OnDisable()
    {
        _baseFlagCreator.BuildingFlagCreated -= BecameFlagTrue;
        _baseFlagCreator.BuildingFlagRemoved -= BecameFlagFalse;
        _workerSpawner.NewWorkerSpawned -= AddNewWorker;
        _baseWorkerManager.RequestFreeWorkerSent -= ProvideFreeWorker;
        _baseWorkerManager.RequestBuildFlagSent -= ProvideNewBaseFlag;
        _baseWorkerManager.BuilderDispatched -= DeleteWorker;
    }

    private void Start()
    {
        _isSetFlag = false;
        _newBasePrice = 5;
    }

    public void IncreaseCountCollectedResources()
    {
        _countCollectedResources++;

        CountResourcesChanged.Invoke(_countCollectedResources);

        SelectAction();
    }

    public void AddWorker(Worker worker)
    {
        worker.SetMotherBase(this);

        _listWorkers.Add(worker);
    }

    private void ProvideNewBaseFlag(Worker worker)
    {
        BuildFlag flag = _baseFlagCreator.PlacedBuildFlag;

        _baseWorkerManager.SendWorkerForBuildNewBase(worker, flag);
    }

    private void ProvideFreeWorker()
    {
        Worker freeWorker = null;

        foreach (Worker worker in _listWorkers)
        {
            if (worker.IsFree == true)
            {
                freeWorker = worker;

                break;
            }
        }

        _baseWorkerManager.SelectAction(freeWorker);
    }

    private void BecameFlagTrue()
    {
        _isSetFlag = true;
    }

    private void BecameFlagFalse()
    {
        _isSetFlag = false;

        _baseWorkerManager.ChangeNewBaseBuildingStatus(_isSetFlag);
    }

    private void TryBuyNewWorker()
    {
        int random = UnityEngine.Random.Range(0, 100);
        int chanceBuyNewWorker = 0;

        if (random > chanceBuyNewWorker)
        {
            _workerSpawner.CreateNewWorker(this);
        }
    }

    private void AddNewWorker(Worker newWorker, int spawnPrice)
    {
        _countCollectedResources -= spawnPrice;

        CountResourcesChanged.Invoke(_countCollectedResources);

        AddWorker(newWorker);

        NewWorkerAppeared.Invoke();
    }

    private void DeleteWorker(Worker worker)
    {
        _listWorkers.Remove(worker);
    }

    private void SelectAction()
    {
        if (_isSetFlag == false)
        {
            if (_listWorkers.Count <= _maxWorkers)
            {
                TryBuyNewWorker();
            }
        }
        else if (_baseWorkerManager.IsNewBaseBuilding == false)
        {
            if (_countCollectedResources >= _newBasePrice)
            {
                _countCollectedResources -= _newBasePrice;

                CountResourcesChanged.Invoke(_countCollectedResources);

                bool isBuildingBegin = true;

                _baseWorkerManager.ChangeNewBaseBuildingStatus(isBuildingBegin);
            }
        }
    }
}
