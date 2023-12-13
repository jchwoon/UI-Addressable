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
            Debug.Log("����");
            return null;
        }

        return UnityEngine.Object.Instantiate(go, parent);
    }

    //�޸� ����
    public void Destroy(string key)
    {
        //resourcesHandle���� AsyncOperationHandleŸ���� handle�� ������ Release�� handle�� �������༭ �޸� ����
        if (resourcesHandle.TryGetValue(key, out AsyncOperationHandle operationHandle) == false) return;
        //key(address)�� �ָ� ������ ��
        Addressables.Release(operationHandle);
        resources.Remove(key);
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

    //�� �ε�
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
