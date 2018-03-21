using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    public class TopDown : MonoBehaviour
    {

        public GameObject focusTarget;
        public float border;

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

            float distance = (focusMesh.bounds.size.z + 2*border) * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            transform.position = new Vector3(focusTarget.transform.position.x, distance, focusTarget.transform.position.z);
        }
    }
}