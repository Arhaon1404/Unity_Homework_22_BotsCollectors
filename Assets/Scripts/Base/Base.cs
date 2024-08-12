using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : Target
{
    [SerializeField] private BaseWorkerSpawner _baseWorkerSpawner;
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
    public bool IsSetFlag => _isSetFlag;

    public BuildFlag NewBaseFlag => _baseFlagCreator.PlacedBuildFlag;

    private void OnEnable()
    {
        _baseFlagCreator.BuildingFlagCreated += ChangeFlagState;
        _baseFlagCreator.BuildingFlagRemoved += ChangeFlagState;
        _baseWorkerSpawner.NewWorkerSpawned += AddNewWorker;
        _baseWorkerManager.BuilderDispatched += DeleteWorker;
    }

    private void OnDisable()
    {
        _baseFlagCreator.BuildingFlagCreated -= ChangeFlagState;
        _baseFlagCreator.BuildingFlagRemoved -= ChangeFlagState;
        _baseWorkerSpawner.NewWorkerSpawned -= AddNewWorker;
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

    public Worker ProvideFreeWorker()
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

        return freeWorker;
    }

    public void AddWorker(Worker worker)
    {
        _listWorkers.Add(worker);
    }

    private void ChangeFlagState()
    {
        _isSetFlag = !_isSetFlag;

        if (_isSetFlag == false)
        {
            bool isBuildingEnd = false;

            _baseWorkerManager.ChangeNewBaseBuildingStatus(isBuildingEnd);
        }
    }

    private void TryBuyNewWorker()
    {
        int random = UnityEngine.Random.Range(0, 100);
        int chanceBuyNewWorker = 50;

        if (random > chanceBuyNewWorker)
        {
            _baseWorkerSpawner.CreateNewWorker(this);
        }
    }

    private void AddNewWorker(Worker newWorker, int spawnPrice)
    {
        AddWorker(newWorker);

        _countCollectedResources -= spawnPrice;

        CountResourcesChanged.Invoke(_countCollectedResources);
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
