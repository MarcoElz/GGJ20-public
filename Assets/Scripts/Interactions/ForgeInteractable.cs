using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeInteractable : Interactable
{
    [SerializeField] GameObject railPrefab;
    [SerializeField] float fundTime = 5f;

    bool isFunding;
    bool hasItem;

    public override bool Interact(PlayerController player)
    {
        if (!isFunding && !hasItem)
        {   //It can start process
            if (player.HoldItem != null && player.HoldItem.Type.Equals(ItemType.Ore))
            {
                player.ConsumeItem();
                isFunding = true;
                StartCoroutine(FundingRoutine());

                return true;
            }
        }
        else if (!isFunding && hasItem)
        {   //The process is finish, can take item

            if (player.HoldItem == null)
            {
                hasItem = false;

                //Complete
                GameObject rail = Instantiate(railPrefab) as GameObject;
                player.GiveItem(rail);
                completion.gameObject.SetActive(false);

                return true;
            }
        }
        else
        {   //The process is not finish, have to wait
            return false;
        }



        //Cant complete action
        return false;
    }

    IEnumerator FundingRoutine()
    {
        float count = fundTime;
        completion.gameObject.SetActive(true);
        while (count > 0)
        {
            count -= Time.deltaTime;
            completion.SetValue((float)count / (float)fundTime);
            yield return null;
        }

        isFunding = false;
        hasItem = true;
    }

    public override void OnPlayerIsNear(bool isNear)
    {
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
