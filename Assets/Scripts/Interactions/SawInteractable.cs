using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SawInteractable : Interactable
{
    [SerializeField] GameObject woodPrefab;
    [SerializeField] Transform arrowsParent;

    public void SendArrowKey(string key)
    {
        int num = -1;
        switch (key)
        {
            case "up":
                num = 0;
                break;
            case "down":
                num = 1;
                break;
            case "right":
                num = 2;
                break;
            case "left":
                num = 3;
                break;
        }

        if(num == combination[actualIndex])
        {
            //Paint ui arrow
            arrowsParent.GetChild(actualIndex).GetComponent<Image>().color = Color.green;
            actualIndex++;

            if (actualIndex >= combination.Length)
            {
                //Complete
                actualPlayer.FinishArrowsMechanic();
                GameObject wood = Instantiate(woodPrefab) as GameObject;
                actualPlayer.GiveItem(wood);

                arrowsParent.gameObject.SetActive(false);
            }
        }
        else
        {
            //Error, so reset
            combination = new int[3];
            for (int i = 0; i < combination.Length; i++)
            {
                combination[i] = Random.Range(0, 4);
                arrowsParent.GetChild(i).GetComponent<Image>().color = Color.white;

                switch (combination[i])
                {
                    case 0:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 90f);
                        break;
                    case 1:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 270f);
                        break;
                    case 2:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 0f);
                        break;
                    case 3:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 180f);
                        break;
                }
            }
            actualIndex = 0;
        }


    }
    int[] combination;
    int actualIndex;
    PlayerController actualPlayer;

    public override bool Interact(PlayerController player)
    {
        if (player.HoldItem != null && player.HoldItem.Type.Equals(ItemType.Log))
        {
            //Start arrows mechanic 
            combination = new int[3];
            for (int i = 0; i < combination.Length; i++)
            {
                combination[i] = Random.Range(0, 4);
                arrowsParent.GetChild(i).GetComponent<Image>().color = Color.white;

                switch (combination[i])
                {
                    case 0:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 90f);
                        break;
                    case 1:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 270f);
                        break;
                    case 2:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 0f);
                        break;
                    case 3:
                        arrowsParent.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, 180f);
                        break;
                }
            }
            arrowsParent.gameObject.SetActive(true);

            //Set player
            actualIndex = 0;
            actualPlayer = player;
            player.SetPlayerOnArrowsMechanic(this);
            player.ConsumeItem();

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
