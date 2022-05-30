using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrollingBackground : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] SpriteRenderer near;
    [SerializeField] SpriteRenderer mid;
    [SerializeField] SpriteRenderer far;

    float nearLayerSpeed = 4f;
    float midLayerSpeed = 1f;
    float width;

    SpriteRenderer[] nearLayer;
    SpriteRenderer[] midLayer;

    void Start()
    {
        // get the width of the image being scrolled
        // (making some possibly dodgy assumptions here, but it's working for now)
        width = near.sprite.texture.width / near.sprite.pixelsPerUnit;

        InitScrollingLayer(ref nearLayer, near);
        InitScrollingLayer(ref midLayer, mid);
    }

    void Update()
    {
        // check the width of the sprite again every frame, just in case
        width = near.sprite.texture.width / near.sprite.pixelsPerUnit;

        ScrollLayer(ref nearLayer, nearLayerSpeed);
        ScrollLayer(ref midLayer, midLayerSpeed);
    }

    void InitScrollingLayer(ref SpriteRenderer[] layer, SpriteRenderer image)
    {
        // clone the background layer three times, positioning the clones to the left and right of the original
        layer = new SpriteRenderer[3];
        layer[0] = image;
        layer[1] = Instantiate(image, image.transform.parent);
        layer[1].transform.Translate(Vector3.left * width, Space.Self);
        layer[2] = Instantiate(image, image.transform.parent);
        layer[2].transform.Translate(Vector3.right * width, Space.Self);
    }

    void ScrollLayer(ref SpriteRenderer[] layer, float speed)
    {
        for (int i = 0; i < layer.Length; i++)
        {
            // move each image on this layer to the left at a constant speed
            layer[i].transform.position += Vector3.left * speed * Time.deltaTime;

            // once the image reaches the offscreen threshold, move it back across to the far right side of the other clones
            if (layer[i].transform.position.x < -width)
            {
                layer[i].transform.position += Vector3.right * (width*3);
            }
        }
    }
}
