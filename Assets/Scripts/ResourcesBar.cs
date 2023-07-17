using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesBar : MonoBehaviour
{
    
  public RawImage rBar;
  public float Resources;
  public float maxResources;
  public float barWidth;

  public void UpdateHealthBar(int amount) {
    Resources += amount;
    float fillAmount = Resources / maxResources;
    float currentWidth = barWidth * fillAmount;

    // Calculate the difference between the initial width and the current width
    float widthDifference = barWidth - currentWidth;

    // Set the anchoredPosition of the right anchor to the negative widthDifference
    rBar.rectTransform.anchoredPosition = new Vector2(-(widthDifference/2), 0f);

    // Set the size of the RectTransform to the current width
    rBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);

    rBar.color = Color.blue;
  }

  public float getBar(){
    return Resources;
  }

    // Start is called before the first frame update
    void Start()
    {
        Resources = 100;
        maxResources = Resources;
        barWidth = rBar.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
