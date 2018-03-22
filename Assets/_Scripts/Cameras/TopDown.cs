using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    // Move the camera to encompass the map
    public class TopDown : MonoBehaviour
    {
        // What are we going to have the camera move to view (floor plane)
        public GameObject focusTarget;
        public float border; // make sure there is a buffer around the object

        Camera camera;

        Transform focusTransform;
        MeshRenderer focusMesh;

        // Use this for initialization
        void Start()
        {
            camera = GetComponent<Camera>();
            focusMesh = focusTarget.GetComponent<MeshRenderer>();
            focusTransform = focusMesh.transform;

            
            //Debug.Log("Scale: " + focusMesh.bounds.size.z);
        }

        // Update is called once per frame
        void Update()
        {
            // Get the distance required to have the entire floor plane to be visible
            float distance = (focusMesh.bounds.size.z + 2*border) * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            transform.position = new Vector3(focusTarget.transform.position.x, distance, focusTarget.transform.position.z);
        }
    }
}