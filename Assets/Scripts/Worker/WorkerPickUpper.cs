using System;
using System.Collections;
using UnityEngine;

public class WorkerPickUpper : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private WorkerOrderExecutor _workerOrderExecutior;
    [SerializeField] private WorkerHands _workerHands;
    [SerializeField] private int _delayResourceSelection;

    private Coroutine _pickUpResourceCoroutine;
    private WaitForSeconds _delayCoroutine;
    private bool _isCoroutineDone;

    public event Action OrderComplited;

    private void Start()
    {
        _delayCoroutine = new WaitForSeconds(_delayResourceSelection);
        _isCoroutineDone = true;
    }

    public void InitiateCoroutine()
    {
        if (_pickUpResourceCoroutine != null)
        {
            StopCoroutine(_pickUpResourceCoroutine);
        }

        if (_isCoroutineDone == true)
        {
            _isCoroutineDone = false;
            _pickUpResourceCoroutine = StartCoroutine(PickUpResourceCoroutine());
        }
    }

    private IEnumerator PickUpResourceCoroutine()
    {
        yield return _delayCoroutine;

        Target target = _worker.Target;

        target.transform.SetParent(_workerHands.transform);

        if (target.TryGetComponent(out Resource resource))
        {
            resource.FreezePosition();
        }

        target.transform.position = _workerHands.transform.position;

        OrderComplited.Invoke();

        _isCoroutineDone = true;
    }
}
