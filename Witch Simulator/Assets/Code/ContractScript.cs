using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractScript : MonoBehaviour
{
    
    [HideInInspector]
    public int whatSpawnUsed;
    [HideInInspector]
    public float contractTimer;
    [HideInInspector]
    public ContractManager contractManager;

    private bool activateTimer;

    public string contractName;
    public int contractPoints;
    public Slider contractTimeSlider;

    public float minTimer;
    public float maxTimer;
   
    private void Awake()
    {
        contractTimer = Random.Range(minTimer, maxTimer);
        contractTimeSlider.maxValue = contractTimer;
        StartCoroutine(startTimer());
    }

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

    //STOP TIMER WHEN DEACTIVATED
    public void DeleteContract()
    {
        contractManager.contractSpawns[whatSpawnUsed].GetComponent<ContractSpawnScript>().isSpawnFree = true;
        contractManager.AddFail();
        gameObject.SetActive(false);
        //Instantiate destroy effect of contract
    }
}