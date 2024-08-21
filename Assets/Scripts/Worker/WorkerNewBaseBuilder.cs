using System;
using System.Collections;
using UnityEngine;

public class WorkerNewBaseBuilder : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private WorkerOrderExecutor _workerOrderExecutior;
    [SerializeField] private int _delayBaseBuild;

    private Coroutine _buildNewBaseCoroutine;
    private WaitForSeconds _delayCoroutine;
    private bool _isCoroutineDone;

    public event Action BuildingFinished;

    private void Start()
    {
        _delayCoroutine = new WaitForSeconds(_delayBaseBuild);
        _isCoroutineDone = true;
    }

    public void SetBaseSpawner(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
    }

    public void InitiateCoroutine()
    {
        if (_buildNewBaseCoroutine != null)
        {
            StopCoroutine(_buildNewBaseCoroutine);
        }

        if (_isCoroutineDone == true)
        {
            _isCoroutineDone = false;
            _buildNewBaseCoroutine = StartCoroutine(BuildBaseCoroutine());
        }
    }

    private IEnumerator BuildBaseCoroutine()
    {
        Vector3 newBasePosition = _worker.Target.transform.position;

        yield return _delayCoroutine;

        if (_worker.Target.transform.TryGetComponent(out BuildFlag flag))
        {
            flag.RemoveFlag();
        }

        Base newBase = _baseSpawner.CreateNewBase(newBasePosition);

        newBase.AddWorker(_worker);

        BuildingFinished.Invoke();

        _isCoroutineDone = true;
    }
}
