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
    void Start()
    {
        ogScale = gameObject.transform.localScale;
        highlightColor = GetComponent<Renderer>().material.color;
        order = GetComponent<Renderer>().sortingOrder;
    }
    Color highlightColor = Color.yellow;
    public bool selected = false;
    public Vector3 ogScale = new Vector3(0,0,0);
    public int order = 0;
    public bool isDragging = false;
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

    private Vector3 screenPoint;
    private Vector3 offset;

    public void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    public void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        RectTransform r = GameObject.Find("Hand").GetComponent<RectTransform>();
        if(RectTransformUtility.RectangleContainsScreenPoint(r, curScreenPoint , Camera.main)){
            gameObject.transform.SetSiblingIndex(GameObject.Find("Temp").transform.childCount);
            gameObject.transform.SetParent(GameObject.Find("Temp").transform);
            shrinken();
            gameObject.GetComponentInChildren<Text>().enabled = true;
        }
        else{
            transform.position = GameObject.Find("Deck").transform.position;
            gameObject.transform.SetParent(GameObject.Find("Deck").transform);
            gameObject.GetComponentInChildren<Text>().enabled = false;
        }
    }
    public void OnMouseUp(){
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        RectTransform r = GameObject.Find("Hand").GetComponent<RectTransform>();
        RectTransform t = GameObject.Find("Trash").GetComponent<RectTransform>();
        RectTransform d = GameObject.Find("Deck").GetComponent<RectTransform>();
        if(RectTransformUtility.RectangleContainsScreenPoint(r, curScreenPoint , Camera.main)){
            gameObject.transform.SetSiblingIndex(GameObject.Find("Cards").transform.childCount);
            gameObject.transform.SetParent(GameObject.Find("Cards").transform);
            shrinken();
            gameObject.GetComponentInChildren<Text>().enabled = true;
        }
        else if(RectTransformUtility.RectangleContainsScreenPoint(d, curScreenPoint , Camera.main)){
            transform.position = GameObject.Find("DeckCards").transform.position;
            gameObject.transform.SetParent(GameObject.Find("DeckCards").transform);
            gameObject.GetComponentInChildren<Text>().enabled = false;
        }
        else if(RectTransformUtility.RectangleContainsScreenPoint(t, curScreenPoint , Camera.main)){
            transform.position = GameObject.Find("TrashDeck").transform.position;
            gameObject.transform.SetParent(GameObject.Find("TrashDeck").transform);
            gameObject.GetComponentInChildren<Text>().enabled = false;
        }
            
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
