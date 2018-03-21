using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour {

    private NavMeshSurface[] surfaces;
    private int index = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Bake()
    {
        for(int i=0; i<surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

    public void InitSurfaces(int s)
    {
        surfaces = new NavMeshSurface[s];
    }

    public void SetSurfaces(NavMeshSurface[] s)
    {
        surfaces = new NavMeshSurface[s.Length];
        for(int i=0; i<s.Length; i++)
        {
            surfaces[i] = s[i];
        }
    }

    public void AddSurface(GameObject floor)
    {
        NavMeshSurface surface = floor.GetComponent<NavMeshSurface>();
        if (surface != null)
        {
            surfaces[index++] = surface;
        }
    }
}
