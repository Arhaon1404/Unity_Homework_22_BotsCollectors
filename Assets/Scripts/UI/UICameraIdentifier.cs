using UnityEngine;

public class UICameraIdentifier : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        _canvas.worldCamera = Camera.main;
    }
}
