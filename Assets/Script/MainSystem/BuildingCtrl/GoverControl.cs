using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoverControl : MonoBehaviour
{
    public Image imageRoadFix;

    private void Start()
    {
        //imageRoadFix = GameObject.Find("image_roadFix").GetComponent<Image>();
        if (imageRoadFix == null)
        {
            Debug.LogError("image_roadFix not found!");
        }
    }

    public void GoverButton()
    {
        if (TimeControl.Instance.happyCtrl.happyValue >= 50)
        {
            imageRoadFix.enabled = true;
            TimeControl.Instance.canFix = true;
        }
        else
        {
            imageRoadFix.enabled = false;
        }
    }
}
