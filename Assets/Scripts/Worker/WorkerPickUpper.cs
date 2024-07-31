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

    private void OnEnable()
    {
        _workerOrderExecutior.SelectionStarted += InitiateCoroutine;
    }

    private void OnDisable()
    {
        _workerOrderExecutior.SelectionStarted -= InitiateCoroutine;
    }

    private void Start()
    {
        _delayCoroutine = new WaitForSeconds(_delayResourceSelection);
        _isCoroutineDone = true;
    }

    private void InitiateCoroutine()
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

        Target target = _worker.ProvideTarget();

        target.transform.SetParent(_workerHands.transform);

        if (target.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        }

        target.transform.position = _workerHands.transform.position;

        OrderComplited.Invoke();

        _isCoroutineDone = true;
    }
}
