using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { Log, Wood, Ore, Rail }

public class Item : MonoBehaviour
{
    [SerializeField] ItemType type;

    public ItemType Type { get { return type; } private set { } }
   
}
