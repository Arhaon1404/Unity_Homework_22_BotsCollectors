using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _prefab;

    public Base CreateNewBase(Vector3 newBasePosition)
    {
        Base newBase = Instantiate(_prefab, newBasePosition, _prefab.transform.rotation);

        foreach (Transform child in newBase.transform)
        {
            if (child.TryGetComponent(out WorkerSpawner workerSpawner))
            {
                workerSpawner.SetBaseSpawner(this);
                break;
            }
        }

        return newBase;
    }
}
