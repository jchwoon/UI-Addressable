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
        if (resourcesHandle.TryGetValue(key, out AsyncOperationHandle operationHandle) == false) return;
        Addressables.Release(operationHandle);
        resources.Remove(key);
        resourcesHandle.Remove(key);
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

        //if (key.Contains(".sprite")) loadKey = $"{key}[{key.Replace(".sprite", "")}]";

        //if (key.Contains(".sprite"))
        //{
        //    AsyncOperationHandle<Sprite> asyncOperation = Addressables.LoadAssetAsync<Sprite>(loadKey);
        //    asyncOperation.Completed += (AsyncOperationHandle<Sprite> obj) => {
        //        resources.Add(key, obj.Result);
        //        resourcesHandle.Add(key, obj);
        //        callback?.Invoke(obj.Result as T);
        //    };
        //}

        AsyncOperationHandle<T> asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += (AsyncOperationHandle<T> obj) => {
            resources.Add(key, obj.Result);
            resourcesHandle.Add(key, obj);
            callback?.Invoke(obj.Result);
        };

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
