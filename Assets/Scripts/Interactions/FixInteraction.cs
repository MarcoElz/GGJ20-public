using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixInteraction : Interactable
{

    [SerializeField] ItemType fixType;
    [SerializeField] GameObject requiredFixPrefab;

    private GameObject spawnPrefab;

    public bool IsBroken { get; private set; }



    public override bool Interact(PlayerController player)
    {
        Debug.Log("Try interact with: " + gameObject.name);
        if (!IsBroken)
            return false;

        Debug.Log("Try interact with: IS BROKEN " + gameObject.name);

        if (player.HoldItem != null && player.HoldItem.Type.Equals(fixType))
        {
            Debug.Log("Try interact with: PERFECT " + gameObject.name);

            player.ConsumeItem();
            FixIt();
            IsBroken = false;

            return true;
        }

        //Cant complete action
        return false;


    }

    public void Break()
    {
        spawnPrefab = Instantiate(requiredFixPrefab , this.transform.position + (Vector3.up * 0f), this.transform.rotation) as GameObject;
        IsBroken = true;
    }

    private void FixIt()
    {
        Destroy(spawnPrefab);
    }

    public void ForceFix()
    {
        FixIt();
        IsBroken = false;
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
