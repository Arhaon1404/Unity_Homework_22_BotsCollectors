using System;

public class Resource : Target
{
    private bool _inProcessCollection;

    public bool InProcessCollection => _inProcessCollection;

    public event Action<Resource> ResourceCollected;

    private void Start()
    {
        _inProcessCollection = false;
    }

    public void SwitchStatus()
    {
        _inProcessCollection = true;
    }

    public void ProceedToBase()
    {
        ResourceCollected.Invoke(this);

        Destroy(gameObject);
    }
}
