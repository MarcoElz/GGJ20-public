using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected CompletionBar completion = default;
    [SerializeField] protected bool onlyShowBarNearPlayer = false;
    public abstract bool Interact(PlayerController player);

    public abstract void OnPlayerIsNear(bool isNear);
}
