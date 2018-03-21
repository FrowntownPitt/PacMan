using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace I2M
{
    public class Generator : MonoBehaviour
    {

        public Texture2D mazeImage;

        public Vector2Int imageSize;

        public GameObject playerPrefab;
        private GameObject playerInstance;

        private Vector3 playerSpawn;

        Color[,] pixels;

        public MazeTile mazeTile; // prefab

        MazeTile[,] tiles;

        public float colorTolerance;
        // Floor, Wall, EnemySpawn
        public Color[] imageColors;

        public GameObject floor;

        public NavMeshGenerator navMeshGenerator;

        private List<Vector3> ghostSpawns;

        public GameObject ghostPrefab;

        // Use this for initialization
        public void StartGenerator()
        {
            //floorObjects = new List<GameObject>();

            ghostSpawns = new List<Vector3>();

            imageSize.x = mazeImage.width;
            imageSize.y = mazeImage.height;

            floor.transform.localScale = new Vector3((float)imageSize.x / 10, 1, (float)imageSize.y / 10);
            floor.transform.position = new Vector3((float)(imageSize.x-1) / 2, 0, (float)(imageSize.y-1) / 2);

            pixels = new Color[imageSize.x, imageSize.y];
            for (int x = 0; x < imageSize.x; x++)
            {
                for (int y = 0; y < imageSize.y; y++)
                {
                    pixels[x,y] = mazeImage.GetPixel(x, y);
                }
            }

            //for(int i=0; i<imageColors.Length; i++)
            //    Debug.Log(imageColors[i]);

            StartCoroutine(GenerateMaze());
        }

        bool CompareColors(Color c1, Color c2)
        {
            if (Mathf.Abs(c1.r - c2.r) < colorTolerance &&
                Mathf.Abs(c1.g - c2.g) < colorTolerance &&
                Mathf.Abs(c1.b - c2.b) < colorTolerance)
            {
                return true;
            }
            else
                return false;
        }

        void SpawnPlayer(float x, float z)
        {
            if(playerInstance == null)
            {
                playerInstance = Instantiate(playerPrefab);
                playerInstance.transform.position = new Vector3(x, 0, z);

                playerInstance.GetComponent<NavMeshAgent>().enabled = true;

                playerInstance.GetComponent<Player.Movement>().SetSpawn(playerInstance.transform.position);
                //Debug.Log(x + " " + z);
                //Debug.Log(playerInstance.transform.position);
            }
        }

        IEnumerator GenerateMaze()
        {
            navMeshGenerator.InitSurfaces(1);
            navMeshGenerator.AddSurface(floor);
            //for(int i=0; i<floorObjects.Count; i++)
            //{
            //    navMeshGenerator.AddSurface(floorObjects[i]);
            //}

            navMeshGenerator.Bake();

            List<Portal> portals = new List<Portal>();

            tiles = new MazeTile[imageSize.x, imageSize.y];

            int pacDots = 0;

            for (int y = 0; y < imageSize.y; y++) {
                for(int x=0; x<imageSize.x; x++)
                {
                    MazeTile tile = Instantiate(mazeTile);//CreateTile();

                    tile.name = "(" + x + "," + y + ")";

                    //Debug.Log(tile.name);

                    tile.transform.localPosition = new Vector3(x, 0, y) * tile.transform.localScale.x;

                    Color c = pixels[x, y];
                    if (CompareColors(c, imageColors[0]))
                    {
                        tile.SetTileType(MazeTile.TileType.PacDot);
                        pacDots++;
                        // Floor
                    }
                    else if (CompareColors(c, imageColors[1]))
                    {
                        //Debug.Log("Wall: " + tile.name);
                        tile.SetIsWall(true);
                        // Wall
                    }
                    else if (CompareColors(c, imageColors[2]))
                    {
                        ghostSpawns.Add(new Vector3(x, 0, y));
                        // EnemySpawn
                    }
                    else if (CompareColors(c, imageColors[3]))
                    {
                        //Debug.Log("Spawn Player: " + tile.name);
                        playerSpawn = new Vector3(x, 0, y);
                        //SpawnPlayer(x, y);
                        // PlayerSpawn
                    }
                    else if (CompareColors(c, imageColors[4]))
                    {
                        //Debug.Log("Portal");
                        Portal p = tile.GetComponent<Portal>();
                        if(p != null)
                        {
                            p.enabled = true;
                            tile.GetComponent<Collider>().enabled = true;
                            tile.tag = "Portal";
                            portals.Add(p);
                        }
                        // Portal
                    }
                    else if (CompareColors(c, imageColors[5]))
                    {
                        tile.SetTileType(MazeTile.TileType.PowerPellet);
                        
                        // Power Pellet
                    }
                    else
                    {
                        //Debug.Log("Odd color: " + c.r + " "+ c.g + " " + c.b + " at " + tile.name);
                    }
                    //tile.ToggleWall(false);

                    tiles[x, y] = tile;

                    tile.transform.parent = transform;

                    //Debug.Log(tile.name + " Color: " + c);
                }
            }

            for(int x=0; x<imageSize.x; x++)
            {
                for(int y=0; y<imageSize.y; y++)
                {
                    if (tiles[x, y].IsWall())
                    {
                        MazeTile t = tiles[x, y];
                        t.ToggleWall(MazeTile.WallTypes.Center, true);
                        if(x != 0 && tiles[x - 1, y].IsWall())
                        { 
                            t.ToggleWall(MazeTile.WallTypes.West, true);
                        }
                        if(x != imageSize.x - 1 && tiles[x + 1, y].IsWall())
                        {
                            t.ToggleWall(MazeTile.WallTypes.East, true);
                        }
                        if(y != 0 && tiles[x, y-1].IsWall())
                        {
                            t.ToggleWall(MazeTile.WallTypes.South, true);
                        }
                        if (y != imageSize.y - 1 && tiles[x, y + 1].IsWall())
                        {
                            t.ToggleWall(MazeTile.WallTypes.North, true);
                        }
                        //yield return null;
                    }
                }
            }


            SpawnPlayer(playerSpawn.x, playerSpawn.z);

            List<Ghost.Controller> ghosts = new List<Ghost.Controller>();

            GameManager gm = FindObjectOfType<GameManager>();
            gm.SetTotalPacDots(pacDots);
            gm.InitGhosts();
            for(int i=0; i<ghostSpawns.Count; i++)
            {
                GameObject ghost = Instantiate(ghostPrefab);


                ghost.transform.position = ghostSpawns[i];

                int x = Random.Range(0,sizeof(Ghost.Controller.GhostType));

                Ghost.Controller gc = ghost.GetComponent<Ghost.Controller>();


                gc.SetGhostType((Ghost.Controller.GhostType)x);
                gc.area = new Vector2(imageSize.x, imageSize.y);
                gc.SetSpawn(ghost.transform.position);

                ghosts.Add(gc);

                //Debug.Log("Adding ghost: " + gc.name);

                //Debug.Log(gm.name);
                gm.AddGhost(gc);

                ghost.GetComponent<NavMeshAgent>().enabled = true;

                gc.SetRandomDestination();
            }
            

            for(int i=0; i<portals.Count; i++)
            {
                portals[i].AddPortals(portals);
            }

            yield return null;
        }

        MazeTile CreateTile()
        {
            MazeTile tile = Instantiate(mazeTile) as MazeTile;
            tile.transform.parent = transform;
            return mazeTile;
        }
    }
}