using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static Managers Instance
    {
        get { Init(); return s_instance; }
    }
    private InputManager _inputManager = new InputManager();
    private ResourceManager _resourceManager = new ResourceManager();
    private UIManager _uiManager = new UIManager();
    private GameManager _gameManager = new GameManager();
    public static InputManager InputManager { get { return Instance._inputManager; } }
    public static ResourceManager ResourceManager { get { return Instance._resourceManager; } }
    public static UIManager UIManager { get { return Instance._uiManager; } }
    public static GameManager GameManager { get { return Instance._gameManager; } }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        //_inputManager.OnUpdate();
    }

    private static void Init()
    {
        GameObject gameObject = GameObject.Find("Managers");
        if (gameObject == null)
        {
            gameObject = new GameObject("Managers");
            gameObject.AddComponent<Managers>();
        }
        DontDestroyOnLoad(gameObject);

        s_instance = gameObject.GetComponent<Managers>();
    }
}
