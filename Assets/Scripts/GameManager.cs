﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region Variablees

    public static GameManager instance;

    public int currentGenericType;

    public List<Hero> heroList = new List<Hero>();
    public List<Item> itemList = new List<Item>();
    public List<ItemPiece> itemPieceList = new List<ItemPiece>();
    public List<HeroGenericType> heroGenericTypeList = new List<HeroGenericType>();
    public List<HeroFightStyle> heroFightStyleList = new List<HeroFightStyle>();

    public List<BestHeroTeam> bestHeroTeamList = new List<BestHeroTeam>();
    public List<Hero> myHeroList = new List<Hero>();
    public List<Item> myComplateItemList = new List<Item>();
    public List<ItemPiece> myPieceItemList = new List<ItemPiece>();
    public List<Hero> suggestionHeroList = new List<Hero>();


    public List<BestHeroTeam> theBestTeamList;

    public int selectedHeroIndex = -1;

    #endregion


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

    private void Start()
    {

        //ListAllHeroTeam();
        //for (int i = 0; i < heroList.Count; i++)
        //{
        //    Debug.Log(i + " : " + heroList[i].name);
        //}
    }

    public void AddHeroToMyHeroList(Hero hero)
    {
        if (myHeroList.Count < 9)
        {
            Hero newhero = new Hero();
            newhero.name = hero.name;
            newhero.image = hero.image;
            newhero.currentItemList = hero.currentItemList;
            newhero.currentItemPieceList = hero.currentItemPieceList;
            newhero.betterItemIdList = hero.betterItemIdList;
            newhero.removeIndex = myHeroList.Count;
            newhero.HeroFightStyleList = hero.HeroFightStyleList;
            newhero.HeroGenericType = hero.HeroGenericType;
            newhero.cardImage = hero.cardImage;
            myHeroList.Add(newhero);
            myHeroList = myHeroList.OrderBy(x => x.HeroGenericType).ToList();

            CanvasManager.instance.SetMyHeroList();
            //if (CanvasManager.instance.suggestionRect.activeSelf)
            //    MakeSuggestionforSelectedHero();
            TheBestHeroTeamSuggestion();

            if (CanvasManager.instance.extraItemSuggestionRect.activeSelf)
                ItemSuggestButtonOnClick();
        }

        //if (!CanvasManager.instance.suggestionRect.activeSelf)
        //    CanvasManager.instance.suggestionRect.SetActive(true);

        //if (!CanvasManager.instance.suggestionRect.activeSelf)
        //    CanvasManager.instance.suggestionRect.SetActive(true);
    }

    public void SetHeroItems(int index)
    {

        Hero myHero;


        myHero = myHeroList[index];

        for (int i = 0; i < CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).childCount; i++)
        {
            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(i).GetComponent<Image>().sprite = null;
            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(i).GetChild(0).GetComponent<Text>().text = "+";

            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(i).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        }


        int counter = 0;
        foreach (ItemPiece itemPiece in myHero.currentItemPieceList)
        {
            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(counter).GetComponent<Image>().sprite = itemPiece.image;

            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(counter).GetChild(0).GetComponent<Text>().text = "";

            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(counter).GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveItemPiece(itemPiece));

            counter++;
        }
        foreach (Item item in myHero.currentItemList)
        {
            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(counter).GetComponent<Image>().sprite = item.image;

            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(counter).GetChild(0).GetComponent<Text>().text = "";

            CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(3).GetChild(counter).GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveComplateItem(item));

            counter++;
        }

        #region ItemSuggest

        for (int k = 0; k < CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).childCount; k++)
        {
            int i = 0;
            foreach (int betterItemId in myHeroList[index].betterItemIdList)
            {
                if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 1)
                {
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = index;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyItem(item));

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().color =
                        new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.r,
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.g,
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.b, 1f);

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color = Color.white;

                    ItemPiece itemPiece = new ItemPiece();
                    itemPiece.name = itemPieceList[itemList[betterItemId].requiredPieceList[0]].name;
                    itemPiece.description = itemPieceList[itemList[betterItemId].requiredPieceList[0]].description;
                    itemPiece.image = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    itemPiece.selectedHeroIndex = index;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                    itemPiece = new ItemPiece();
                    itemPiece.name = itemPieceList[itemList[betterItemId].requiredPieceList[1]].name;
                    itemPiece.description = itemPieceList[itemList[betterItemId].requiredPieceList[1]].description;
                    itemPiece.image = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;
                    itemPiece.selectedHeroIndex = index;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).gameObject.SetActive(true);
                    i++;

                }
                else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name != itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 0 && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() > 0)
                {
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = index;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyItem(item));

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().color =
                        new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.r,
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.g,
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.b, 1f);

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color = Color.white;

                    ItemPiece itemPiece = new ItemPiece();
                    itemPiece.name = itemPieceList[itemList[betterItemId].requiredPieceList[0]].name;
                    itemPiece.description = itemPieceList[itemList[betterItemId].requiredPieceList[0]].description;
                    itemPiece.image = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    itemPiece.selectedHeroIndex = index;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                    itemPiece = new ItemPiece();
                    itemPiece.name = itemPieceList[itemList[betterItemId].requiredPieceList[1]].name;
                    itemPiece.description = itemPieceList[itemList[betterItemId].requiredPieceList[1]].description;
                    itemPiece.image = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;
                    itemPiece.selectedHeroIndex = index;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).gameObject.SetActive(true);


                    i++;
                }
                else
                {
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.r,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.g,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetComponent<Image>().color.b, 0.65f);


                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    #region Color
                    if (myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() == 0)
                    {
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color.r,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.b, 0.65f);

                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                    }
                    else
                    {
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color.r,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color.g,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().color.b, 1f);

                        ItemPiece itemPiece = new ItemPiece();
                        itemPiece.name = itemPieceList[itemList[betterItemId].requiredPieceList[0]].name;
                        itemPiece.description = itemPieceList[itemList[betterItemId].requiredPieceList[0]].description;
                        itemPiece.image = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                        itemPiece.selectedHeroIndex = index;

                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                    }

                    if (myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() == 0)
                    {
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.b, 0.65f);

                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                    }
                    else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() == 1)
                    {
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.b, 0.65f);

                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                    }
                    else
                    {
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().color.b, 1f);

                        ItemPiece itemPiece = new ItemPiece();
                        itemPiece.name = itemPieceList[itemList[betterItemId].requiredPieceList[0]].name;
                        itemPiece.description = itemPieceList[itemList[betterItemId].requiredPieceList[0]].description;
                        itemPiece.image = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                        itemPiece.selectedHeroIndex = index;

                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                        CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                    }

                    #endregion

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;

                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;




                    CanvasManager.instance.myHeroMenuRect.transform.GetChild(index).GetChild(2).GetChild(i).gameObject.SetActive(true);
                    i++;
                }
            }

        }
        #endregion
    }

    List<Item> itemsuggestionList = new List<Item>();
    public void ItemSuggestButtonOnClick()
    {
        CanvasManager.instance.heroTeamSuggestionRect.SetActive(false);
        CanvasManager.instance.extraItemSuggestionRect.SetActive(true);
        CanvasManager.instance.suggestionRect.SetActive(false);


        for (int f = 0; f < CanvasManager.instance.extraItemSuggestionRect.transform.childCount; f++)
        {
            CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).GetComponent<Image>().sprite = null;
            CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).gameObject.SetActive(false);
            for (int s = 0; s < CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).childCount; s++)
            {
                CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).GetChild(s).GetComponent<Image>().sprite = null;
            }
        }

        #region ItemSuggest

        int i = 0;
        int heroIndex = 0;
        foreach (Hero myHero in myHeroList)
        {
            if (i > 8)
                return;

            if (myHero.currentItemList.Count > 2)
                return;

            foreach (int betterItemId in myHero.betterItemIdList)
            {


                if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 1)
                {
                    if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 4) //look here
                    {
                        return;
                    }

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = heroIndex;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = myHero.image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;

                }
                else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name != itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 0 && myPieceItemList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() > 0)
                {
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = heroIndex;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = myHero.image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;
                }
            }

            heroIndex++;
        }


        #endregion

    }

    public void SuggestHero()
    {
        CanvasManager.instance.heroTeamSuggestionRect.SetActive(true);
        CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(0).gameObject.SetActive(true);
        CanvasManager.instance.extraItemSuggestionRect.SetActive(false);
        CanvasManager.instance.suggestionRect.SetActive(false);


        for (int i = 0; i < CanvasManager.instance.heroTeamSuggestionRect.transform.childCount; i++)
        {
            for (int j = 0; j < CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).childCount; j++)
            {
                CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(j).GetComponent<Image>().gameObject.SetActive(false);
            }
        }

        CanvasManager.instance.teamBuffText.text = theBestTeamList[0].buffName;

        for (int i = 0; i < theBestTeamList.Count; i++)
        {
            for (int j = 0; j < theBestTeamList[i].heroList.Count; j++)
            {
                if (i < CanvasManager.instance.heroTeamSuggestionRect.transform.childCount)
                {
                    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(j).GetComponent<Image>().sprite = theBestTeamList[i].heroList[j].image;
                    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                }

            }
        }
    }

    public void TakeItem(Item item)
    {
        item.selectedHeroIndex = selectedHeroIndex;

        if (item.selectedHeroIndex == -1)
        {
            myComplateItemList.Add(item);
        }
        else if (myHeroList[item.selectedHeroIndex].currentItemList.Count + myHeroList[item.selectedHeroIndex].currentItemPieceList.Count < 3)
        {
            myHeroList[item.selectedHeroIndex].currentItemList.Add(item);
        }

        selectedHeroIndex = -1;
        CanvasManager.instance.addItemRect.SetActive(false);
        CanvasManager.instance.RefreshListes();
    }

    public void TakePieceItem(ItemPiece itemPiece)
    {
        itemPiece.selectedHeroIndex = selectedHeroIndex;

        if (itemPiece.selectedHeroIndex == -1)
        {
            myPieceItemList.Add(itemPiece);
        }
        else if (myHeroList[itemPiece.selectedHeroIndex].currentItemList.Count + myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Count < 3)
        {
            myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Add(itemPiece);
        }


        selectedHeroIndex = -1;
        CanvasManager.instance.addItemRect.SetActive(false);
        CanvasManager.instance.RefreshListes();
    }

    public void BuyItem(Item item)
    {
        if (myHeroList[item.selectedHeroIndex].currentItemList.Count + myHeroList[item.selectedHeroIndex].currentItemPieceList.Count > 2)
            return;

        if (myComplateItemList.Where(x => x.name == item.name).Count() > 0)
        {
            myComplateItemList.Remove(item);
        }
        else
        {

            foreach (int itemId in item.requiredPieceList)
            {
                myPieceItemList.Remove(myPieceItemList.Where(x => x.name == itemPieceList[itemId].name).FirstOrDefault());
            }
        }
        myHeroList[item.selectedHeroIndex].currentItemList.Add(item);

        CanvasManager.instance.RefreshListes();
    }

    public void BuyPieceItem(ItemPiece itemPiece)
    {

        if (myHeroList[itemPiece.selectedHeroIndex].currentItemList.Count + myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Count > 2)
            return;

        myPieceItemList.Remove(myPieceItemList.Where(x => x.name == itemPiece.name).FirstOrDefault());

        myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Add(itemPiece);

        CanvasManager.instance.RefreshListes();
    }


    void RemoveItemPiece(ItemPiece itemPiece)
    {
        myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Remove(itemPiece);
        CanvasManager.instance.RefreshListes();
    }

    void RemoveComplateItem(Item item)
    {
        myHeroList[item.selectedHeroIndex].currentItemList.Remove(item);
        CanvasManager.instance.RefreshListes();
    }

    List<Hero> tempHeroList = new List<Hero>();

    public void ArrangeSuggestionHeroList(List<Hero> heroes)
    {
        heroes.LastOrDefault().currentBuffCount = 0;
        heroes.LastOrDefault().currentBuffName = "";
        //CanvasManager.instance.teamBuffText.text = "";
        //string heronames = "";
        //foreach (Hero hero in heroes)
        //{
        //    heronames += " : \n" + hero.name;
        //    tempBestHeroTeam.heroList.Add(hero);
        //}


        for (int i = 0; i < heroGenericTypeList.Count; i++)
        {
            if (heroes.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent >= 0)
            {
                heroes.LastOrDefault().currentBuffCount += (int)(heroes.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent);

                heroes.LastOrDefault().currentBuffName += (int)(heroes.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent) + "x " + heroGenericTypeList[i].name + " / ";

                //CanvasManager.instance.teamBuffText.text += "\n" + (int)(tempBestHeroTeam.heroList.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent) + "x " + heroFightStyleList[i].name;
            }
        }

        for (int i = 0; i < heroFightStyleList.Count; i++)
        {

            if (heroes.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent >= 0)
            {
                heroes.LastOrDefault().currentBuffCount += (int)(heroes.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent);

                heroes.LastOrDefault().currentBuffName += (int)(heroes.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent) + "x " + heroFightStyleList[i].name + " / ";

                //CanvasManager.instance.teamBuffText.text += "\n" + (int)(tempBestHeroTeam.heroList.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent) + "x " + heroFightStyleList[i].name;
            }
        }
        Debug.Log(heroes.LastOrDefault().currentBuffCount);
        suggestionHeroList.Add(heroes.LastOrDefault());
        suggestionHeroList = suggestionHeroList.OrderByDescending(x => x.currentBuffCount).ThenBy(x => x.level).ToList();
    }

    public int currentBuffCount;
    public List<BestHeroTeam> TheBestHeroes;
    public void TheBestHeroTeamSuggestion()
    {

        suggestionHeroList.Clear();

        foreach (Hero hero in heroList)
        {
            tempHeroList = new List<Hero>();

            foreach (Hero myhero in myHeroList)
            {
                tempHeroList.Add(myhero);
            }

            if (tempHeroList.Where(x => x.name == hero.name).Count() == 0)
            {
                tempHeroList.Add(hero);
                ArrangeSuggestionHeroList(tempHeroList);
            }
        }
        CanvasManager.instance.RefreshListes();
        //SuggestHero();

    }

    public void Save_TheBestHeroes(List<Hero> TheBestHeroes)
    {
        string path = Application.persistentDataPath + "/TheBestHeroes.txt";
        if (!string.IsNullOrEmpty(path))
        {
            var jsonString = JsonConvert.SerializeObject(TheBestHeroes, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            System.IO.File.WriteAllText(path, jsonString);
            Debug.Log("TheBestHeroes Successfully Saved.");
        }
    }

    public IEnumerator Read_TheBestHeroes()
    {
        //if (Application.internetReachability != NetworkReachability.NotReachable)
        //{
        //    UnityWebRequest uwr = UnityWebRequest.Get("http://cebur.fun/Suggestion" + "/TheBestHeroes.txt");
        //    yield return uwr.SendWebRequest();
        //    if (uwr.isNetworkError)
        //    {
        //        Debug.Log("Error");
        //    }
        //    else
        //    {
        //        var Result = JsonConvert.DeserializeObject<List<Hero>>(uwr.downloadHandler.text, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });
        //        TheBestHeroes = new List<Hero>();
        //        TheBestHeroes = Result;
        //        ExtendedPlayerPrefs.SetBool("TheBestHeroes", true);
        //        ExtendedPlayerPrefs.Flush();
        //        try
        //        {
        //            System.IO.File.WriteAllText(Application.persistentDataPath + "/TheBestHeroes.txt", uwr.downloadHandler.text);
        //        }
        //        catch (System.Exception e)
        //        {
        //            Debug.Log(e.Message);
        //            throw;
        //        }
        //    }
        //}
        //else if (ExtendedPlayerPrefs.GetBool("TheBestHeroes"))
        //{
        string path = Application.persistentDataPath + "/TheBestHeroes.txt";
        if (!string.IsNullOrEmpty(path))
        {
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Debug.Log("TheBestHeroes Load Error");
            }
            else
            {
                var Result = JsonConvert.DeserializeObject<List<BestHeroTeam>>(uwr.downloadHandler.text, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });
                TheBestHeroes = new List<BestHeroTeam>();
                TheBestHeroes = Result;
                Debug.Log("TheBestHeroes Successfully Loaded.");
            }
        }
        //}
    }

    #region Classes

    [Serializable]
    public class Hero
    {
        public string name;
        public Sprite image;
        public Sprite cardImage;
        public int level;
        public int gameLevel;
        public List<int> betterItemIdList;
        public int HeroGenericType;
        public List<int> HeroFightStyleList;
        public int removeIndex;
        public List<Item> currentItemList;
        public List<ItemPiece> currentItemPieceList;
        public int currentBuffCount;
        public string currentBuffName;
        public string description;
    }

    [Serializable]
    public class Item
    {
        public string name;
        public Sprite image;
        public int selectedHeroIndex;
        public int Type;
        public List<int> requiredPieceList;
        public string description;
    }

    [Serializable]
    public class ItemPiece
    {
        public string name;
        public Sprite image;
        public int selectedHeroIndex;
        public string description;
    }

    [Serializable]
    public class HeroGenericType
    {
        public string name;
        public Sprite image;
        public int exponent;
        public string description;
    }

    [Serializable]
    public class HeroFightStyle
    {
        public string name;
        public Sprite image;
        public int exponent;
        public string description;
    }

    [Serializable]
    public class BestHeroTeam
    {
        public List<Hero> heroList;
        public int winCount;
        public int loseCount;
        public string buffName;
        public int buffCount;
    }

    #endregion

}



/*
 
    public void MakeSuggestionforSelectedHero()
    {
        Hero myHero;

        #region ClearImages

        for (int k = 1; k < CanvasManager.instance.suggestionRect.transform.childCount; k++)
        {
            CanvasManager.instance.suggestionRect.transform.GetChild(k).gameObject.SetActive(false);
            for (int j = 0; j < CanvasManager.instance.suggestionRect.transform.GetChild(k).transform.childCount; j++)
            {
                CanvasManager.instance.suggestionRect.transform.GetChild(k).GetChild(j).GetComponent<Image>().sprite = null;
            }
        }

        #endregion

        if (myHeroList.Count != 0)
        {
            myHero = myHeroList[selectedHeroIndex];

            CanvasManager.instance.suggestionRect.transform.GetChild(0).GetComponent<Image>().sprite = myHero.image;
            //CanvasManager.instance.removeHeroButton.onClick.RemoveAllListeners();
            //CanvasManager.instance.removeHeroButton.onClick.AddListener(() => CanvasManager.instance.RemoveHeroButtonOnClick(selectedHeroIndex));

            if (myHero.betterItemIdList == null) // Look here
                return;


            //for (int q = 0; q < CanvasManager.instance.myHeroItemsRect.transform.childCount - 1; q++)
            //{
            //    CanvasManager.instance.myHeroItemsRect.transform.GetChild(q).GetComponent<Image>().sprite = null;
            //}

            for (int p = 0; p < myHero.currentItemList.Count; p++)
            {
                Debug.Log("curren item");
                //CanvasManager.instance.myHeroItemsRect.transform.GetChild(p).GetComponent<Image>().sprite = myHero.currentItemList[p].image;
            }


            //for (int t = 0; t < CanvasManager.instance.bestItemsRect.transform.childCount - 1; t++)
            //{
            //    CanvasManager.instance.bestItemsRect.transform.GetChild(t).GetComponent<Image>().sprite = itemList[myHero.betterItemIdList[t]].image;
            //    CanvasManager.instance.bestItemsRect.transform.GetChild(t).GetChild(0).GetComponent<Text>().text = itemList[myHero.betterItemIdList[t]].description;
            //}

            #region ItemSuggest


            int i = 0;
            foreach (int betterItemId in myHero.betterItemIdList)
            {
                if (i > 3)
                    return;

                if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 1)
                {
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = selectedHeroIndex;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().color =
                        new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.r,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.g,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.b, 1f);

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = Color.white;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;

                }
                else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name != itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 0 && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() > 0)
                {
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = selectedHeroIndex;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().color =
                        new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.r,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.g,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.b, 1f);

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = Color.white;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).gameObject.SetActive(true);


                    i++;
                }
                else
                {
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.b, 0.5f);


                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    #region Color
                    if (myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() == 0)
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.b, 0.5f);
                    }
                    else
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.b, 1f);
                    }

                    if (myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() == 0)
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.b, 0.5f);
                    }
                    else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() == 1)
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.b, 0.5f);
                    }
                    else
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.b, 1f);
                    }

                    #endregion

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;


                    CanvasManager.instance.suggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;
                }
            }

            #endregion

        }
        else
        {
            //CanvasManager.instance.selectedInfoRect.SetActive(false);
            CanvasManager.instance.suggestionRect.SetActive(false);
        }
    }
 * */
