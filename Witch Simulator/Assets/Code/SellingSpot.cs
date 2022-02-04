using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingSpot : MonoBehaviour
{
    public ContractManager contractManager;

    private void OnTriggerEnter(Collider other)
    { 
        if (other.GetComponent<Potion>())
        {
            Potion potionScript;
            potionScript = other.GetComponent<Potion>();
            contractManager.finishContract(potionScript.potionIndex, other.gameObject);
        }
    }
}
