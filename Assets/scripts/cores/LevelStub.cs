using System;
using UnityEngine;

public class LevelStub<T> : MonoBehaviour where T : GenericSingleton<T>
{
    public SceneID SceneID = SceneID.Level01;
    public bool InGame = false;
    public bool LoadUIOnStart = false;
    public PFTGameUIType StartUI = PFTGameUIType.LogoIntro;
    public GameObject ManagerPrefab;
    T _sceneData = null;

    protected virtual void Awake()
    {
        if (GenericSingleton<T>.Instance == null)
        {
            GameObject obj = Instantiate(ManagerPrefab);
            OnFirstStart();
        }
        _sceneData = GenericSingleton<T>.Instance;
        UIManager.Instance.LoadOnStart = LoadUIOnStart;
        UIManager.Instance.StartUI = StartUI;
    }
    protected virtual void OnFirstStart() { }
    protected virtual void OnDestroy()
    {
        _sceneData.ShutDown();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _sceneData.Initialize(SceneID, InGame);
        _sceneData.StartGame();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}

public abstract class IGameManager : MonoBehaviour
{
    public static IGameManager _Instance { get; }
}
public enum SceneID
{
    Start,
    ReturnToStart,
    Special01,
    Special02,
    Special03,

    //1000 and up
    Level01 = 1000,
    Level02,
    Level03,
    Level04
}

[Serializable]
public class SceneInfo
{
    public string Title;
    public Sprite Background;
    public SceneID SceneID;
    [TextArea(2, 10)]
    public string Description;
    public string SceneFile;
    public PFTGameUIType LoadingScreen = PFTGameUIType.LoadScene;
}