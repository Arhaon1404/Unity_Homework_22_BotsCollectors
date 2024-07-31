using System;

public class Resource : Target
{
    private bool _inProcessCollection;

    public event Action<Resource> ResourceCollected;

    public bool InProcessCollection => _inProcessCollection;

    private void Start()
    {
        _inProcessCollection = false;
    }

    public void SetStatus()
    {
        _inProcessCollection = true;
    }

    public void Collect()
    {
        ResourceCollected.Invoke(this);

        Destroy(gameObject);
    }
}
