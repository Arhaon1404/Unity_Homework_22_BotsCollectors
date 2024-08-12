using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorkerSpawner : MonoBehaviour
{
    [SerializeField] private Worker _prefab;
    [SerializeField] private int _spawnPrice;

    public event Action<Worker,int> NewWorkerSpawned;

    public void CreateNewWorker(Base motherBase)
    {
        if (motherBase.CountCollectedResources >= _spawnPrice)
        {
            Worker newWorker = Instantiate(_prefab,transform.position,transform.rotation);

            newWorker.SetMotherBase(motherBase);

            NewWorkerSpawned.Invoke(newWorker, _spawnPrice);
        }
    }
}
