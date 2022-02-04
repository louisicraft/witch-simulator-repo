using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractManager : MonoBehaviour
{

    [System.Serializable]
    public class Contract
    {
        //public int ingredientCount;
        public string name;
     
        public GameObject contractVisuals;

        public float minTimer;
        public float maxTimer;

        public int points;
    }

    public Contract[] contracts;


    public List<Transform> contractSpawns;
    [HideInInspector]
    public List<Transform> activeContractVisuals;

    public float minTimeBtwContracts;
    public float maxTimeBtwContracts;
    private float timeBetweenContracts;

    //contract timer
    private float timer;
    private float contractTimer;

    //new potion spawning
    public Transform potionSpawn;

    //contract fails
    private bool dead; 
    [HideInInspector]
    public int failedContracts = 0;
    private int failLimit = 4;
    public List<Image> failedCrosses;

    //points display-text
    private TextMeshProUGUI pointsText;

    void Start()
    {
        pointsText = GameObject.FindGameObjectWithTag("Points").GetComponent<TextMeshProUGUI>();
        pointsText.text = PlayerPrefs.GetInt("points").ToString() + "$";

        timeBetweenContracts = Random.Range(minTimeBtwContracts, maxTimeBtwContracts);
        contractTimer = timeBetweenContracts;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= contractTimer)
        {
            contractTimer = timer + timeBetweenContracts;
            AddContract();
        }                
    }
     
    public void AddContract()
    {
        
        //zufaellige zahl fuer contract auswaehlen
        Transform _temp;
        int temp;

        int i = 0;
        //nur neuen auftrag nehmen wenn durch alle spawns gelooped wurde und keienr frei ist       
        foreach (Transform isSpawnFree in contractSpawns)
        {
            if (contractSpawns[i].GetComponent<ContractSpawnScript>().isSpawnFree == true)
            {
                print("contract added");               
                temp = Random.Range(0, 2);

                _temp = (Instantiate(contracts[temp].contractVisuals, contractSpawns[i].position, Quaternion.identity, this.transform).transform);
                _temp.GetComponent<ContractScript>().contractName = contracts[temp].name;
                _temp.GetComponent<ContractScript>().whatSpawnUsed = i;
                _temp.GetComponent<ContractScript>().contractTimer = Random.Range(contracts[temp].minTimer, contracts[temp].maxTimer);
                _temp.GetComponent<ContractScript>().contractManager = gameObject.GetComponent<ContractManager>();
                _temp.GetComponent<ContractScript>().contractPoints = contracts[temp].points;

                activeContractVisuals.Add(_temp);

                contractSpawns[i].GetComponent<ContractSpawnScript>().isSpawnFree = false;
                i++;
                return;
            }
            else
            {
                print("Ein Kunde hat kein Platz gefunden...");
                i++;
            }
        }
    }

    public void finishContract(string potionIndex, GameObject potion)
    {
        foreach (Transform child in transform)
        {
            if(child.GetComponent<ContractScript>())
            {
                ContractScript contractScript = child.GetComponent<ContractScript>();
                if (contractScript.contractName == potionIndex)
                {
                    PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points") + contractScript.contractPoints);
                    pointsText.text = PlayerPrefs.GetInt("points").ToString() + "$";

                    contractSpawns[contractScript.whatSpawnUsed].GetComponent<ContractSpawnScript>().isSpawnFree = true;

                    potionSpawn.GetComponent<SpawnItems>().StartSpawn();

                    Destroy(child.gameObject);
                    Destroy(potion);                   
                    break;
                }
                else
                {
                    print("potion not needed");
                }
            }   
        }   
    }

    public void AddFail()
    {
        if(failedContracts < failedCrosses.Count)
        {
            failedCrosses[failedContracts].enabled = true;
        }
        failedContracts++;       
        if(failedContracts >= failLimit)
        {
            Die();
            
        }
    }

    public void Die()
    {
        //loose and fail the mission
        //get some kind of punishment or less bonus
        if (dead == false)
        {
            Debug.Log("ded");
         
            dead = true;
            //Death effect
            //looseCanvas.enabled = true;
            Time.timeScale = 0;
        }
    }

}
