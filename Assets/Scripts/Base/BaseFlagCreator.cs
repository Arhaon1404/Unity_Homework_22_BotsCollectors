using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlagCreator: MonoBehaviour
{
    [SerializeField] private ParticleSystem _baseHighlighEffect;
    [SerializeField] private BuildFlag _prefab;

    private BuildFlag _placedBuildFlag;

    public event Action BuildingFlagCreated;
    public event Action BuildingFlagRemoved;

    public BuildFlag PlacedBuildFlag => _placedBuildFlag;

    private void OnMouseDown()
    {
        _baseHighlighEffect.Play();
    }

    private void OnMouseUp()
    {
        _baseHighlighEffect.Stop();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.TryGetComponent(out Floor floor))
            {
                SetFlagPosition(hit);
            }
        }
    }

    private void SetFlagPosition(RaycastHit hit)
    {
        if (_placedBuildFlag == null)
        {
            _placedBuildFlag = Instantiate(_prefab, hit.point, Quaternion.identity);

            _placedBuildFlag.BuildingCompleted += ÑlearValues;

            BuildingFlagCreated.Invoke();
        }
        else
        {
            _placedBuildFlag.transform.position = hit.point;
        }
    }

    private void ÑlearValues()
    {
        _placedBuildFlag.BuildingCompleted -= ÑlearValues;

        BuildingFlagRemoved.Invoke();

        _placedBuildFlag = null;
    }
}
