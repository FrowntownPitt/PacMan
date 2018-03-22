using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private List<Portal> portals;
    
    // Gain access to all other portals
    public void AddPortals(List<Portal> p)
    {
        portals = p;
    }

    // Pick a random portal to go to
    public Portal GetRandomPortal()
    {
        return portals[Random.Range(0, portals.Count)];
    }
}
