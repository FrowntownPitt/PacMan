using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour {

    public GameObject wallCenter;
    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;
    public GameObject floor;

    public GameObject pacDot;
    private GameObject pacDotInstance;

    public GameObject powerPellet;
    private GameObject powerPelletInstance;

    public enum WallTypes
    {
        Center,
        North,
        South,
        East,
        West
    }

    public bool isWall = false;

    public TileType tileType;

    public enum TileType
    {
        Wall,
        PacDot,
        PowerPellet,
        Portal,
    }

	// Use this for initialization
	void Start () {
		
	}

    public GameObject GetFloor()
    {
        return floor;
    }

    public void SetIsWall(bool f)
    {
        tileType = TileType.Wall;
        isWall = f;
    }

    public void InstantiatePacDot()
    {
        pacDotInstance = Instantiate(pacDot, transform);
        pacDotInstance.transform.localPosition = new Vector3(0, wallCenter.transform.lossyScale.y / 2, 0);
        pacDotInstance.SetActive(true);
    }

    public void TogglePacDot(bool b)
    {
        pacDot.SetActive(b);
    }

    public void InstantiatePowerPellet()
    {
        powerPelletInstance = Instantiate(powerPellet, transform);
        powerPelletInstance.transform.localPosition = new Vector3(0, wallCenter.transform.lossyScale.y / 2, 0);
        powerPelletInstance.SetActive(true);
    }

    public void TogglePowerPellet(bool b)
    {
        powerPellet.SetActive(b);
    }

    public void SetTileType(TileType type)
    {
        tileType = type;
        switch (type)
        {
            case TileType.Wall:
                SetIsWall(true);
                break;
            case TileType.PacDot:
                InstantiatePacDot();
                break;
            case TileType.PowerPellet:
                InstantiatePowerPellet();
                break;
            case TileType.Portal:
                break;
            default:
                break;
        }
    }

    public bool IsWall()
    {
        return isWall;
    }

    public void ToggleWall(WallTypes wall, bool toggle)
    {
        switch (wall)
        {
            case WallTypes.Center:
                wallCenter.SetActive(toggle);
                break;
            case WallTypes.North:
                wallNorth.SetActive(toggle);
                break;
            case WallTypes.South:
                wallSouth.SetActive(toggle);
                break;
            case WallTypes.East:
                wallEast.SetActive(toggle);
                break;
            case WallTypes.West:
                wallWest.SetActive(toggle);
                break;
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
