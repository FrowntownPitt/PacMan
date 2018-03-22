using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    // General controller for the ghosts.  Handles location targeting and ghost death
    public class Controller : MonoBehaviour
    {

        public Material[] ghostMaterials;   // Colors of the different ghosts. Index is the GhostType
        public Material frightenedMaterial; // Color for when the ghost is frightened (Power pellet)

        public GameObject coloredObject;

        public Vector2 area;   // Bounds of the playing area

        [SerializeField]
        Movement movement;     // Ghost movement script

        GameObject player;

        Vector3 spawnLocation; // "Home", for when it dies

        public float targetUpdateTimer;
        float startTime;

        public float frightenedTime;
        public float scatterTime;

        public float frightenedDistance;

        Coroutine frightenedCoroutine;  // Pool of the Frightened coroutines (only have 1 active)
        Coroutine scatterCoroutine;     // Pool of the Scatter coroutines (only have 1 active)

        // 4 ghosts, classic PacMan names
        public enum GhostType
        {
            Inky,
            Blinky,
            Pinky,
            Clyde,
        }

        public GhostType ghostType; // Which ghost it is
        public GhostMode ghostMode; // Current mode of the ghost

        public enum GhostMode
        {
            Chase,
            Scatter,
            Frightened,
        }

        public void SetSpawn(Vector3 pos)
        {
            spawnLocation = pos;
        }

        public void SetGhostType(GhostType type)
        {
            ghostType = type;

            //Debug.Log((int)type);
            // Change its color
            SetGhostColor(ghostMaterials[(int)type]);

            // Change its name
            name = System.Enum.GetName(typeof(GhostType), type);
        }

        private void SetGhostColor(Material mat)
        {
            coloredObject.GetComponent<Renderer>().material = mat;
        }

        public void SetGhostMode(GhostMode mode)
        {
            ghostMode = mode;
        }

        // Use this for initialization
        void Start()
        {
            // pool all necessary variables
            movement = GetComponent<Movement>();
            player = GameObject.FindGameObjectWithTag("Player");

            startTime = Time.time;
        }

        private void OnEnable()
        {

            movement = GetComponent<Movement>();
            player = GameObject.FindGameObjectWithTag("Player");
            startTime = Time.time;
        }

        public void Die()
        {
            // Set it back to chasing (after it "respawns")
            ghostMode = GhostMode.Chase;

            // Make sure it isn't moving anymore
            if(frightenedCoroutine != null)
                StopCoroutine(frightenedCoroutine);
            frightenedCoroutine = null;

            // Set it back to its original color
            SetGhostColor(ghostMaterials[(int)ghostType]);

            // Send it home to start over again
            movement.WarpToLocation(spawnLocation);
        }

        // Update is called once per frame
        void Update()
        {
            if(player == null)
            {
                player =  GameObject.FindGameObjectWithTag("Player");
            }
            if(ghostMode == GhostMode.Chase) // Chasing is ghost-dependent
                switch (ghostType)
                {
                    case GhostType.Inky:
                        {
                            // Follow in the path of the player
                            if (movement.reachedTarget || Time.time - startTime > targetUpdateTimer)
                            {
                                startTime = Time.time;
                                movement.AddDestinationAdditive(player.transform.position);
                            }
                            break;
                        }
                    case GhostType.Blinky:
                        {
                            // Target the player
                            if (movement.reachedTarget || Time.time - startTime > targetUpdateTimer)
                            {
                                startTime = Time.time;
                                SetDestination(player.transform.position);
                            }
                            break;
                        }
                    case GhostType.Pinky:
                        {
                            // Target the player's target
                            if (movement.reachedTarget || Time.time - startTime > targetUpdateTimer)
                            {
                                startTime = Time.time;
                                SetDestination(player.GetComponent<NavMeshAgent>().destination);
                            }
                            break;
                        }
                    case GhostType.Clyde:
                        {
                            // Just move randomly.  Clyde is dumb.
                            if (movement.reachedTarget)
                            {
                                SetRandomDestination();
                                //SetDestination(new Vector3(10, 0, 10));
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            else if(ghostMode == GhostMode.Scatter)  // On scatter, pick a random destination and go to it
            {
                if (scatterCoroutine == null) // Ensure only 1 instance of the coroutine is active
                    scatterCoroutine = StartCoroutine(ScatterActions());
                if (movement.reachedTarget)
                {
                    SetRandomDestination();
                }
            }
            else if(ghostMode == GhostMode.Frightened)  // On frightened, pick a nearby random point and go to it
            {
                if(frightenedCoroutine == null)
                    frightenedCoroutine = StartCoroutine(FrightenedActions());

                // Go to 10 random nearby points in the timespan
                if (movement.reachedTarget || Time.time - startTime > frightenedTime/10)
                {
                    startTime = Time.time;
                    Vector3 nearRand;
                    nearRand.x = Random.Range(transform.position.x - frightenedDistance, transform.position.x + frightenedDistance);
                    nearRand.y = 0;
                    nearRand.z = Random.Range(transform.position.z - frightenedDistance, transform.position.z + frightenedDistance);
                    SetDestination(nearRand); // Set the random destination
                }
            }
        }

        public IEnumerator FrightenedActions()
        {
            //Debug.Log("Frightening");
            SetGhostColor(frightenedMaterial); // Make it look frightened

            // Make sure scatter is not running
            if(scatterCoroutine != null)
            {
                StopCoroutine(scatterCoroutine);
                scatterCoroutine = null;
            }

            float start = Time.time;
            // Wait until the timer is up
            while (Time.time - start < frightenedTime)
            {
                yield return null;
            }

            // Go back to normal
            ghostMode = GhostMode.Chase;

            frightenedCoroutine = null;

            SetGhostColor(ghostMaterials[(int)ghostType]);
            yield return null;
        }

        public IEnumerator ScatterActions()
        {
            // Ensure the ghost is not frightened
            if (frightenedCoroutine != null)
            {
                StopCoroutine(frightenedCoroutine);
                scatterCoroutine = null;
            }
            yield return null;

            float start = Time.time;
            // Scatter for the duration
            while(Time.time - start < scatterTime)
            {
                yield return null;
            }

            // Go back to normal
            ghostMode = GhostMode.Chase;
            yield return null;
        }

        public void SetDestination(Vector3 target)
        {
            movement.AddDestination(target);  // Add the destination as immediate
        }

        // Pick a random location in the play area and go to it
        public void SetRandomDestination()
        {
            if(movement == null)
            {
                movement = GetComponent<Movement>();
            }
            if(movement != null)
                SetDestination(new Vector3(Random.Range(0, area.x), 0, Random.Range(0, area.y)));
        }
    }
}