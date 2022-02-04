using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public GameObject item;

    private void Start()
    {
        StartCoroutine(SpawnItem());
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(0.05f);

        GameObject temp = Instantiate(item, transform.position, transform.rotation);
        temp.GetComponent<DragObject>().spawnPoint = gameObject.transform;
    }
}
