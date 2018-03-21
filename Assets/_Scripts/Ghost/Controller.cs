using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    public class Controller : MonoBehaviour
    {

        public Material[] ghostMaterials;
        public Material frightenedMaterial;

        public GameObject coloredObject;

        public Vector2 area;

        [SerializeField]
        Movement movement;

        GameObject player;

        Vector3 spawnLocation;

        public float targetUpdateTimer;
        float startTime;

        public float frightenedTime;
        public float scatterTime;

        public float frightenedDistance;

        Coroutine frightenedCoroutine;
        Coroutine scatterCoroutine;

        public enum GhostType
        {
            Inky,
            Blinky,
            Pinky,
            Clyde,
        }

        public GhostType ghostType;
        public GhostMode ghostMode;

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
            SetGhostColor(ghostMaterials[(int)type]);

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
            ghostMode = GhostMode.Chase;

            if(frightenedCoroutine != null)
                StopCoroutine(frightenedCoroutine);
            frightenedCoroutine = null;

            SetGhostColor(ghostMaterials[(int)ghostType]);
            movement.WarpToLocation(spawnLocation);
        }

        // Update is called once per frame
        void Update()
        {
            if(player == null)
            {
                player =  GameObject.FindGameObjectWithTag("Player");
            }
            if(ghostMode == GhostMode.Chase)
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
            else if(ghostMode == GhostMode.Scatter)
            {
                if (scatterCoroutine == null)
                    scatterCoroutine = StartCoroutine(ScatterActions());
                if (movement.reachedTarget)
                {
                    SetRandomDestination();
                }
            }
            else if(ghostMode == GhostMode.Frightened)
            {
                if(frightenedCoroutine == null)
                    frightenedCoroutine = StartCoroutine(FrightenedActions());


                if (movement.reachedTarget || Time.time - startTime > targetUpdateTimer/10)
                {
                    startTime = Time.time;
                    Vector3 nearRand;
                    nearRand.x = Random.Range(transform.position.x - frightenedDistance, transform.position.x + frightenedDistance);
                    nearRand.y = 0;
                    nearRand.z = Random.Range(transform.position.z - frightenedDistance, transform.position.z + frightenedDistance);
                    SetDestination(nearRand);
                }
            }
        }

        public IEnumerator FrightenedActions()
        {
            //Debug.Log("Frightening");
            SetGhostColor(frightenedMaterial);

            if(scatterCoroutine != null)
            {
                StopCoroutine(scatterCoroutine);
                scatterCoroutine = null;
            }

            float start = Time.time;
            while (Time.time - start < frightenedTime)
            {
                yield return null;
            }

            ghostMode = GhostMode.Chase;

            frightenedCoroutine = null;

            SetGhostColor(ghostMaterials[(int)ghostType]);
            yield return null;
        }

        public IEnumerator ScatterActions()
        {
            if (frightenedCoroutine != null)
            {
                StopCoroutine(frightenedCoroutine);
                scatterCoroutine = null;
            }
            yield return null;

            float start = Time.time;
            while(Time.time - start < scatterTime)
            {
                yield return null;
            }

            ghostMode = GhostMode.Chase;
            yield return null;
        }

        public void SetDestination(Vector3 target)
        {
            movement.AddDestination(target);
        }

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