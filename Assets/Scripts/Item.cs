using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public List<int> requiredPieceList;
    public string description;

    [HideInInspector]
    public Hero selectedHero;
    [HideInInspector]
    public GameObject gameObject;
}
