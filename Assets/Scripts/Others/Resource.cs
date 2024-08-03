using System;

public class Resource : Target
{
    public event Action<Resource> ResourceCollected;

    public void Collect()
    {
        ResourceCollected.Invoke(this);

        Destroy(gameObject);
    }
}
