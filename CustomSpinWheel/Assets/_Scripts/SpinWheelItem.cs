using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spin Wheel Item", menuName = "Spin Wheel/Item")]
public class SpinWheelItem : ScriptableObject
{
    public string name;
    public string quantityText;     // will need to change this type when we scale the project 
    public Sprite itemSprite;

    public int dropChance;          // can change it float if drop chance can be lower than 1%

}
