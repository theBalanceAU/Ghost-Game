using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start_OnClick()
    {
        GameManager.Instance.ChangeScene("Overworld", 2f);
        //SceneManager.LoadScene("Overworld");
    }

    public void Quit_OnClick()
    {
        Application.Quit();
    }
}
