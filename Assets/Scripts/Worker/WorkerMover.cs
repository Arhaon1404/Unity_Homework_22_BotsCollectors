using System;
using UnityEngine;

public class WorkerMover : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private WorkerOrderExecutor _workerOrderExecutor;
    [SerializeField] private float _interactionDistance;
    [SerializeField] private float _speed;
    
    private bool _isMovingEnable;

    private Vector3 _heightÑalculatedTargetPosition;

    public event Action TargetReached;

    private void OnEnable()
    {
        _worker.TargetReceived += ÑarryOrder;
        _workerOrderExecutor.SelectionComplited += ÑarryOrder;
    }

    private void OnDisable()
    {
        _worker.TargetReceived += ÑarryOrder;
        _workerOrderExecutor.SelectionComplited -= ÑarryOrder;
    }
    private void Start()
    {
        _isMovingEnable = false;
    }

    private void Update()
    {
        if (_isMovingEnable)
        {
            float step = _speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _heightÑalculatedTargetPosition, step);

            float targetDistance = Vector3.Distance(_heightÑalculatedTargetPosition, transform.position);

            if (targetDistance < _interactionDistance)
            {
                _isMovingEnable = false;

                TargetReached.Invoke();
            }
        }
    }

    private void ÑarryOrder()
    {
        float axisX = _worker.Target.transform.position.x;
        float axisY = transform.position.y;
        float axisZ = _worker.Target.transform.position.z;

        _heightÑalculatedTargetPosition = new Vector3(axisX, axisY, axisZ);

        SetDirection();

        _isMovingEnable = true;
    }

    private void SetDirection()
    {
        Vector3 direction = _heightÑalculatedTargetPosition - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
