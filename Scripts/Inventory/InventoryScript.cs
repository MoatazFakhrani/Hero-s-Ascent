using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour{

    //Singleton 
    private static InventoryScript instance;

    public static InventoryScript MyInstance{
        get{
            if(instance == null){
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    [SerializeField]
    private Item[] items;

    public bool CanAddBag{
        get{
            return bags.Count < 1;
        }
    }

    private void Awake(){
        Bag bag = (Bag)Instantiate(items[0]);

        //Bag slot is 9
        bag.Initialize(6);
        bag.Use();
    }

    public void Update(){

    }

    public void AddPotion(){
        HealthPotion potion =(HealthPotion) Instantiate(items[1]);
        AddItem(potion);
    }

   public void AddBag(Bag bag){
        foreach(BagButton bagButton in bagButtons){

            if(bagButton.MyBag == null){
                bagButton.MyBag = bag;
                bags.Add(bag);
                break;
            }
        }
    }

    public void AddItem(Item item){
        
        if(item.MyStackSize > 0){

            //If the item added is Stackable then stack it
            if(PlaceInStack(item)){
                return;
            }
        }

        //Otherwise place it in an empty slot
        PlaceInEmpty(item);
    }

    //Function to place items in an empty slot
    private void PlaceInEmpty(Item item){

        foreach(Bag bag in bags){

            if(bag.MyBagScript.AddItem(item)){
                return;
            }
        }       
    }

    //Function to place items in exist stack 
    private bool PlaceInStack(Item item){

        foreach(Bag bag in bags){

            //Check all the slots inside the bag
            foreach(SlotScript slots in bag.MyBagScript.MySlots){

                if(slots.StackItem(item)){
                    return true;
                }
            }
        }

        //If Stacking items is not possible -> then return false
        return false;
    }

    //Open and close the inventory
    public void OpenClose(){

        //Check if the bag is closed
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

        //If closed bag is true then open the closed bag
        //If closed bag is false then close the opened bag
        foreach(Bag bag in bags){
            if(bag.MyBagScript.IsOpen != closedBag){
                bag.MyBagScript.OpenClose();
            }
        }
    }
}
