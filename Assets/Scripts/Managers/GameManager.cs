using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    public void Init()
    {
        Managers.ResourceManager.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if (count >= totalCount)
            {
                // ���ҽ� �ε尡 ��� �Ϸ��.
                SetGameData();
            }
        }
        );
        Managers.ResourceManager.InstantialteAllAsync("PreGameObject");
    }

    private void SetGameData()
    {
        Managers.DataManager.Init();
    }
}
