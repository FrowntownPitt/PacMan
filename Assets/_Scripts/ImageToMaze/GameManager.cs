using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace I2M
{
    public class GameManager : MonoBehaviour
    {
        public Generator gameGenerator;

        private int totalPacDots;

        List<Ghost.Controller> ghosts;

        public Text LivesText;
        public Text ScoreText;

        public Canvas loseCanvas;
        public Canvas winCanvas;

        public int playerLives;
        public int score;


        // Use this for initialization
        void Start()
        {
            // Disable the end canvases. They cover the screen when active
            winCanvas.gameObject.SetActive(false);
            loseCanvas.gameObject.SetActive(false);

            totalPacDots = 0;
            score = 0;
            //ghosts = new List<Ghost.Controller>();

            UpdateScoreText();
            UpdateLivesText();

            gameGenerator.StartGenerator();
        }

        public void SetTotalPacDots(int c)
        {
            totalPacDots = c;
        }

        public int GetTotalPacDots()
        {
            return totalPacDots;
        }

        private void UpdateLivesText()
        {
            LivesText.text = "Lives: " + playerLives;
        }

        private void UpdateScoreText()
        {
            ScoreText.text = "Score:\n" + score;
        }

        public void UpdateScore(int points)
        {
            score += points;
            UpdateScoreText();
        }

        // Make the ghosts list not null
        public void InitGhosts()
        {
            //Debug.Log("Init ghosts");
            ghosts = new List<Ghost.Controller>();
        }

        // Add a ghost to our list of active ghosts
        public void AddGhost(Ghost.Controller g)
        {
            //Debug.Log(g.name);
            ghosts.Add(g);
            //Debug.Log(ghosts.Count);
        }

        // Make all of the ghosts go into their frightened states
        public void FrightenGhosts()
        {
            //Debug.Log("Frightening ghosts: " + ghosts.Count);
            for(int i=0; i<ghosts.Count; i++)
            {
                ghosts[i].SetGhostMode(Ghost.Controller.GhostMode.Frightened);
            }
        }

        // The player hit a ghost, kill them
        public void KillPlayer()
        {
            //Debug.Log("Ghosts: " + ghosts.Count);
            playerLives--;
            UpdateLivesText();

            if (playerLives == 0)
            {
                // They lost all their lives, end the game
                EndGame();
            }
            else
            {
                // Do not let the ghosts or player move while we wait for a reset
                Time.timeScale = 0;
                // reset the ghosts
                StartCoroutine(ResetAgents());
            }
        }

        public void EndGame()
        {
            // Activate the lose screen
            loseCanvas.gameObject.SetActive(true);
            Debug.Log("Player lost the game!");
        }

        public void WinGame()
        {
            // Activate the win screen
            winCanvas.gameObject.SetActive(true);
            Debug.Log("Player won the game!");
        }

        IEnumerator ResetAgents()
        {
            //Debug.Log("Agents: " + ghosts.Count);
            // "kill" off all of the ghosts
            for(int i=0; i<ghosts.Count; i++)
            {
                ghosts[i].Die();

            }

            // Move player to his spawn point
            FindObjectOfType<Player.Movement>().WarpToSpawn();

            //int start = System.DateTime.Now.Millisecond;
            //System
            //while (System.DateTime.Now.Millisecond - start < 1000) ;

            // re-enable time
            Time.timeScale = 1;
            //Debug.Log("Player lives: " + playerLives);
            yield return null;
        }
        
        public int GettotalPacDots()
        {
            return totalPacDots;
        }

        public void SettotalPacDots(int value)
        {
            totalPacDots = value;
        }
    }
}