using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMesh : MonoBehaviour
{
    private NavMeshSurface surface;
    private void OnEnable()
    {
        surface = GetComponent<NavMeshSurface>();
        Invoke("BakeNavMesh", 1);
    }

    private void BakeNavMesh()
    {
        if (surface != null)
        {
            print("Использует");
            surface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface не найден на объекте " + gameObject.name);
        }
    }
}
