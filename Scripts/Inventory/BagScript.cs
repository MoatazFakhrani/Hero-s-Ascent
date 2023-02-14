using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour{

    [SerializeField]
    //Prefab to create slots
    private GameObject slotPrefab;

    //Canvas group to show and hide the bag
    private CanvasGroup canvasGroup;

    //List of the slots 
    private List<SlotScript> slots = new List<SlotScript>();

    //Property checks if the bag is open or closed
    public bool IsOpen{
        get{
            //If true then open (by assigning alpha larger than 0)
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> MySlots{
        
        get{
            return slots;
        }
    }

    private void Awake(){

        //Canvas group reference
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //Create the amount of slots 
    public void AddSlots(int slotCount){

        for(int i = 0; i < slotCount; i++){

            //Instantiate slot prefab and store it in slot variable
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slots.Add(slot);
        }
    }

    public bool AddItem(Item item){

        foreach(SlotScript slot in slots){
            //If there is an empty slot -> then
            if(slot.IsEmpty){
                //Add item
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    //Function to open and close the bag
    public void OpenClose(){

        //Set the alpha to 0 or 1 (0 = hidden, 1 = shown)
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        //Block or remove raycast blocking
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
