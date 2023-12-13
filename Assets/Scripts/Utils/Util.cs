using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Util
{
    public static T[] FindChilds<T>(UnityEngine.Transform transform, bool recursive = false) where T : UnityEngine.Object
    {
        if (transform == null) return null;

        if (recursive == false)
        {
            T[] dd = new T[transform.childCount];
            
            for (int i = 0; i < transform.childCount; i++)
            {
                dd[i] = transform.GetChild(i).GetComponent<T>();
            }
            return dd;
        }
        else
        {
            return transform.GetComponentsInChildren<T>();
        }
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }
}
