using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

public class SpinWheelPopulateData : MonoBehaviour
{
    public SpinWheelItem item;

    private string imageObjectName = "ItemImage";
    private string textObjectName = "ItemQuantity";

     // Checks the structure for spin wheel item
    private void OnValidate()
    {
        Assert.IsNotNull(item, transform.name + " is missing SpinWheelItem ScriptableObject");

        // check if gameobject exists
        Transform getItemImage = transform.Find(imageObjectName);
        Transform getItemQuantity = transform.Find(textObjectName);
        Assert.IsNotNull(getItemImage, transform.name + " is missing "+ imageObjectName +" ChildObject");
        Assert.IsNotNull(getItemQuantity, transform.name + " is missing "+ textObjectName +" ChildObject");
        
        // check if required components exists
        Image itemImage = getItemImage.GetComponent<Image>();
        TextMeshProUGUI itemQuantity = getItemQuantity.GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(itemImage, transform.name + " is missing Image Component in "+ imageObjectName +" ChildObject");
        Assert.IsNotNull(itemQuantity, transform.name + " is missing TextMeshProUGUI Component in "+ textObjectName +" ChildObject");

        // assigns sprite and text from scriptable object
        itemImage.sprite = item.itemSprite;
        itemQuantity.text = item.quantityText;     
    }
}
