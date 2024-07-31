using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOrderManager : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private BaseResourceDatabase _baseResourceDatabase;
    [SerializeField] private int _delay;

    private Coroutine _sendCoroutine;
    private WaitForSeconds _sendDelay;
    private bool _isCoroutineDone = true;

    private void OnEnable()
    {
        _baseResourceDatabase.ResourceAppeared += InitiateCoroutine;
    }

    private void OnDisable()
    {
        _baseResourceDatabase.ResourceAppeared -= InitiateCoroutine;
    }

    private void Start()
    {
        _sendDelay = new WaitForSeconds(_delay);
    }

    private void InitiateCoroutine()
    {
        if (_sendCoroutine != null && _isCoroutineDone == true)
        {
            StopCoroutine(_sendCoroutine);
        }

        if (_isCoroutineDone == true)
        {
            _isCoroutineDone = false;
            _sendCoroutine = StartCoroutine(SendWorkerCoroutine());
        }
    }

    private IEnumerator SendWorkerCoroutine()
    {
        while (_isCoroutineDone == false)
        {
            List<Worker> currentListWorkers = _base.ProvideListWorkers();
            List<Resource> currentListFreeResources = _baseResourceDatabase.ProvideListFreeResource();

            yield return _sendDelay;

            if (_baseResourceDatabase.ListCount == 0)
            {
                _isCoroutineDone = true;
            }
            else
            {
                foreach (Worker worker in currentListWorkers)
                {
                    foreach (Resource resource in currentListFreeResources)
                    {
                        if (worker.IsFree == true && resource.InProcessCollection == false)
                        {
                            worker.SetResourceTarget(resource);
                            worker.SetStatus();
                            resource.SetStatus();
                        }
                    }
                }
            }
        }
    }
}
