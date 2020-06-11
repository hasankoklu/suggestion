using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Heroes/Hero")]
public class Hero : ScriptableObject
{
    public Sprite image;
    public Sprite cardImage;
    public int level;
    public HeroGenericType HeroGenericType;
    public List<HeroFightStyle> HeroFightStyleList;
    public string description;

    
    [HideInInspector]
    public List<Item> betterItemList;
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
