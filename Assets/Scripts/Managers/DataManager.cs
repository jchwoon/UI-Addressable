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
    public void Init()
    {
        _playerJson = Managers.ResourceManager.Load<TextAsset>("player");
        _weaponJson = Managers.ResourceManager.Load<TextAsset>("weapon");
        _armorJson = Managers.ResourceManager.Load<TextAsset>("armor");

        _playerData = JsonUtility.FromJson<PlayerData>(_playerJson.text);
        _weaponData = JsonUtility.FromJson<WeaponData>(_weaponJson.text);
        _armorData = JsonUtility.FromJson<ArmorData>(_armorJson.text);


        Debug.Log(_armorData.ArmorType);
        Debug.Log(_weaponData.Damage);
    }

}

enum PlayerType
{
    Warrior,
    Mage,
}

[System.Serializable]
public class PlayerData
{
    private PlayerType _playerType;
    private int _hp;
    private int _damage;
    private int _defence;
    private int _critical;
    private int _level;
    private int _gold;

    public int PlayerType { get; set; }
    public int Hp { get; set; }
    public int Damage { get; set; }
    public int Critical { get; set; }
    public int Level { get; set; }
    public int Gold { get; set; }
}

[System.Serializable]
public class WeaponData
{
    private PlayerType _weaponType;
    private int _damage;
    private string _name;
    public int WeaponType { get; set; }
    public int Damage { get; set; }
    public int Name { get; set; }
}

[System.Serializable]
public class ArmorData
{
    private PlayerType _armorType;
    private int _defence;
    private string _name;

    public int ArmorType { get; set; }
    public int Defence { get; set; }
    public int Name { get; set; }
}