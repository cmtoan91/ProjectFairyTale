using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;

using UIObjects = System.Tuple<UnityEngine.GameObject, UnityEngine.GameObject>;
public class UIManager : GenericSingleton<UIManager>
{
    [Header("Canvas")]
    public GameObject CanvasPrefab;
    public Canvas TemporaryCanvas;
    [HideInInspector]
    public Canvas SceneCanvas;
    [Header("UI Stuff")]
    public bool LoadOnStart = true;
    public PFTGameUIType StartUI = PFTGameUIType.LogoIntro;
    public float StartFadeIn = 2;
    public PFTGameUIType BackToMenuUI;
    public PFTGameUIType PreviousUI = PFTGameUIType.DialogYesNo;
    public List<UIObjects> CurrentUIObjects = new List<UIObjects>();
    public GameObject CurrentDialogObject = null;    

    public string UIBagFolder = "ui_bags";

    //public UIBag MainBag;
    Dictionary<PFTGameUIType, SO_UIBag> _sources = new Dictionary<PFTGameUIType, SO_UIBag>();
    Coroutine mainco = null;


    protected override void Awake()
    {
        base.Awake();
        if(TemporaryCanvas != null)
            Destroy(TemporaryCanvas.gameObject);

        TemporaryCanvas = null;
        SO_UIBag[] uis = Resources.LoadAll<SO_UIBag>(UIBagFolder);
        foreach (SO_UIBag ui in uis)
        {
            _sources[ui.UI] = ui;
        }
        if (this.isActiveAndEnabled)
        {
            CreateCanvas();
        }
        //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void CloseAll(float fadeTime = 0.4f)
    {
        var uis = CurrentUIObjects.ToArray();
        foreach (var ui in uis)
        {
            GameObject g = ui.Item1;
            Close(null, g, fadeTime);
        }
        CurrentUIObjects.Clear();
    }
    //private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{
    //    if(LoadOnStart && this != null)
    //        OpenReplace(StartUI, StartFadeIn);
    //}

    void CreateCanvas()
    {
        if (SceneCanvas == null)
        {
            GameObject canvasObj = (GameObject)Instantiate(CanvasPrefab);
            SceneCanvas = canvasObj.GetComponent<Canvas>();
        }
    }

    public void Initialize()
    {
        if (LoadOnStart && this != null)
            OpenReplace(StartUI, StartFadeIn);
    }
    public T OpenPersistent<T>(PFTGameUIType ui, float fadeTime = 0.4f)
    {
        GameObject obj = OpenPersistent(ui, fadeTime);
        T component = obj.GetComponent<T>();
        return component;
    }
    public GameObject OpenPersistent(PFTGameUIType ui, float fadeTime = 0.4f)
    {
        GameObject opened = null;
        GameObject orig = _sources[ui].UIPrefab;
        if (orig != null)
        {
            CreateCanvas();
            GameObject newObj = (GameObject)Instantiate(orig);
            newObj.transform.SetParent(SceneCanvas.transform, false);
            opened = newObj;
            CanvasGroup cg = newObj.GetComponent<CanvasGroup>();
            if (cg != null)
                StartCoroutine(Fade(PFTUiFadeType.In, fadeTime, cg));
        }
        return opened;
    }

    public GameObject Open(PFTGameUIType ui, bool rememberSelection = true, float fadeTime = 0.4f)
    {
        GameObject opened = null;
        if (!_sources.ContainsKey(ui)) throw new Exception(ui + " bag is missing.");
        GameObject orig = _sources[ui].UIPrefab;
        if (orig != null)
        {
            CreateCanvas();
            GameObject newObj = (GameObject)Instantiate(orig);
            newObj.transform.SetParent(SceneCanvas.transform, false);
            opened = newObj;
            GameObject lastSelected = rememberSelection ? EventSystem.current.currentSelectedGameObject : null;
            var v = CurrentUIObjects.LastOrDefault();
            if(v!=null)
            {
                int idx = CurrentUIObjects.FindIndex(x => x == v);
                CurrentUIObjects[idx] = new UIObjects(v.Item1, lastSelected);
            }
            CurrentUIObjects.Add(new UIObjects(newObj, null));
            CanvasGroup cg = newObj.GetComponent<CanvasGroup>();
            if (cg != null)
                StartCoroutine(Fade(PFTUiFadeType.In, fadeTime, cg));
        }
        return opened;
    }
    public T Open<T>(PFTGameUIType ui, bool rememberSelection = true, float fadeTime = 0.4f)
    {
        GameObject obj = Open(ui, rememberSelection, fadeTime);
        T component = obj.GetComponent<T>();
        return component;
    }
    public T OpenReplace<T>(PFTGameUIType ui, float fadeTime = 0.4f)
    {
        GameObject obj = OpenReplace(ui, fadeTime);
        T component = obj.GetComponent<T>();
        return component;
    }
    public GameObject OpenReplace(PFTGameUIType ui, float fadeTime = 0.4f)
    {
        GameObject opened = null;
        GameObject orig = _sources[ui].UIPrefab;
        if (orig != null)
        {
            if (mainco != null)
                StopCoroutine(mainco);

            //if (CurrentUIObject!=null)
            //Close(CurrentUIObject, fadeTime);
            CreateCanvas();
            CloseAll(fadeTime);

            GameObject newObj = (GameObject)Instantiate(orig);
            newObj.transform.SetParent(SceneCanvas.transform, false);
            opened = newObj;
            CanvasGroup cg = newObj.GetComponent<CanvasGroup>();
            if (cg != null)
                mainco = StartCoroutine(Fade(PFTUiFadeType.In, fadeTime, cg));
            CurrentUIObjects.Add(new UIObjects(newObj, null));
        }
        return opened;
    }
    public bool HasSomethingOpened => CurrentUIObjects.Count > 0;
    public bool IsForeground(GameObject obj)
    {
        if (CurrentUIObjects.Count > 0)
        {
            var v = CurrentUIObjects.LastOrDefault();
            if(v!=null)
            {
                return v.Item1 == obj;
            }
        }
        return false;
    }
    void Close(Coroutine co, GameObject uiObj, float fadeTime = 0.4f)
    {
        if (uiObj != null)
        {
            if (co != null)
            {
                StopCoroutine(co);
            }

            var v = CurrentUIObjects.Where(x => x.Item1 == uiObj).FirstOrDefault();
            if (v != null)
            {
                CurrentUIObjects.Remove(v);
            }
            CanvasGroup cg = uiObj.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                StartCoroutine(Fade(PFTUiFadeType.Out, fadeTime, cg, ()=> Destroy(cg.gameObject)));
            }
        }
    }
    public void SelectLast()
    {
        var v = CurrentUIObjects.LastOrDefault();
        if(v!=null && v.Item2)
        {
            EventSystem.current.SetSelectedGameObject(v.Item2);
        }
    }
    public void Close(GameObject uiObj, float fadeTime = 0.4f)
    {
        if (uiObj != null)
        {
            Close(null, uiObj, fadeTime);
        }
    }

    public static IEnumerator Hide(GameObject uiObj, Action ondone = null, float fadeTime = 0.4f)
    {
        CanvasGroup cg = uiObj.GetComponent<CanvasGroup>();
        if (cg.alpha > 0.0f)
            yield return Fade(PFTUiFadeType.Out, fadeTime, cg, ondone);
        else 
            yield return null;
    }
    public static IEnumerator Show(GameObject uiObj, Action ondone = null, float fadeTime = 0.4f)
    {
        CanvasGroup cg = uiObj.GetComponent<CanvasGroup>();
        if (cg.alpha < 1.0f)
            yield return Fade(PFTUiFadeType.In, fadeTime, cg, ondone);
        else
            yield return null;
    }
    public static IEnumerator Fade(PFTUiFadeType fade, float fadeTime, CanvasGroup cg, Action ondone = null)
    {
        if (cg == null)
            yield break;

        float targetValue = 0;
        float t = 0;
        switch (fade)
        {
            case PFTUiFadeType.In:
                targetValue = 1.0f;
                cg.alpha = 0;
                //t = cg.alpha;
                break;
            case PFTUiFadeType.Out:
                targetValue = 0.0f;
                t = 1 - cg.alpha;
                //cg.alpha = 1;
                break;
        }

        float alpha = cg.alpha;
        for (; t < 1.0f; t += Time.unscaledDeltaTime / fadeTime)
        {
            if (cg == null)
                yield break;
            cg.alpha = Mathf.Lerp(alpha, targetValue, t);
            yield return new WaitForEndOfFrame();
        }

        if (cg != null)
            cg.alpha = targetValue;

        if (ondone != null) ondone();

    }

    public delegate void OnUIEventHandler(object sender, object args);
}
[System.Serializable]
public struct UISource
{
    public string Source;
    public PFTGameUIType UI;
}
