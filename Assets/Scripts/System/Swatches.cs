using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public struct MaterialSwatch
{
    public string key;
    public Material material;
}

[CreateAssetMenu(fileName = "SwatchManager", menuName = "Scriptable Objects/Swatches", order = 1)]
public class Swatches : ScriptableObject
{
    public List<MaterialSwatch> materialSwatches;

    public Material GetMaterial(string key)
    {
        return materialSwatches.Find(m => m.key.Equals(key)).material;
    }
}
