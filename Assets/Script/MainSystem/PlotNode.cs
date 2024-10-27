using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlotNode
{
    public Sprite picture;

    [TextArea]
    public string content;

    [TextArea]
    public string choiceAtext;

    [TextArea]
    public string choiceBtext;

    [TextArea]
    public string choiceCtext;

    [TextArea]
    public string buttonAtext;

    [TextArea]
    public string buttonBtext;

    [TextArea]
    public string buttonCtext;

}
