using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a menu
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUsable{

    private int slots;
    
    [SerializeField]
    //Prefab od the bag
    private GameObject bagPrefab;

    //Property for setting the bag script 
    public BagScript MyBagScript{ get; set; }

    //Make property of slots to get them
    public int Slots{
        get{
            return slots;
        }
    }

    public void Initialize(int slots){

        this.slots = slots;
    }
     
    public void Use(){

        //Can add bag only if you dont have any bag
        if(InventoryScript.MyInstance.CanAddBag){
            
            Remove();
            //Instantiate a bag prefab by getting a game object from bag script
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);
            InventoryScript.MyInstance.AddBag(this);
        } 
    }
}
