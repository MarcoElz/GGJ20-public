using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            FixInteraction interaction = other.GetComponent<FixInteraction>();
            
            if(interaction != null)
            {
                if (!interaction.IsBroken)
                {
                    if (interaction.transform.position.x > -5f && interaction.transform.position.x < 10f)
                    {
                        int random = Random.Range(0, 10);
                        if(random < 1)
                            interaction.Break();
                    }
                        
                }
            }
        }
    }

}
