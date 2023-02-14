using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent abstract class of all items in the game
public abstract class Item : ScriptableObject{

    [SerializeField]
    //The icon of the item
    private Sprite icon;

    [SerializeField]
    //Stack items
    private int stackSize;

    //Reference to slot
    private SlotScript slot;

    //Property to access the icon
    public Sprite MyIcon{
        get{
            return icon;
        }
    }

    //Property to access the stack size
    public int MyStackSize{
        get{
            return stackSize;
        }
    }

    //Property to access the slot script
    public SlotScript MySlot{
        get{
            return slot;
        }
        set{
            slot = value;
        }
    }
    
    //Function to remove the item
    public void Remove(){

        //If the item exists -> then
        if(MySlot != null){

            //Remove the item from slot
            MySlot.RemoveItem(this);
        }
    }
}
