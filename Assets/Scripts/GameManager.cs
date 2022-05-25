using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject hintPanel;
    [SerializeField] TextMeshProUGUI hintText;

    void Start()
    {
        SetUIHintActive(false);
    }

    public void SetUIHint(string button, string text)
    {
        //SetUIHintActive(true);
        hintText.text = $"[{button}]: {text}";
    }

    public void SetUIHintActive(bool active)
    {
        hintPanel.SetActive(active);
    }
}
