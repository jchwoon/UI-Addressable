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
    private GameObject _playerImageGameObject;
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
        PlayerData playerData = Managers.DataManager.PlayerDat;

        _playerImageGameObject = GetImage("playerImage").gameObject;
        GetText("PlayerTypeTxt").text = "Warrior";
        GetText("HpTxt").text = "100";
        GetText("DamageTxt").text = "20";
        GetText("DefenceTxt").text = "5";
        GetText("LevelTxt").text = "1";
        GetText("CriticalTxt").text = "10";
        GetText("GoldTxt").text = "2000";


        Managers.ResourceManager.LoadAsync<Sprite>("fire");



        AddUIEvent(_playerImageGameObject, OnDrag, Define.UIEvent.Drag);
        AddUIEvent(GetButton("loadBtn").gameObject, OnClick, Define.UIEvent.Click);
        AddUIEvent(GetButton("unloadBtn").gameObject, UnloadSprite, Define.UIEvent.Click);
    }

    private void OnDrag(PointerEventData eventData)
    {
        _playerImageGameObject.transform.position = eventData.position;
    }

    private void OnClick(PointerEventData eventData)
    {
        _playerImageGameObject.GetComponent<Image>().sprite = Managers.ResourceManager.Load<Sprite>("fire");
    }

    private void UnloadSprite(PointerEventData eventData)
    {
        Managers.ResourceManager.Destroy("fire");
        _playerImageGameObject.GetComponent<Image>().sprite = null;
    }
}
