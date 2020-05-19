using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentGenericType;

    public List<Hero> heroList = new List<Hero>();
    public List<Item> itemList = new List<Item>();
    public List<ItemPiece> itemPieceList = new List<ItemPiece>();
    public List<HeroGenericType> heroGenericTypeList = new List<HeroGenericType>();
    public List<HeroFightStyle> heroFightStyleList = new List<HeroFightStyle>();

    public List<Hero> myHeroList = new List<Hero>();
    public List<Item> myItemList = new List<Item>();
    public List<ItemPiece> myItemPieceList = new List<ItemPiece>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    
    void AddHeroToMyTeamList(int heroIndex)
    {
        myHeroList.Add(heroList[heroIndex]);
        CanvasManager.instance.SetMyHeroList();
    }

    void RemoveHeroToMyTeamList(Hero myHero)
    {
        myHeroList.Remove(myHero);
        CanvasManager.instance.SetMyHeroList();
    }
         
    #region Classes

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

    #endregion

}