using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Load Scene")]
    [SerializeField] bool loadScene;
    [SerializeField] string sceneName;
    [SerializeField] float delaySceneLoad;

    [Header("Play Sound")]
    [SerializeField] bool playSound;
    [SerializeField] AudioClip soundClip;

    [Header("Pickup Object")]
    [SerializeField] bool pickupObject;
    
    PlayerInteraction playerInteraction;
    // public GameManager gameManager;

    void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    void Start()
    {
        // gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // show prompt for interact
            GameManager.Instance.SetUIHintActive(true);
            GameManager.Instance.SetUIHint("Interact", name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // hide interaction prompt
            GameManager.Instance.SetUIHintActive(false);
        }
    }

    void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, boxCollider.size);
    }

    public void DoInteraction(PlayerInteraction other)
    {
        Debug.Log($"DoInteraction()");

        if (loadScene && !string.IsNullOrEmpty(sceneName))
        {
            GameManager.Instance.SetUIHintActive(false);

            Debug.Log($"LOAD SCENE {sceneName}");
            if (delaySceneLoad > 0)
            {
                Debug.Log($"Delay for {delaySceneLoad} seconds");
                //TODO: Create SceneManager to open scene with optional delay and transition effects
            }
            else
            {
                
            }

            SceneManager.LoadSceneAsync(sceneName);
            
        }

        if (playSound && soundClip)
        {
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
        }

        if (pickupObject)
        {
            GameObject pickup = transform.parent.gameObject;

            Debug.Log($"Pickup object {pickup.name}");

            other.PickupObject(pickup);
        }
    }
}
