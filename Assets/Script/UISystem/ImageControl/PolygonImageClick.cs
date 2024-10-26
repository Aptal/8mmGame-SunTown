using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyImage : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = transform.GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.1f;
    }
}
