using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUsable{

    [SerializeField]
    private int health;
    public void Use(){

        //If Player's health is less than the maximum health -> then
        if(Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue){
            
            //The player can use Health potion
            Remove();
            Player.MyInstance.MyHealth.MyCurrentValue +=  health;
        }
    }  
}
