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
    
    void Start()
    {
        
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
        if(checkselected()){
            GameObject c = getSelected();
            if(c.gameObject.transform.IsChildOf(GameObject.Find("Cards").transform)){
                c.transform.SetParent(trash.transform);
                c.transform.position = trash.transform.position;
                c.GetComponent<CardEvent>().Toggle();
                c.GetComponent<CardEvent>().shrinken();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
