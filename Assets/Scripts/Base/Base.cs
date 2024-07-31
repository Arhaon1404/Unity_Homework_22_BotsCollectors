using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : Target
{
    [SerializeField] private List<Worker> _listWorkers;

    private int _countCollectedResources;

    public event Action<int> CountResourcesIncreased;

    public List<Worker> ListWorkers => _listWorkers;

    public void IncreasecountCollectedResources()
    {
        _countCollectedResources++;

        CountResourcesIncreased.Invoke(_countCollectedResources);
    }

    public List<Worker> ProvideListWorkers()
    {
        return _listWorkers;
    }
}
