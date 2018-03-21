using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private List<Portal> portals;

	// Use this for initialization
	void Start () {
		
	}

    public void AddPortals(List<Portal> p)
    {
        portals = p;
    }

    public Portal GetRandomPortal()
    {
        return portals[Random.Range(0, portals.Count)];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
