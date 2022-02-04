using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractScript : MonoBehaviour
{
    public string contractName;
    public int whatSpawnUsed;
    public float contractTimer;
    public int contractPoints;

    public ContractManager contractManager;

    private bool activateTimer;

    public Slider contractTimeSlider;

    private void Start()
    {
        contractTimeSlider.maxValue = contractTimer;
        StartCoroutine(startTimer());
    }

    //spaeter noch den Timer hier einbauen

    private IEnumerator startTimer()
    {
        yield return new WaitForSeconds(0.25f);
        activateTimer = true;
    }

    public void Update()
    {
        if (activateTimer)
        {        
            contractTimer -= Time.deltaTime;
            contractTimeSlider.value = contractTimer;
        }
        if (contractTimer <= 0)
        {
            DeleteContract();
        }
        
    }
    public void DeleteContract()
    {
        contractManager.contractSpawns[whatSpawnUsed].GetComponent<ContractSpawnScript>().isSpawnFree = true;
        contractManager.AddFail();  
        Destroy(gameObject);
        //Instantiate destroz effect of contract
    }
}