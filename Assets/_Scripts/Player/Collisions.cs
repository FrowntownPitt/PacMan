using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2M;

public class Collisions : MonoBehaviour {
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            //Debug.Log("Hit ghost!");

            Ghost.Controller ghost = other.GetComponent<Ghost.Controller>();

            if(ghost.ghostMode == Ghost.Controller.GhostMode.Frightened)
            {
                //Debug.Log("Eat");

                GameManager gm = FindObjectOfType<GameManager>();
                gm.FrightenGhosts();
                gm.UpdateScore(10);

                ghost.Die();
            } else
            {
                //Debug.Log("Die");
                FindObjectOfType<I2M.GameManager>().KillPlayer();
            }
        }
    }
}
