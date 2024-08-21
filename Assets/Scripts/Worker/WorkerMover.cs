using System;
using System.Collections;
using UnityEngine;

public class WorkerMover : MonoBehaviour
{
    [SerializeField] private float _interactionDistance;
    [SerializeField] private float _speed;

    private Coroutine _moveCoroutine;
    private bool _isCoroutineDone = true;

    private Vector3 _heightCalculatedTargetPosition;

    public event Action TargetReached;

    public void CarryOrder(Target target)
    {
        float axisX = target.transform.position.x;
        float axisY = transform.position.y;
        float axisZ = target.transform.position.z;

        _heightCalculatedTargetPosition = new Vector3(axisX, axisY, axisZ);

        Vector3 direction = _heightCalculatedTargetPosition - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);

        InitiateCoroutine(target);
    }

    private void InitiateCoroutine(Target target)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        if (_isCoroutineDone == true)
        {
            _isCoroutineDone = false;

            _moveCoroutine = StartCoroutine(MoveCoroutine(target));
        }
    }

    private IEnumerator MoveCoroutine(Target target)
    {
        bool _isMovingEnable = true;

        while (_isMovingEnable)
        {
            yield return null;

            float step = _speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _heightCalculatedTargetPosition, step);

            Vector3 targetPosition = target.transform.position;

            float targetDistance = transform.position.SqrDistance(targetPosition);

            if (targetDistance < _interactionDistance)
            {
                _isMovingEnable = false;                
            }
        }

        _isCoroutineDone = true;

        TargetReached.Invoke();
    }
}
