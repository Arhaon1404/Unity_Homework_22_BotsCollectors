using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : Target
{
    [SerializeField] private List<Worker> _listWorkers;

    private int _countCollectedResources;

    public event Action<int> CountResourcesIncreased;

    public void IncreasecountCollectedResources()
    {
        _countCollectedResources++;

        CountResourcesIncreased.Invoke(_countCollectedResources);
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
}
