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
                // 리소스 로드가 모두 완료됨.
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
