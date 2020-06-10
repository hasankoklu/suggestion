using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Piece", menuName = "Items/Item Piece")]
public class ItemPiece : ScriptableObject
{
    public Sprite image;
    public string description;

    [HideInInspector]
    public Hero selectedHero;
    [HideInInspector]
    public GameObject gameObject;
}
