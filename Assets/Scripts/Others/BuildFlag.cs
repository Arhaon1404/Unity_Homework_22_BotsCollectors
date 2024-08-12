using System;
using UnityEngine;

public class BuildFlag : Target
{
    public event Action BuildingCompleted;

    public void RemoveFlag()
    {
        BuildingCompleted.Invoke();

        Destroy(gameObject);
    }
}
