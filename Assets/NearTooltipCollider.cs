using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearTooltipCollider : MonoBehaviour
{

    public Interactable interactable;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactable.OnPlayerIsNear(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactable.OnPlayerIsNear(false);
        }
    }
}
