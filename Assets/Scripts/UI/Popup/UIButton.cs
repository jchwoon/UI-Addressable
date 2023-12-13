using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class UIButton : UIPopup
{
    private AsyncOperationHandle _operationHandle;
    private GameObject _itemIconGameObject;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>();
        Bind<Button>();
        Bind<TextMeshProUGUI>();
        _itemIconGameObject = GetImage("itemIcon").gameObject;


        AddUIEvent(_itemIconGameObject, OnDrag, Define.UIEvent.Drag);
        AddUIEvent(GetButton("loadBtn").gameObject, OnClick, Define.UIEvent.Click);
        AddUIEvent(GetButton("unloadBtn").gameObject, UnloadSprite, Define.UIEvent.Click);
    }

    private void OnDrag(PointerEventData eventData)
    {
        _itemIconGameObject.transform.position = eventData.position;
    }

    private void OnClick(PointerEventData eventData)
    {
        Addressables.LoadAssetAsync<Sprite>("fire").Completed +=
        (AsyncOperationHandle<Sprite> Obj) =>
        {
            _operationHandle = Obj;
            _itemIconGameObject.GetComponent<Image>().sprite = Obj.Result;
        };
    }

    private void UnloadSprite(PointerEventData eventData)
    {
        Addressables.Release(_operationHandle);
        _itemIconGameObject.GetComponent<Image>().sprite = null;
    }
}
