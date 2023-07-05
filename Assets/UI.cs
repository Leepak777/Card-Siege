using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    public Button button;
    public GameObject card;
    public GameObject trash;
    public GameObject Deck;
    Random rnd;
    public int turn = 1;
    public bool turnEnd = true;
    void Start()
    {
        for(int i = 0; i < 52; i++){
            initDeck(i+1);
        }
        rnd = new Random((int)Time.time*1000);
    }

    public void endTurn(){
        turn++;
        turnEnd = true;
    }
    public void startTurn(){
        turnEnd = false;
        drawCard();
        drawCard();
    }
    public void initDeck(int count){
        GameObject prefab = card;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.position = new Vector3(0,0,2);
        player.transform.localScale = new Vector3(1, 1, 1);
        player.transform.SetParent(Deck.transform);
        player.transform.position = Deck.transform.position;
        player.name = "Card" + (count);
        player.GetComponentInChildren<Text>().text = count+"";
    }
    public void clearDeck(){
        if(!turnEnd){
            foreach(GameObject c in GameObject.FindGameObjectsWithTag("Card")){
                if(c.gameObject.transform.IsChildOf(Deck.transform)){
                    c.transform.SetParent(trash.transform);
                    c.transform.position = trash.transform.position;
                    c.GetComponent<CardEvent>().unhighlight();
                    c.GetComponent<CardEvent>().shrinken();
                }
            }
        }
    }
    public void shuffleDeck(){
        if(!turnEnd){
            int i = 0;
            while(trash.transform.childCount>0){
                i = rnd.Next(0,trash.transform.childCount);
                GameObject card = trash.transform.GetChild(i).gameObject;
                card.transform.SetParent(Deck.transform);
                card.transform.position = Deck.transform.position;
            }
        }
    }
    public void drawCard(){
        if(!turnEnd){
            if(Deck.transform.childCount>0){
                GameObject card = Deck.transform.GetChild(0).gameObject;
                GameObject goParent = GameObject.Find("Cards");
                int count = 0;
                for(int i = 0; i < goParent.transform.childCount; i++){
                    if(goParent.transform.GetChild(i).tag == "Card"){
                        count++;
                    }
                }
                if(count >= 7){
                    return;
                }
                card.transform.SetParent(goParent.transform);
            }
        }
    }
    public void createCard(){
        GameObject goParent = GameObject.Find("Cards");
        int count = 0;
        for(int i = 0; i < goParent.transform.childCount; i++){
            if(goParent.transform.GetChild(i).tag == "Card"){
                count++;
            }
        }
        if(count >= 7){
            return;
        }
        
        GameObject prefab = card;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.position = new Vector3(0,0,2);
        player.transform.localScale = new Vector3(1, 1, 1);
        player.transform.SetParent(goParent.transform);
        
        player.name = "Card" + (count);
        
    
    }
    public bool checkselected(){
        foreach(GameObject card in GameObject.FindGameObjectsWithTag("Card")){
            if(card.GetComponent<CardEvent>().isSelected()){
                return true;
            }
        }
        return false;
        
    }
    public void playCard(){
        if(!turnEnd){
            if(checkselected()){
                GameObject c = getSelected();
                if(c.gameObject.transform.IsChildOf(GameObject.Find("Cards").transform)){
                    c.transform.SetParent(trash.transform);
                    c.transform.position = trash.transform.position;
                    c.GetComponent<CardEvent>().Toggle();
                    c.GetComponent<CardEvent>().shrinken();
                    checkEmpty();
                }
            }
        }
    }

    public GameObject getSelected(){
        foreach(GameObject card in GameObject.FindGameObjectsWithTag("Card")){
            if(card.GetComponent<CardEvent>().isSelected()){
                return card;
            }
        }
        return null;

    }

    public void checkEmpty(){
        if(GameObject.Find("Cards").transform.childCount == 0 && Deck.transform.childCount == 0 ){
            shuffleDeck();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
