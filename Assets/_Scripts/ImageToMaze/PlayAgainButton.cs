using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAgainButton : MonoBehaviour {

    public Button button;

    void Start()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(ResetScene);
    }

    // Reload the current scene (play again)
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
