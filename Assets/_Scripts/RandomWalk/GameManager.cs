using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomWalk
{
    public class GameManager : MonoBehaviour
    {

        public Maze mazePrefab;

        private Maze mazeInstance;
        Coroutine generator;

        void Start()
        {
            BeginGame();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }

        public void BeginGame()
        {
            mazeInstance = Instantiate(mazePrefab) as Maze;
            generator = StartCoroutine(mazeInstance.Generate());
        }

        public void RestartGame()
        {
            StopCoroutine(generator);
            Destroy(mazeInstance.gameObject);
            BeginGame();
        }
    }
}