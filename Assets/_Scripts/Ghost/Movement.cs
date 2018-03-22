using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    public class Movement : MonoBehaviour
    {

        NavMeshAgent agent; // this ghost's agent

        bool warping = false;

        List<Vector3> destinations; // Queue of waypoints

        public float approachDistance;

        public bool reachedTarget = false;
        
        // Use this for initialization
        void Start()
        {
            destinations = new List<Vector3>();
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            // Once it reaches its target, go to next or stop there
            if(!reachedTarget && Vector3.Magnitude(transform.position - agent.destination) <= approachDistance)
            {
                if (destinations.Count > 0)
                {
                    //Debug.Log(name + " Setting next target");
                    agent.destination = destinations[0];
                    //agent.SetDestination(destinations[0]);
                    destinations.RemoveAt(0);
                    reachedTarget = false;
                }
                else
                {
                    //Debug.Log(name + " Reached Target");
                    reachedTarget = true;
                    //agent.ResetPath();
                    //agent.isStopped = true;
                }
            }
        }

        // Change the destination to this position
        public void AddDestination(Vector3 position)
        {
            reachedTarget = false;

            if(destinations == null)
            {
                destinations = new List<Vector3>();
            }
            destinations.Clear();

            NavMeshHit hit;
            // Make sure the target location is on the navmesh
            NavMesh.SamplePosition(position, out hit, 100f, NavMesh.AllAreas);

            position = hit.position;

            destinations.Add(position);

            if(agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            agent.destination = position;
            //agent.SetDestination(position);


            //Debug.Log(name + "Set destination: " + position);
        }

        // Add this position as a waypoint
        public void AddDestinationAdditive(Vector3 position)
        {

            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, 100f, NavMesh.AllAreas);

            position = hit.position;

            if (destinations == null)
            {
                destinations = new List<Vector3>();
            }
            destinations.Add(position); // Add the target to the queue

            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            //agent.SetDestination(position);

            reachedTarget = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            // If it reaches a portal, warp to a random other portal
            if (!warping && other.CompareTag("Portal"))
            {
                Debug.Log("Entered Portal");
                Portal p = other.gameObject.GetComponent<Portal>();

                agent.isStopped = true;
                agent.ResetPath();

                // Make sure it doesn't warp again after it warps in to another trigger
                warping = true;

                // Make sure we warp into a valid navmesh location
                WarpToLocation(p.GetRandomPortal().transform.position);
                //agent.Warp(p.GetRandomPortal().transform.position);

                warping = false;

                //Debug.Log(p.name);
            }
        }

        public void WarpToLocation(Vector3 position)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, 100f, NavMesh.AllAreas);

            position = hit.position;
            agent.Warp(position);
        }
    }
}