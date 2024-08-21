using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    [SerializeField] private Worker _prefab;
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private int _spawnPrice;

    public event Action<Worker,int> NewWorkerSpawned;

    public void CreateNewWorker(Base motherBase)
    {
        if (motherBase.CountCollectedResources > _spawnPrice)
        {
            Worker newWorker = Instantiate(_prefab,transform.position,transform.rotation);

            if (newWorker.transform.TryGetComponent(out WorkerNewBaseBuilder workerNewBaseBuilder))
            {
                workerNewBaseBuilder.SetBaseSpawner(_baseSpawner);
            }

            NewWorkerSpawned.Invoke(newWorker, _spawnPrice);
        }
    }

    public void SetBaseSpawner(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
    }
}
