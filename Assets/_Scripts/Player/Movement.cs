using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        
        NavMeshAgent agent;

        public float speed;
        public float turnClamp;

        public int dots;

        bool warping = false;

        Vector3 spawnLocation;

        // Use this for initialization
        void Start()
        {
            dots = 0;
            agent = GetComponent<NavMeshAgent>();
        }

        public void SetSpawn(Vector3 pos)
        {
            spawnLocation = pos;
        }

        public void WarpToSpawn()
        {
            agent.Warp(spawnLocation);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))  // Find the location of the click on the floor
                {
                    NavMeshHit navHit;
                    // Get the nearest navmesh location
                    if (NavMesh.SamplePosition(hit.point, out navHit, 100, NavMesh.AllAreas))
                    {
                        //Debug.Log(hit.transform.position + ", " + navHit.position);
                        agent.destination = navHit.position;
                    }
                }
            }

            /*Vector3 intendedDir = agent.desiredVelocity.normalized;
            float speedMod = Vector3.Dot(transform.forward, intendedDir);
            agent.speed = speed * Mathf.Max(speedMod, turnClamp);*/
        }

        private void OnTriggerEnter(Collider other)
        {
            // On portal entry, go to a random other portal
            if (!warping && other.CompareTag("Portal"))
            {
                //Debug.Log("Entered Portal");
                Portal p = other.gameObject.GetComponent<Portal>();

                agent.isStopped = true;
                agent.ResetPath();

                warping = true;

                agent.Warp(p.GetRandomPortal().transform.position);

                warping = false;

                //Debug.Log(p.name);
            }
        }
    }
}