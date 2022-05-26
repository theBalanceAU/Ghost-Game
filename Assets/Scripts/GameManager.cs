using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    GameObject[] hearts;

    static GameManager instance;

    const int healthPointsPerHeart = 4;
    const int initialHeartCount = 3;

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
}
