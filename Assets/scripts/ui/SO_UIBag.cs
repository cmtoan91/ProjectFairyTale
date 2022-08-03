using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "uibag", menuName = "ScriptableObjects/UIBag", order = 1)]
public class SO_UIBag : ScriptableObject
{
    [Header("UI Type + Prefab Path")]
    public PFTGameUIType UI;
    public GameObject UIPrefab;
}
