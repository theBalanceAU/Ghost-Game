using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScrollingBackground : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] Image near;
    [SerializeField] Image mid;
    [SerializeField] Image far;

    float nearLayerSpeed = 4f;
    float midLayerSpeed = 1f;
    float width;

    Image[] nearLayer;
    Image[] midLayer;

    void Start()
    {
        // get the width of the image being scrolled
        // (making some possibly dodgy assumptions here, but it's working for now)
        width = near.GetComponent<RectTransform>().rect.width;

        InitScrollingLayer(ref nearLayer, near);
        InitScrollingLayer(ref midLayer, mid);
    }

    void FixedUpdate()
    {
        ScrollLayer(ref nearLayer, nearLayerSpeed);
        ScrollLayer(ref midLayer, midLayerSpeed);
    }

    void InitScrollingLayer(ref Image[] layer, Image image)
    {
        // clone the background layer three times, positioning the clones to the left and right of the original
        layer = new Image[3];
        layer[0] = image;
        layer[1] = Instantiate(image, image.transform.parent);
        layer[1].transform.position += Vector3.left * width;
        layer[2] = Instantiate(image, image.transform.parent);
        layer[2].transform.position += Vector3.right * width;
    }

    void ScrollLayer(ref Image[] layer, float speed)
    {
        for (int i = 0; i < layer.Length; i++)
        {
            // move each image on this layer to the left at a constant speed
            layer[i].transform.position += Vector3.left * speed;

            // once the image reaches threshold, move it back across to the far right side of the other clones
            if (layer[i].transform.position.x < -width)
            {
                layer[i].transform.position += Vector3.right * (width*3);
            }
        }
    }
}
