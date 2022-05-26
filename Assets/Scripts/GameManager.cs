using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject hintPanel;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] GameObject heartContainer;

    // each full heart container equals 4 health points
    [SerializeField] int healthPoints;
    [SerializeField] int score;

    [SerializeField] Sprite fullHeartSprite;
    [SerializeField] Sprite[] partialHeartSprites;

    static GameManager instance;

    const int healthPointsPerHeart = 4;
    const int initialHeartCount = 3;

    Vector2 playerSpawn;

    public static GameManager Instance
    {
        get {
            return instance;
        }
    }

    void Awake()
    {
        int instances = FindObjectsOfType<GameManager>().Length;
        if (instances > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    void Start()
    {
        SetUIHintActive(false);
        UpdateHealthDisplay();
    }

    public void SetUIHint(string button, string text)
    {
        hintText.text = $"[{button}]: {text}";
    }

    public void SetUIHintActive(bool active)
    {
        hintPanel.SetActive(active);
    }

    public void ResetHealth()
    {
        healthPoints = initialHeartCount * healthPointsPerHeart;
    }

    public void UpdateHealthDisplay()
    {
        int fullHearts = healthPoints / healthPointsPerHeart;
        int remainder = healthPoints % healthPointsPerHeart;

        int heartContainerCount = heartContainer.transform.childCount;

        for (int i = 0; i < heartContainerCount; i++)
        {
            GameObject heartObject = heartContainer.transform.GetChild(i).gameObject;
            Image heartImage = heartObject.GetComponent<Image>();

            Sprite heartSprite = fullHeartSprite;
            if (i == fullHearts)
            {
                heartSprite = partialHeartSprites[remainder];
            }
            else if (i > fullHearts)
            {
                heartSprite = partialHeartSprites[0];
            }
            heartImage.sprite = heartSprite;
        }
    }

    public void SetPlayerSpawnPosition(Vector2 position)
    {
        playerSpawn = position;
    }

    public Vector2 GetPlayerSpawnPosition()
    {
        return playerSpawn;
    }

    public void ChangeScene(string sceneName, float delaySceneLoad)
    {
        StartCoroutine(ChangeSceneCo(sceneName, delaySceneLoad));
    }

    IEnumerator ChangeSceneCo(string sceneName, float delaySceneLoad)
    {
        // play crossfade animation (if one is present in the current scene)
        SceneCrossfade crossFade = FindObjectOfType<SceneCrossfade>();
        crossFade?.FadeOut();

        if (delaySceneLoad > 0)
        {
            Debug.Log($"Delay for {delaySceneLoad} seconds");
            yield return new WaitForSeconds(delaySceneLoad);
        }

        SceneManager.LoadSceneAsync(sceneName);
    }
}
