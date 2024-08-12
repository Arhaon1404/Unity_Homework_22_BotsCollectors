using System;
using UnityEngine;

public class WorkerMover : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private WorkerOrderExecutor _workerOrderExecutor;
    [SerializeField] private float _interactionDistance;
    [SerializeField] private float _speed;
    
    private bool _isMovingEnable;

    private Vector3 _height�alculatedTargetPosition;

    public event Action TargetReached;

    private void OnEnable()
    {
        _worker.TargetReceived += �arryOrder;
        _workerOrderExecutor.SelectionComplited += �arryOrder;
    }

    private void OnDisable()
    {
        _worker.TargetReceived += �arryOrder;
        _workerOrderExecutor.SelectionComplited -= �arryOrder;
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

            transform.position = Vector3.MoveTowards(transform.position, _height�alculatedTargetPosition, step);

            float targetDistance = Vector3.Distance(_height�alculatedTargetPosition, transform.position);

            if (targetDistance < _interactionDistance)
            {
                _isMovingEnable = false;

                TargetReached.Invoke();
            }
        }
    }

    private void �arryOrder()
    {
        float axisX = _worker.Target.transform.position.x;
        float axisY = transform.position.y;
        float axisZ = _worker.Target.transform.position.z;

        _height�alculatedTargetPosition = new Vector3(axisX, axisY, axisZ);

        SetDirection();

        _isMovingEnable = true;
    }

    private void SetDirection()
    {
        Vector3 direction = _height�alculatedTargetPosition - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
