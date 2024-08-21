using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected int PoolCapacity;
    [SerializeField] protected int MaxPoolCapacity;

    protected Vector3 SpawnPosition;
    protected ObjectPool<T> Pool;

    protected virtual void Awake()
    {
        SpawnPosition = transform.position;

        Pool = new ObjectPool<T>(
        createFunc: () => CreateObject(),
        actionOnGet: (poolObject) => OnTakeFromPool(poolObject),
        actionOnRelease: (poolObject) => OnReturnedToPool(poolObject),
        actionOnDestroy: (poolObject) => Destroy(poolObject),
        collectionCheck: false,
        defaultCapacity: PoolCapacity,
        maxSize: MaxPoolCapacity
        );
    }

    protected virtual void OnTakeFromPool(T poolObject)
    {
        poolObject.gameObject.SetActive(true);
    }

    protected virtual T CreateObject()
    {
        return Instantiate(Prefab, SpawnPosition, transform.rotation);
    }

    protected virtual void OnReturnedToPool(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
    }
}