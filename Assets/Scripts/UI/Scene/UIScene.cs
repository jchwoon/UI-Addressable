using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene : MonoBehaviour
{
    public virtual void Init()
    {
        Managers.UIManager.SetCanvas(gameObject, false);
    }
}
