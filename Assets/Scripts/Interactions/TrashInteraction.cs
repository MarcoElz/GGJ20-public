using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashInteraction : Interactable
{


    public override bool Interact(PlayerController player)
    {
        if (player.HoldItem != null)
        {
            player.ConsumeItem();
            return true;
        }

        //Cant complete action
        return false;


    }


    public override void OnPlayerIsNear(bool isNear)
    {
        if (completion == null)
            return;

        if (onlyShowBarNearPlayer)
        {
            if (isNear)
            {
                completion.gameObject.SetActive(true);
            }
            else
            {
                completion.gameObject.SetActive(false);
            }

        }
    }
}
