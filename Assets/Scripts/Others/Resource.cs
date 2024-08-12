using System;

public class Resource : Target
{
    private bool _colladerIsTouch;

    public event Action<Resource> ResourceCollected;

    public bool ColladerIsTouch => _colladerIsTouch;

    private void Start()
    {
        _colladerIsTouch = false;
    }

    public void Collect()
    {
        ResourceCollected.Invoke(this);

        Destroy(gameObject);
    }

    public void TouchCollader()
    {
        _colladerIsTouch = true;
    }
}
