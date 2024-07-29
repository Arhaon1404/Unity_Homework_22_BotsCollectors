using UnityEngine;
using TMPro;
using System;

public class UIBaseResourceCounter : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _base.CountResourcesIncreased += ChangeInfo;
    }

    private void OnDisable()
    {
        _base.CountResourcesIncreased -= ChangeInfo;
    }

    private void ChangeInfo(int count)
    {
        _text.text = Convert.ToString(count);
    }
}
