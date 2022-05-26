using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneCrossfade : MonoBehaviour
{
    [SerializeField] Animator crossFade;

    // Note: Fade In will currently happen automatically on load of the scene

    public void FadeOut()
    {
        crossFade?.SetTrigger("fadeOut");
    }
}
