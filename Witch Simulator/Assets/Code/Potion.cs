using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public string potionIndex;

    public void SetIndex(string index, Material material)
    {
        potionIndex = index;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
    }
}
