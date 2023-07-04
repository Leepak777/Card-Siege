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
    // Update is called once per frame
    void Update()
    {
        
    }
}
