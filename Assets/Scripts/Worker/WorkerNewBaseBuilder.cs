using System;
using System.Collections;
using UnityEngine;

public class WorkerNewBaseBuilder : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private Base _prefab;
    [SerializeField] private WorkerOrderExecutor _workerOrderExecutior;
    [SerializeField] private int _delayBaseBuild;

    private Coroutine _buildNewBaseCoroutine;
    private WaitForSeconds _delayCoroutine;
    private bool _isCoroutineDone;

    private void OnEnable()
    {
        _workerOrderExecutior.BuildingStarted += InitiateCoroutine;
    }

    private void OnDisable()
    {
        _workerOrderExecutior.BuildingStarted += InitiateCoroutine;
    }

    private void Start()
    {
        _delayCoroutine = new WaitForSeconds(_delayBaseBuild);
        _isCoroutineDone = true;
    }

    private void InitiateCoroutine()
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

        Base newBase = Instantiate(_prefab, newBasePosition, _prefab.transform.rotation);

        _worker.SetMotherBase(newBase);

        newBase.AddWorker(_worker);

        _worker.ChangeIsFreeStatus();

        _isCoroutineDone = true;
    }
}
