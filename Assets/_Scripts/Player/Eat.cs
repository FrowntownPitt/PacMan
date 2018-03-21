using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2M;

namespace Player
{
    public class Eat : MonoBehaviour
    {
        public float dots;

        AudioSource waka;

        // Use this for initialization
        void Start()
        {
            waka = GetComponent<AudioSource>();
            dots = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PacDot"))
            {
                dots++;
                Destroy(other.gameObject);
                GameManager gm = FindObjectOfType<GameManager>();

                if (dots >= gm.GetTotalPacDots())
                {
                    Debug.Log(gm.GetTotalPacDots());
                    gm.WinGame();
                }
                gm.UpdateScore(1);
                waka.Play();
            }
            if (other.CompareTag("PowerPellet"))
            {
                Destroy(other.gameObject);

                GameManager gm = FindObjectOfType<GameManager>();
                gm.FrightenGhosts();
                gm.UpdateScore(5);

                waka.Play();
            }
        }
    }
}