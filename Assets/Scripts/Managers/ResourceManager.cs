using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ResourceManager
{
    public Dictionary<string, UnityEngine.Object> resources = new Dictionary<string, UnityEngine.Object>();
    private Dictionary<string, AsyncOperationHandle> resourcesHandle = new Dictionary<string, AsyncOperationHandle>();

    
    public GameObject Instantiate(string key, Transform parent = null, bool instantiateInWorld = false)
    {
        GameObject go = Load<GameObject>(key);
        if (go == null)
        {
            Debug.Log("����");
            return null;
        }

        return UnityEngine.Object.Instantiate(go, parent, instantiateInWorld);
    }

    //�޸� ����
    public void Destroy(string key)
    {
        if (resourcesHandle.TryGetValue(key, out AsyncOperationHandle operationHandle) == false) return;
        Addressables.Release(operationHandle);
        resources.Remove(key);
        resourcesHandle.Remove(key);
    }

    //resources(��ųʸ�)���� �� ��ȯ
    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (!resources.TryGetValue(key, out UnityEngine.Object resource)) return null;
        return resource as T;
    }


    //���� �ε�
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }
        
        AsyncOperationHandle<T> asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += (AsyncOperationHandle<T> obj) => {
            resources.Add(key, obj.Result);
            resourcesHandle.Add(key, obj);
            callback?.Invoke(obj.Result);
        };
    }
    //���� �ε� && �ν��Ͻ�
    public void InstantiateAssetAsync(string key, Transform parent = null, bool instantiateInWorld = false) 
    {
        if (resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            Instantiate(key, parent, instantiateInWorld);
            return;
        }

        AsyncOperationHandle<GameObject> asyncOperation = Addressables.InstantiateAsync(key, parent, instantiateInWorld);
        asyncOperation.Completed += (AsyncOperationHandle<GameObject> obj) => {
            resources.Add(key, obj.Result);
            resourcesHandle.Add(key, obj);
        };
    }

    //�� �ε�
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += (AsyncOperationHandle<IList<IResourceLocation>> obj) => {
            int loadCount = 0;
            int totalCount = obj.Result.Count;
            foreach (IResourceLocation location in obj.Result)
            {
                LoadAsync<T>(location.PrimaryKey, obj => {
                    loadCount++;
                    callback?.Invoke(location.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

    //�� �ε� && �ν��Ͻ�
    public void InstantialteAllAsync(string label, bool instantiateInWorld = false)
    {
        AsyncOperationHandle<IList<IResourceLocation>> operation = Addressables.LoadResourceLocationsAsync(label, typeof(GameObject));
        operation.Completed += (AsyncOperationHandle<IList<IResourceLocation>> obj) => {
            foreach (IResourceLocation location in obj.Result)
            {
                InstantiateAssetAsync(location.PrimaryKey, null, instantiateInWorld);
            }
        };
    }
}
