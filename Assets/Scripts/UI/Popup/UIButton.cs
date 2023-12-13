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
       //resources, resourcesHandle에 로드
        Managers.ResourceManager.LoadAsync<Sprite>("fire");

        AddUIEvent(_itemIconGameObject, OnDrag, Define.UIEvent.Drag);
        AddUIEvent(GetButton("loadBtn").gameObject, OnClick, Define.UIEvent.Click);
        AddUIEvent(GetButton("unloadBtn").gameObject, UnloadSprite, Define.UIEvent.Click);
    }

    private void OnDrag(PointerEventData eventData)
    {
        _itemIconGameObject.transform.position = eventData.position;
    }

    //load버튼 클릭하면 Image에 fire sprite 추가
    private void OnClick(PointerEventData eventData)
    {
        _itemIconGameObject.GetComponent<Image>().sprite = Managers.ResourceManager.Load<Sprite>("fire");
    }

    //unload버튼 클릭하면 해당 스프라이트 메모리 해제
    private void UnloadSprite(PointerEventData eventData)
    {
        Managers.ResourceManager.Destroy(_itemIconGameObject.GetComponent<Image>().sprite.name);
        //_itemIconGameObject.GetComponent<Image>().sprite = null;
    }
}
