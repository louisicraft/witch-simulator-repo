using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {      
            other.transform.GetComponent<DragObject>().respawn();
            Destroy(other.gameObject);       
    }
}
