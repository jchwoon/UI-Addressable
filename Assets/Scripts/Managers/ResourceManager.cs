using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
    private Dictionary<string, UnityEngine.Object> resources = new Dictionary<string, UnityEngine.Object>();
    private Dictionary<string, AsyncOperationHandle> resourcesHandle = new Dictionary<string, AsyncOperationHandle>();

    
    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject go = Load<GameObject>(key);
        if (go == null)
        {
            Debug.Log("실패");
            return null;
        }

        return UnityEngine.Object.Instantiate(go, parent);
    }

    //메모리 해제
    public void Destroy(string key)
    {
        //resourcesHandle에서 AsyncOperationHandle타입의 handle을 꺼내고 Release에 handle을 전달해줘서 메모리 해제
        if (resourcesHandle.TryGetValue(key, out AsyncOperationHandle operationHandle) == false) return;
        //key(address)로 주면 에러가 뜸
        Addressables.Release(operationHandle);
        resources.Remove(key);
    }

    //resources(딕셔너리)에서 값 반환
    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (!resources.TryGetValue(key, out UnityEngine.Object resource)) return null;
        return resource as T;
    }


    //단일 로드
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        string loadKey = key;
        if (key.Contains(".sprite")) loadKey = $"{key}[{key.Replace(".sprite", "")}]";

        if (key.Contains(".sprite"))
        {
            var asyncOperation = Addressables.LoadAssetAsync<Sprite>(loadKey);
            asyncOperation.Completed += obj => {
                resources.Add(key, obj.Result);
                callback?.Invoke(obj.Result as T);
            };
        }
        else
        {
            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
            asyncOperation.Completed +=  obj => {
                resources.Add(key, obj.Result);
                callback?.Invoke(obj.Result);
            };
        }
    }

    //라벨 로드
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += op => {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, obj => {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

    
}
