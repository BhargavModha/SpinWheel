using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpinWheelAnimations : MonoBehaviour
{

    public GameObject wheel;
    public GameObject wheelBorder;
    public GameObject wheelPointer;

    public GameObject EndResultObject;
    public GameObject PlayButton;
    
    private string imageObjectName = "ItemImage";
    private string textObjectName = "ItemQuantity";

    void Start()
    {   
        SpinWheelManager.instance.onSpinStart += HidePlayButton;
        SpinWheelManager.instance.onSpinEnd += HideSpinWheelObjects;
        SpinWheelManager.instance.onSpinEnd += SetEndResultObject;
        SpinWheelManager.instance.onSpinReset += ShowAllWheelObjects;
    }

    public void HidePlayButton(){
        PlayButton.SetActive(false);
    }

    public void HideSpinWheelObjects(SpinWheelItem item){
        wheel.SetActive(false);
        wheelBorder.SetActive(false);
        wheelPointer.SetActive(false);
    }

    public void SetEndResultObject(SpinWheelItem item){

        Transform getItemImage = EndResultObject.transform.Find(imageObjectName);
        Transform getItemQuantity = EndResultObject.transform.Find(textObjectName);
        
        Image itemImage = getItemImage.GetComponent<Image>();
        TextMeshProUGUI itemQuantity = getItemQuantity.GetComponent<TextMeshProUGUI>();

        // assigns sprite and text from scriptable object
        itemImage.sprite = item.itemSprite;
        itemQuantity.text = item.quantityText;     
    }

    public void ShowAllWheelObjects(){
        wheel.SetActive(true);
        wheelBorder.SetActive(true);
        wheelPointer.SetActive(true);
        PlayButton.SetActive(true);
    }


}
