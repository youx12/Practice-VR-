using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialEmission
{
    private static MaterialEmission instance;

    private MaterialEmission() { }
    public static MaterialEmission Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new MaterialEmission();
            }
            return instance;
        }
    }

    public void OpenEmission(GameObject obj)
    {
        if (!obj) return;

        Material[] mas = obj.GetComponent<MeshRenderer>().materials;     
        foreach(var item in mas)
        {
            item.EnableKeyword("_EMISSION");
            item.SetColor("_EmissionColor",Color.blue);
        }
    }
    public void CloseEmission(GameObject obj)
    {
        if (!obj) return;

        Material[] mas = obj.GetComponent<MeshRenderer>().materials;
        foreach (var item in mas)
        {
            item.DisableKeyword("_EMISSION");
        }
    }
}
