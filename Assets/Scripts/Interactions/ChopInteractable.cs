using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopInteractable : Interactable
{
    [SerializeField] int startLifePoints = 5;
    [SerializeField] GameObject logPrefab;

    [SerializeField] GameObject explosionPrefab;

    [SerializeField] GameObject tree;
    [SerializeField] Animator cutTree;

    [SerializeField] float timeToSpawn = 15f;

    private int actualLifePoints;

    private bool interactable;

    private void Start()
    {
        actualLifePoints = startLifePoints;
        interactable = true;
    }

    public override bool Interact(PlayerController player)
    {
        if (!interactable)
            return false;

        if (player.HoldItem == null)
        {
            actualLifePoints--;
            completion.SetValue((float)actualLifePoints / (float)startLifePoints);

            if (actualLifePoints <= 0)
            {
                //Spawn Particles
                GameObject g = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity) as GameObject;
                Destroy(g, 2f);
                tree.transform.localScale = Vector3.zero;
                tree.SetActive(false);
                cutTree.gameObject.SetActive(true);
                cutTree.transform.localScale = Vector3.one;

                //Start Tree Timer
                StartCoroutine(TreeTimer());

                GameObject log = Instantiate(logPrefab) as GameObject;
                player.GiveItem(log);

                interactable = false;
            }

            return true;
        }

        //Cant complete action
        return false;

        
    }

    IEnumerator TreeTimer()
    {
        yield return new WaitForSeconds(timeToSpawn);

        cutTree.SetTrigger("Out");
        yield return new WaitForSeconds(2f);
        cutTree.gameObject.SetActive(false);
        tree.SetActive(true);
        interactable = true;
        actualLifePoints = startLifePoints;
        completion.SetValue(1f);
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
