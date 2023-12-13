using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    bool isPressed = false;
    public void OnUpdate()
    {
        if (Input.anyKey != false && KeyAction != null) KeyAction.Invoke();

        if (Input.GetMouseButton(1))
        {
            MouseAction.Invoke(Define.MouseEvent.Click);
            isPressed = true;
        }
        else
        {
            MouseAction.Invoke(Define.MouseEvent.Press);
            isPressed = false;
        }
    }
}
