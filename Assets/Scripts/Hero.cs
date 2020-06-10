using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Heroes/Hero")]
public class Hero : ScriptableObject
{
    public Sprite image;
    public Sprite cardImage;
    public int level;
    public List<Item> betterItemList;
    public int HeroGenericType;
    public List<int> HeroFightStyleList;
    public string description;
    

    [HideInInspector]
    public int gameLevel;
    [HideInInspector]
    public List<Item> currentItemList;
    [HideInInspector]
    public List<ItemPiece> currentItemPieceList;
    [HideInInspector]
    public float winRate;
    [HideInInspector]
    public GameObject gameObject;
}
