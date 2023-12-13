using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        Managers.ResourceManager.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if (count >= totalCount)
            {
                // 리소스 로드가 모두 완료됨.
                SetGameObject(key);
            }
        }
        );
    }

    private void SetGameObject(string key)
    {
        Managers.ResourceManager.Instantiate(key);
    }
}
