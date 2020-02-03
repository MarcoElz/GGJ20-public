using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineInteractable : Interactable
{
    [SerializeField] int startLifePoints = 5;
    [SerializeField] GameObject rockPrefab;

    private int actualLifePoints;

    private void Start()
    {
        actualLifePoints = startLifePoints;
    }

    public override bool Interact(PlayerController player)
    {
        if (player.HoldItem == null)
        {
            actualLifePoints--;
            completion.SetValue((float)actualLifePoints / (float)startLifePoints);

            if (actualLifePoints <= 0)
            {
                GameObject rock = Instantiate(rockPrefab) as GameObject;
                player.GiveItem(rock);

                actualLifePoints = startLifePoints;
                completion.SetValue(1);
            }

            return true;
        }

        //Cant complete action
        return false;


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
