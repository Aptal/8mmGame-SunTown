using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour
{

    public Texture2D cursorTexture;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}