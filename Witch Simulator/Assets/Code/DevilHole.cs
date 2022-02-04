using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilHole : MonoBehaviour
{


    [HideInInspector]
    public List<string> workingIngredient;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Ingredient>())
        {
            if (workingIngredient.Count <= 1)
            {
                int counter = workingIngredient.Count + 1;
                //devilHoleCounter.text = counter.ToString();
                workingIngredient.Add(collision.transform.GetComponent<Ingredient>().ingredientIndex);

                collision.transform.GetComponent<DragObject>().respawn();
                Destroy(collision.gameObject);
                //add floating icon above work station to show what is inside         

            }
        }

        /*else if (collision.transform.GetComponent<Potion>())
        {
            if (soupIndex != water)
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
        }*/
    }
}
