using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableInteraction : Interactable
{
    [SerializeField] Transform itemHolder;

    bool hasItem;
    GameObject storedItem;

    public override bool Interact(PlayerController player)
    {
        if (storedItem == null)
        {   //It can store an item
            if (player.HoldItem != null)
            {
                StoreItem(player.RemoveItem());
                return true;
            }
        }
        else
        {   //Item can be taken

            if (player.HoldItem == null)
            {
                //Complete
                player.GiveItem(storedItem);

                storedItem = null;

                return true;
            }
        }


        //Cant complete action
        return false;
    }


    public void StoreItem(GameObject itemObject)
    {
        if (itemObject)
        {
            itemObject.transform.position = itemHolder.position;
            itemObject.transform.SetParent(itemHolder);
            storedItem = itemObject;

        }

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
