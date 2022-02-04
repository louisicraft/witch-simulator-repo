using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingPot : MonoBehaviour
{
    public TextMeshProUGUI potCounter;

    private float deleteTimer;
    public float deleteCooldown = 3f;
    private bool deleted = false;
    public Slider potSlider;


    [System.Serializable]
    public class Recepie
    {
        //public int ingredientCount;
        public string name;

        public List<string> Ingredients;
        public Material material;
    }

    public Color waterColor;

    public Recepie[] recepies;

    private Material soupRenderer;

    private string water = "water";

    public string soupIndex = "water";

    [HideInInspector]
    public List<string> potIngredient;

    private void Start()
    {
        potSlider.minValue = 0f;
        deleteTimer = 0f;
        soupRenderer = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        potCounter.text = potIngredient.Count.ToString();
    }

    void OnMouseDrag()
    {
        potSlider.maxValue = deleteCooldown;
        if (deleted == false)
        {
            potSlider.gameObject.SetActive(true);
            deleteTimer += Time.deltaTime;
            //slider soll rueckwerts gehen
            potSlider.value = deleteCooldown - deleteTimer;

            if (deleteTimer > deleteCooldown)
            {
                ClearPot();
                deleted = true;
            }
        }       
    }

    private void OnMouseUp()
    {
        potSlider.gameObject.SetActive(false);
        potSlider.value = deleteCooldown;
        deleteTimer = 0;       
        deleted = false;
    }

    private void CheckRecepie()
    {
        for (int i = 0; i < recepies.Length; i++)
        {
            if(CompareLists(potIngredient, recepies[i].Ingredients))
            {
                soupIndex = recepies[i].name;
                soupRenderer.color = recepies[i].material.GetColor("sideColor");
                return;
            }
            else
            {
                soupIndex = water;
            }        
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Ingredient>())
        {
            if(potIngredient.Count <= 1)
            {
                int counter = potIngredient.Count + 1;
                potCounter.text = counter.ToString();
                potIngredient.Add(collision.transform.GetComponent<Ingredient>().ingredientIndex);

                collision.transform.GetComponent<DragObject>().respawn();
                Destroy(collision.gameObject);
                CheckRecepie();

                //add floating icon above poot to show what is inside         

            }
        }       
                
        else if (collision.transform.GetComponent<Potion>())
        {       
            if(soupIndex != water)
            {   
                for (int i = 0; i < recepies.Length; i++)
                {
                    if (recepies[i].name == soupIndex)
                    {
                        collision.transform.GetComponent<Potion>().SetIndex(soupIndex, recepies[i].material);
                        ClearPot();
                        return;
                    }
                    else
                    {
                        print("kein rezept gefunden");
                    }
                }              
            }
        }
    }

    private void ClearPot()
    {

        potCounter.text = "0";
        soupRenderer.color = waterColor;
        potIngredient.Clear();
        soupIndex = water;
    }

    public static bool CompareLists<T>(List<T> potIngredient, List<T> recepieIngredients)
    {
        if (potIngredient == null || recepieIngredients == null || potIngredient.Count != recepieIngredients.Count)
            return false;
        if (potIngredient.Count == 0)
            return true;
        Dictionary<T, int> lookUp = new Dictionary<T, int>();
        // create index for the first list
        for (int i = 0; i < potIngredient.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(potIngredient[i], out count))
            {
                lookUp.Add(potIngredient[i], 1);
                continue;
            }
            lookUp[potIngredient[i]] = count + 1;
        }
        for (int i = 0; i < recepieIngredients.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(recepieIngredients[i], out count))
            {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(recepieIngredients[i]);
            else
                lookUp[recepieIngredients[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }
}
