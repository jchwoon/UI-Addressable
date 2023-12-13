using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    private Dictionary<Type, UnityEngine.Object[]> _componentDictionary = new Dictionary<Type, UnityEngine.Object[]>();
    protected void Bind<T>() where T : UnityEngine.Object
    {
        //필터 해줄 타입 추가
        if (typeof(T) != typeof(Button) && typeof(T) != typeof(TextMeshProUGUI) && typeof(T) != typeof(Image) && typeof(T) != typeof(GameObject)) return;


        if (typeof(T) == typeof(GameObject))
        {
            Transform[] transforms = Util.FindChilds<Transform>(transform, true);
            GameObject[] gameObjects = new GameObject[transforms.Length];
            for (int i = 0; i < transforms.Length; i++)
            {
                gameObjects[i] = transforms[i].gameObject;
            }

            _componentDictionary.Add(typeof(GameObject), gameObjects);
            return;
        }

        T[] objects = Util.FindChilds<T>(transform, true);

        _componentDictionary.Add(typeof(T), objects);
    }

    protected T Get<T>(string name) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_componentDictionary.TryGetValue(typeof(T), out objects) == false) return null;

        foreach (UnityEngine.Object obj in objects)
        {
            if (obj.name == name) return obj as T;
        }

        return null;
    }

    protected void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent uIEvent)
    {
        UIEventHandler uiEventHandler = Util.GetOrAddComponent<UIEventHandler>(go);


        switch (uIEvent)
        {
            case Define.UIEvent.Click:
                uiEventHandler.ClickAction -= action;
                uiEventHandler.ClickAction += action;
                break;
            case Define.UIEvent.Drag:
                uiEventHandler.DragAction -= action;
                uiEventHandler.DragAction += action;
                break;
        }
    }

    protected TextMeshProUGUI GetText(string name) { return Get<TextMeshProUGUI>(name); }
    protected Button GetButton(string name) { return Get<Button>(name); }
    protected Image GetImage(string name) { return Get<Image>(name); }
    protected GameObject GetGameObject(string name) { return Get<GameObject>(name); }
}
