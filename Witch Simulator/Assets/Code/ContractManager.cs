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


    //contract timer
    public float minTimeBtwContracts;
    public float maxTimeBtwContracts;

    private float timeBetweenContracts;   
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
    public Canvas looseCanvas;

    //points display-text
    private TextMeshProUGUI pointsText;

    //difficulty and loop
    private int difficulty = 0;


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
     
    public void IncreaseDifficulty()
    {
        
    }

    public void RestartLoop()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ContractScript>())
            {
                child.gameObject.SetActive(false);
            }
        }

        failedContracts = 0;
        looseCanvas.enabled = false;
        dead = false;
        contractTimer = timer + timeBetweenContracts;

    }

    public void AddContract()
    {
        //temporary variables
        int randomTemp;
        string searchedName;
        ContractScript cScript;

        int i = 0;
        //nur neuen auftrag nehmen wenn durch alle spawns gelooped wurde und keienr frei ist       
        foreach (Transform isSpawnFree in contractSpawns)
        {
            if (contractSpawns[i].GetComponent<ContractSpawnScript>().isSpawnFree == true)
            {               
                randomTemp = Random.Range(0, 2);
                searchedName = contracts[randomTemp].name;
                Debug.Log(searchedName);
                Debug.Log("freier slot gefunden");

                foreach (Transform child in transform)
                {
                    if (child.GetComponent<ContractScript>())
                    {
                        Debug.Log("contract gefunden");
                        if (child.GetComponent<ContractScript>().contractName == searchedName)
                        {
                            Debug.Log("PASSENDEN gefunden");
                            child.gameObject.SetActive(true);
                            child.position = contractSpawns[i].position;
                            cScript = child.GetComponent<ContractScript>();
                            cScript.whatSpawnUsed = i;
                            //maybe delete later...
                            cScript.contractManager = gameObject.GetComponent<ContractManager>();
                        }
                    }
                }            
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
        if (dead == false)
        {   
            dead = true;
            //Death effect
            looseCanvas.enabled = true;
            Time.timeScale = 0;
        }
    }
}
