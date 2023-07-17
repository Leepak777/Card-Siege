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

public class CardEvent : MonoBehaviour
{
    // Start is called before the first frame update
    Color highlightColor = Color.yellow;
    public bool selected = false;
    public Vector3 ogScale = new Vector3(0,0,0);
    public int order = 0;
    public bool isDragging = false;
    Vector3 ogPos = new Vector3(0,0,0); 
    GameObject ogParent;
    UI ui;
    ResourcesBar bar;
    public GameObject hand;
    public GameObject trash;
    public GameObject Deck;
    public GameObject temp;
    public GameObject summon;
    public Button b;
    public Text txt;
    void Start()
    {
        ogScale = gameObject.transform.localScale;
        highlightColor = GetComponent<Renderer>().material.color;
        order = GetComponent<Renderer>().sortingOrder;
    }
    public void initCard(UI ui, ResourcesBar bar,GameObject hand,GameObject trash,GameObject Deck,GameObject temp,GameObject summon){
        this.ui = ui;
        this.bar = bar;
        this.hand = hand;
        this.trash = trash;
        this.Deck = Deck;
        this.temp = temp;
        this.summon = summon; 
    }

    public void Toggle(){
        if((GameObject.Find("UI").GetComponent<UI>().checkselected()&& !selected)){
                return;
        }
        selected = !selected;
        if(selected){
            forward();
            highlight();
        }
        else{
            back();
            unhighlight();
            }
    }
    public void enlarge(){
        if(!GameObject.Find("UI").GetComponent<UI>().checkselected())
            gameObject.transform.localScale = ogScale*1.69f;
    }
    public void shrinken(){
        if(!selected)
            gameObject.transform.localScale = ogScale;
    }
    public void highlight(){
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    public void unhighlight(){
        GetComponent<Renderer>().material.color = highlightColor;
    }
    public void forward(){
        GetComponent<Renderer>().sortingOrder = 99;
    }
    public void back(){
        GetComponent<Renderer>().sortingOrder = order;
    }
    public bool isSelected(){
        return selected;
    }
    public void play(){
        if(!ui.isTurnEnded()){
            if(transform.IsChildOf(hand.transform) && bar.getBar() >= 5){
                    selected = false;
                    transform.SetParent(summon.transform);
                    //transform.position = summon.transform.position;
                    shrinken();
                    ui.checkEmpty();
                    //c.GetComponentInChildren<Text>().enabled = (false);
                    bar.UpdateHealthBar(-5);
            }
        }
    }

    private Vector3 screenPoint;
    private Vector3 offset;

    public void OnMouseDown()
    {
        if(!ui.isTurnEnded()){
            selected = true;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

            ogPos = transform.position;
            ogParent = transform.parent.gameObject;
        }
    }

    public void OnMouseDrag()
    {
        if(!ui.isTurnEnded()){
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }
    public void OnMouseUp(){
        if(!ui.isTurnEnded()){
            selected = false;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            RectTransform r = GameObject.Find("Hand").GetComponent<RectTransform>();
            RectTransform t = GameObject.Find("Trash").GetComponent<RectTransform>();
            RectTransform d = GameObject.Find("Deck").GetComponent<RectTransform>();
            RectTransform s = GameObject.Find("Summon").GetComponent<RectTransform>();
            ResourcesBar bar = GameObject.Find("InnerBar").GetComponent<ResourcesBar>();
            if(RectTransformUtility.RectangleContainsScreenPoint(r, curScreenPoint , Camera.main) && GameObject.Find("Cards").transform.childCount < 7){
                gameObject.transform.SetSiblingIndex(GameObject.Find("Cards").transform.childCount);
                gameObject.transform.SetParent(GameObject.Find("Cards").transform);
                shrinken();
                txt.enabled = true;
                b.enabled = true;
            }
            else if(RectTransformUtility.RectangleContainsScreenPoint(d, curScreenPoint , Camera.main)){
                transform.position = GameObject.Find("DeckCards").transform.position;
                gameObject.transform.SetParent(GameObject.Find("DeckCards").transform);
                txt.enabled = false;
                b.enabled = false;

                shrinken();
            }
            else if(RectTransformUtility.RectangleContainsScreenPoint(s, curScreenPoint , Camera.main)){
                if(transform.IsChildOf(GameObject.Find("Cards").transform) && bar.getBar() >= 5){
                    bar.UpdateHealthBar(-5);
                    transform.position = GameObject.Find("SummonCards").transform.position;
                    gameObject.transform.SetParent(GameObject.Find("SummonCards").transform);
                    shrinken();
                }
                else{
                    reset();
                }
            }
            else if(RectTransformUtility.RectangleContainsScreenPoint(t, curScreenPoint , Camera.main)){
                if(transform.IsChildOf(GameObject.Find("SummonCards").transform)){
                    bar.UpdateHealthBar(-5);
                    transform.position = GameObject.Find("TrashDeck").transform.position;
                    gameObject.transform.SetParent(GameObject.Find("TrashDeck").transform);
                    txt.enabled = false;
                    b.enabled = false;
                    shrinken();
                }
                else{
                    reset();
                }
            }
            else{
                reset();
            }
            shrinken(); 
        }
        
    }

    void reset(){
         
            transform.SetParent(ogParent.transform);
            transform.position = ogPos;
            shrinken();
            if(transform.parent.name != "Cards"){
                txt.enabled = false;
                b.enabled = false;
            }
            else{
                txt.enabled = true;
                b.enabled = true;
            }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
