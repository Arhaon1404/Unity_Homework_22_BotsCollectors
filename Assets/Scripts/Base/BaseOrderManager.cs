using System.Collections;
using UnityEngine;

public class BaseOrderManager : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private BaseResourceScaner _baseResourceScaner;
    [SerializeField] private int _delay;

    private Coroutine _sendCoroutine;
    private WaitForSeconds _sendDelay;
    private bool _isCoroutineDone = true;

    private void OnEnable()
    {
        _baseResourceScaner.ResourceAppeared += InitiateCoroutine;
    }

    private void OnDisable()
    {
        _baseResourceScaner.ResourceAppeared -= InitiateCoroutine;
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
            if (_baseResourceScaner.ListFreeResource.Count == 0)
            {
                _isCoroutineDone = true;
            }
            else
            {
                yield return _sendDelay;

                foreach (Worker worker in _base.ListWorkers)
                {
                    foreach (Resource resource in _baseResourceScaner.ListFreeResource)
                    {
                        if (worker.IsFree == true && resource.InProcessCollection == false)
                        {
                            worker.SetResourceTarget(resource);
                            worker.ChangeStatus();
                            resource.SwitchStatus();
                        }
                    }
                }
            }
        }
    }
}
