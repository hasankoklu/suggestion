using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour
{

    public List<Hero> heroList = new List<Hero>();
    public List<Item> itemList = new List<Item>();
    public List<ItemPiece> itemPieceList = new List<ItemPiece>();
    public List<HeroGenericType> heroGenericTypeList = new List<HeroGenericType>();
    public List<HeroFightStyle> heroFightStyleList = new List<HeroFightStyle>();

    private void Start()
    {

    }


    [Serializable]
    public class Hero
    {
        public string name;
        public int level;
        public List<int> betterItemIdList;
        public int HeroGenericType;
        public List<int> HeroFightStyleList;
        public string description;

    }

    [Serializable]
    public class Item
    {
        public string name;
        public List<int> requiredPieceList;
        public string description;
    }

    [Serializable]
    public class ItemPiece
    {
        public string name;
        public string description;
    }

    [Serializable]
    public class HeroGenericType
    {
        public string name;
        public string description;
    }

    [Serializable]
    public class HeroFightStyle
    {
        public string name;
        public string description;
    }
}
