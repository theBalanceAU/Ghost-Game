using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public void Resume_OnClick()
    {
        GameManager.Instance.ResumeGame();
    }

    public void MainMenu_OnClick()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.ChangeScene("MainMenu", 2f, false);
    }
}
