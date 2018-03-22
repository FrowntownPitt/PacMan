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
            // Eat a Pac Dot
            if (other.CompareTag("PacDot"))
            {
                dots++;
                Destroy(other.gameObject);
                GameManager gm = FindObjectOfType<GameManager>();

                // If we ran out of dots, the game is won!
                if (dots >= gm.GetTotalPacDots())
                {
                    Debug.Log(gm.GetTotalPacDots());
                    gm.WinGame();
                }
                gm.UpdateScore(1);

                // Make the waka sound
                waka.Play();
            }

            // Eat a power pellet
            if (other.CompareTag("PowerPellet"))
            {
                Destroy(other.gameObject);

                GameManager gm = FindObjectOfType<GameManager>();

                // Frighten all ghosts and update the score
                gm.FrightenGhosts();
                gm.UpdateScore(5);

                // Make the waka sound
                waka.Play();
            }
        }
    }
}