using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private TextAsset _playerJson;
    private TextAsset _weaponJson;
    private TextAsset _armorJson;

    private PlayerData _playerData;
    private WeaponData _weaponData;
    private ArmorData _armorData;

    public PlayerData PlayerDat { get { return _playerData; } }
    public WeaponData WeaponData { get; }
    public ArmorData ArmorData { get; }
    public void Init()
    {
        _playerJson = Managers.ResourceManager.Load<TextAsset>("player");
        _weaponJson = Managers.ResourceManager.Load<TextAsset>("weapon");
        _armorJson = Managers.ResourceManager.Load<TextAsset>("armor");

        AllPlayerData allPlayerData = JsonUtility.FromJson<AllPlayerData>(_playerJson.text);
        Debug.Log(allPlayerData.player[0].damage);
        Debug.Log(allPlayerData.player[0].defence);
        Debug.Log(allPlayerData.player[0].critical);
        Debug.Log(allPlayerData.player[0].gold);
        Debug.Log(allPlayerData.player[0].hp);
        Debug.Log(allPlayerData.player[0].playerType);
        Debug.Log(allPlayerData.player[0].level);

        //_playerData.damage = allPlayerData.player[0].damage;
        //_playerData.defence = allPlayerData.player[0].defence;
        //_playerData.critical = allPlayerData.player[0].critical;
        //_playerData.gold = allPlayerData.player[0].gold;
        //_playerData.hp = allPlayerData.player[0].hp;
        //_playerData.playerType = allPlayerData.player[0].playerType;
        //_playerData.level = allPlayerData.player[0].level;
    }

}

enum PlayerTypes
{
    Warrior,
    Mage,
}

public class AllPlayerData
{
    public PlayerData[] player;
}

[System.Serializable]
public class PlayerData
{
    public string playerType;
    public int hp;
    public int damage;
    public int defence;
    public int critical;
    public int level;
    public int gold;
}

[System.Serializable]
public class WeaponData
{
    private PlayerTypes _weaponType;
    private int _damage;
    private string _name;
    public int WeaponType { get; set; }
    public int Damage { get; set; }
    public int Name { get; set; }
}

[System.Serializable]
public class ArmorData
{
    private PlayerTypes _armorType;
    private int _defence;
    private string _name;

    public int ArmorType { get; set; }
    public int Defence { get; set; }
    public int Name { get; set; }
}