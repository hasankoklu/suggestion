﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static GameManager;

public class CanvasManager : MonoBehaviour
{
    #region Contents

    public Transform heroMenu_Content;
    public Transform myHeroMenu_Content;
    public Transform currentTeamBuff_Content;
    public Transform askWinLose_Content;

    #endregion

    public static CanvasManager instance;

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
        for (int t = 0; t < currentTeamBuff_Content.transform.childCount; t++)
        {
            currentTeamBuff_Content.transform.GetChild(t).gameObject.SetActive(false);
        }

        RefreshListes();
    }

    public void RefreshListes()
    {
        SetItemPiece();
        SetComplateItem();
        SetHeroList();
        SetMyHeroList(GameManager.instance.myHeroList);

    }

    public void RemoveHeroButtonOnClick(Hero hero)
    {
        GameManager.instance.myHeroList.Remove(hero);
        RefreshListes();
    }

    public void PlayerLevelIncreaseButtonClick()
    {
        //GameManager.instance.currentPlayer.PlayerLevel++;
        //PlayerLevelText.text = GameManager.instance.playerLevel.ToString();
    }

    public void PlayerLevelDecreaseButtonClick()
    {
        if (GameManager.instance.playerLevel > 1)
        {
            //GameManager.instance.playerLevel--;
            //PlayerLevelText.text = GameManager.instance.playerLevel.ToString();
        }
    }

    List<BestHeroTeam> tempBestHeroTeamList = new List<BestHeroTeam>();

    public void TeamWon()
    {

        LoadBestTeamList();
        tempBestHeroTeamList = GameManager.instance.bestHeroTeamList;
        foreach (Hero hero in GameManager.instance.myHeroList)
        {
            tempBestHeroTeamList = GameManager.instance.bestHeroTeamList.Where(x => x.heroList.Contains(hero)).ToList();
        }

        if (tempBestHeroTeamList.Count > 0)
        {
            GameManager.instance.bestHeroTeamList.Where(x => x == tempBestHeroTeamList.FirstOrDefault()).FirstOrDefault().winCount++;
        }
        else
        {
            BestHeroTeam tempBestHeroTeam = new BestHeroTeam();
            tempBestHeroTeam.winCount = 1;
            tempBestHeroTeam.heroList = GameManager.instance.myHeroList;

            GameManager.instance.theBestTeamList.Add(tempBestHeroTeam);

        }
        SaveBestTeamList();
    }

    public void TeamLost()
    {
        LoadBestTeamList();
        tempBestHeroTeamList = GameManager.instance.bestHeroTeamList;
        foreach (Hero hero in GameManager.instance.myHeroList)
        {
            tempBestHeroTeamList = GameManager.instance.bestHeroTeamList.Where(x => x.heroList.Contains(hero)).ToList();
        }

        if (tempBestHeroTeamList.Count > 0)
        {
            GameManager.instance.bestHeroTeamList.Where(x => x == tempBestHeroTeamList.FirstOrDefault()).FirstOrDefault().winCount--;
        }
        else
        {
            BestHeroTeam tempBestHeroTeam = new BestHeroTeam();
            tempBestHeroTeam.loseCount = 1;
            tempBestHeroTeam.heroList = GameManager.instance.myHeroList;

            GameManager.instance.theBestTeamList.Add(tempBestHeroTeam);
        }
        SaveBestTeamList();
    }

    public void SaveBestTeamList()
    {
        Debug.Log("GameManager.instance.bestTeamList Kayıt edilecek.");
    }

    public void LoadBestTeamList()
    {
        Debug.Log("GameManager.instance.bestTeamList Geri Yüklenecek.");
    }

    public void IsHoldButtonClick()
    {
        isHold = true;
    }

    bool isHold = false;
    public IEnumerator AskWinLoseSystem()
    {
        while (GameManager.instance.myHeroList.Count > 1)
        {
            if (isHold)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(1000, 1200));
                isHold = false;
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(150, 210));
            }
        }
    }

    #region ItemPieceAddRemove

    public void AddItemPieceToMyItemPieceList(int index)
    {
        GameManager.instance.myPieceItemList.Add(GameManager.instance.itemPieceList[index]);

    }

    public void RemoveItemPieceToMyItemPieceList(int index)
    {
        GameManager.instance.myPieceItemList.Remove(
            GameManager.instance.myPieceItemList.Where(x => x.name == GameManager.instance.itemPieceList[index].name).FirstOrDefault()
            );
    }

    #endregion

    #region GeneralListsArrangement

    void SetHeroList()
    {
        GameManager.instance.heroList = GameManager.instance.heroList.OrderBy(x => x.level).ThenBy(x => x.HeroGenericType).ToList();

        for (int i = 0; i < heroMenu_Content.childCount; i++)
        {
            Destroy(heroMenu_Content.GetChild(i).gameObject);
        }

        if (GameManager.instance.myHeroList.Count > 0)
        {

            foreach (Hero hero in GameManager.instance.suggestionHeroList)
            {
                Hero myHero = new Hero();
                myHero = hero;
                myHero.gameObject = Instantiate(PrefabManager.instance.Hero_Button);
                myHero.gameObject.transform.SetParent(heroMenu_Content);
                myHero.gameObject.GetComponent<Image>().sprite = hero.image;
                myHero.gameObject.transform.GetChild(0).GetComponent<Text>().text = hero.name;
                myHero.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].image;
                myHero.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].image;
                myHero.gameObject.transform.GetChild(3).GetComponent<Text>().text = "%" + hero.winRate.ToString();

                myHero.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                myHero.gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.AddHeroToMyHeroList(myHero));
            }
        }
        else
        {
            foreach (Hero hero in GameManager.instance.heroList)
            {
                Hero myHero = new Hero();
                myHero = hero;
                myHero.gameObject = Instantiate(PrefabManager.instance.Hero_Button);
                myHero.gameObject.transform.SetParent(heroMenu_Content);
                myHero.gameObject.GetComponent<Image>().sprite = hero.image;
                myHero.gameObject.transform.GetChild(0).GetComponent<Text>().text = hero.name;
                myHero.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].image;
                myHero.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].image;
                myHero.gameObject.transform.GetChild(3).GetComponent<Text>().text = "";

                myHero.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                myHero.gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.AddHeroToMyHeroList(myHero));
            }
        }
    }

    void SetItemPiece()
    {
        foreach (ItemPiece itemPiece in GameManager.instance.itemPieceList)
        {
            //itemPiece.gameObject = Instantiate(AddIPieceItemButtonPrefab);

            itemPiece.gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.TakePieceItem(itemPiece));

            itemPiece.gameObject.GetComponent<Image>().sprite = itemPiece.image;

            if (GameManager.instance.myPieceItemList.Where(x => x.name == itemPiece.name).FirstOrDefault() != null)
                itemPiece.gameObject.transform.GetChild(0).GetComponent<Text>().text =
                    GameManager.instance.myPieceItemList.Where(x => x.name == itemPiece.name).Count().ToString();
            else
            {
                itemPiece.gameObject.transform.GetChild(0).GetComponent<Text>().text = "0";
            }
        }

    }

    void SetComplateItem()
    {
        foreach (Item item in GameManager.instance.itemList)
        {
            item.gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.TakeItem(item));

            item.gameObject.GetComponent<Image>().sprite = item.image;
        }
    }

    #endregion

    #region MyListsArrangement

    public void SetMyHeroList(List<Hero> heroList)
    {
        for (int i = 0; i < myHeroMenu_Content.transform.childCount; i++)
        {
            Destroy(myHeroMenu_Content.transform.GetChild(i).gameObject);
        }


        foreach (Hero hero in GameManager.instance.myHeroList)
        {
            hero.gameObject = Instantiate(PrefabManager.instance.HeroCard_Button);
            hero.gameObject.transform.SetParent(myHeroMenu_Content.transform);

            hero.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

            hero.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => RemoveHeroButtonOnClick(hero));

            hero.gameObject.transform.GetComponent<Image>().sprite = hero.cardImage;
            hero.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = hero.name;

            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].image;
            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].name;

            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].image;
            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].name;

            if (hero.HeroFightStyleList[1] != 0)
            {
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[1]].image;
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[1]].name;
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).gameObject.SetActive(false);
            }


            GameManager.instance.SetHeroItems(hero);

            #region Current Buff System

            GameManager.instance.currentBuffNameList.Clear();

            for (int t = 0; t < GameManager.instance.heroGenericTypeList.Count; t++)
            {
                if (GameManager.instance.myHeroList.Where(x => x.HeroGenericType == t).Count() / GameManager.instance.heroGenericTypeList[t].exponent >= 1)
                {
                    for (int j = 0; j < (int)(GameManager.instance.myHeroList.Where(x => x.HeroGenericType == t).Count() / GameManager.instance.heroGenericTypeList[t].exponent); j++)
                    {
                        GameManager.instance.currentBuffNameList.Add(GameManager.instance.heroGenericTypeList[t].name);
                    }
                }
            }

            for (int t = 0; t < GameManager.instance.heroFightStyleList.Count; t++)
            {
                if ((GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[0] == t).Count() + GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[1] == t).Count()) / GameManager.instance.heroFightStyleList[t].exponent >= 1)
                {

                    for (int j = 0; j < (int)((GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[0] == t).Count() + GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[1] == t).Count()) / GameManager.instance.heroFightStyleList[t].exponent); j++)
                    {
                        GameManager.instance.currentBuffNameList.Add(GameManager.instance.heroFightStyleList[t].name);
                    }
                }
            }

            for (int i = 0; i < currentTeamBuff_Content.transform.childCount; i++)
            {

                Destroy(currentTeamBuff_Content.transform.GetChild(i));
            }

            #endregion
        }
    }
    #endregion
}
