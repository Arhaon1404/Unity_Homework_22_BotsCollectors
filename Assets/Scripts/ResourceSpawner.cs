using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private int _spawnAmount;
    [SerializeField] private int _delay;

    private Coroutine _spawnCoroutine;
    private WaitForSeconds _spawnDelay;
    private bool _isCoroutineDone = true;

    [SerializeField] private int _spawnCount;

    private void Start()
    {
        _spawnDelay = new WaitForSeconds(_delay);

        _spawnCount = 0;

        InitiateCoroutine();
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

            Spawn();

            _spawnCount++;
        }

        _isCoroutineDone = true;
    }

    private void Spawn()
    {
        float bisection = 2;

        float localWidth = transform.localScale.x / bisection;
        float localHeight = transform.localScale.y / bisection;
        float localLenght = transform.localScale.z / bisection;

        float axesX = Random.Range(-localWidth, localWidth);
        float axesY = Random.Range(-localHeight, localHeight);
        float axesZ = Random.Range(-localLenght, localLenght);

        Vector3 newPosition = new Vector3(axesX, axesY, axesZ);

        Instantiate(_prefab, transform.position + newPosition,transform.rotation);
    }
}
