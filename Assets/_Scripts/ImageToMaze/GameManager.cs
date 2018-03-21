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

        public void InitGhosts()
        {
            //Debug.Log("Init ghosts");
            ghosts = new List<Ghost.Controller>();
        }

        public void AddGhost(Ghost.Controller g)
        {
            //Debug.Log(g.name);
            ghosts.Add(g);
            //Debug.Log(ghosts.Count);
        }

        public void FrightenGhosts()
        {
            //Debug.Log("Frightening ghosts: " + ghosts.Count);
            for(int i=0; i<ghosts.Count; i++)
            {
                ghosts[i].SetGhostMode(Ghost.Controller.GhostMode.Frightened);
            }
        }

        public void KillPlayer()
        {
            //Debug.Log("Ghosts: " + ghosts.Count);
            playerLives--;
            UpdateLivesText();

            if (playerLives == 0)
            {
                EndGame();
            }
            else
            {
                Time.timeScale = 0;
                StartCoroutine(ResetAgents());
            }
        }

        public void EndGame()
        {
            loseCanvas.gameObject.SetActive(true);
            Debug.Log("Player lost the game!");
        }

        public void WinGame()
        {

            winCanvas.gameObject.SetActive(true);
            Debug.Log("Player won the game!");
        }

        IEnumerator ResetAgents()
        {
            //Debug.Log("Agents: " + ghosts.Count);
            for(int i=0; i<ghosts.Count; i++)
            {
                ghosts[i].Die();

            }

            FindObjectOfType<Player.Movement>().WarpToSpawn();

            //int start = System.DateTime.Now.Millisecond;
            //System
            //while (System.DateTime.Now.Millisecond - start < 1000) ;

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