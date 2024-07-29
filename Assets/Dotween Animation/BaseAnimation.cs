using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimation : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private int _loops;
    [SerializeField] private LoopType _loopType;
    [SerializeField] private Vector3 _targetRotate;
    [SerializeField] private float _targetMove;

    private void Start()
    {
        transform.DOMoveY(_targetMove, _duration).SetLoops(_loops, _loopType);
        transform.DORotate(_targetRotate, _duration,RotateMode.FastBeyond360).SetLoops(_loops, _loopType);
    }
}
