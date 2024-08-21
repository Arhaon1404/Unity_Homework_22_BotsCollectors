using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private int _delay;
    [SerializeField] private int _spawnAmount;

    private Coroutine _spawnCoroutine;
    private WaitForSeconds _spawnDelay;
    private bool _isCoroutineDone = true;

    private void Start()
    {
        _spawnDelay = new WaitForSeconds(_delay);

        InitiateCoroutine();
    }

    protected override Resource CreateObject()
    {
        Vector3 randomPosition = CreateRandomPosition();

        Resource resource = Instantiate(Prefab, SpawnPosition + randomPosition, transform.rotation);

        resource.ResourceCollected += OnReturnedToPool;

        return resource;
    }

    protected override void OnTakeFromPool(Resource resource)
    {
        Vector3 randomPosition = CreateRandomPosition();

        resource.transform.position = SpawnPosition + randomPosition;

        resource.ResourceCollected += Release;

        base.OnTakeFromPool(resource);
    }

    protected override void OnReturnedToPool(Resource resource)
    {
        base.OnReturnedToPool(resource);
    }

    private void Release(Resource resource)
    {
        Pool.Release(resource);
        resource.ResourceCollected -= Release;
    }

    private void InitiateCoroutine()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }

        if (_isCoroutineDone == true)
        {
            _isCoroutineDone = false;
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        for (int i = 0; i < _spawnAmount;i++)
        {
            yield return _spawnDelay;

            Pool.Get();
        }

        _isCoroutineDone = true;
    }

    private Vector3 CreateRandomPosition()
    {
        float bisection = 2;

        float localWidth = transform.localScale.x / bisection;
        float localHeight = transform.localScale.y / bisection;
        float localLenght = transform.localScale.z / bisection;

        float axesX = Random.Range(-localWidth, localWidth);
        float axesY = Random.Range(-localHeight, localHeight);
        float axesZ = Random.Range(-localLenght, localLenght);

        Vector3 newPosition = new Vector3(axesX, axesY, axesZ);

        return newPosition;
    }
}
