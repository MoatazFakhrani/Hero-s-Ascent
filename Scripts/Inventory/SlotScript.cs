using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable{

    //items variable is to contain the item in a slot (by using Stack)
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    //Reference to the icon of the item in a slot
    private Image icon;

    [SerializeField]
    //Text of item count
    private Text stackSize;

    //Check if there is an empty slot
    public bool IsEmpty{
        get{
            return items.Count == 0;
        }
    }

    //Property to return items on the stack
    public Item MyItem{
        get{
            //If there is an item (Not Empty) -> then
            if(!IsEmpty){

                //To look on items
                return items.Peek();
            }

            //Return nothing if it is (Empty)
            return null;
        }
    }

    public Image MyIcon{
        get{
            return icon;
        }

        set{
            icon = value;
        }
    }

    public int MyCount {
        get{
            return items.Count;
        }
    }

    public Text MyStackText{
        get{
            return stackSize;
        }
    }

    private void Awake(){

        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    //Function to add items on slots
    public bool AddItem(Item item){

        //Add item to the Stack
        items.Push(item);

        //Based the icon based on the item
        icon.sprite = item.MyIcon;

        //Set icon color to white
        icon.color = Color.white;

        item.MySlot = this;
        return true;
    }


    //Function to remove items from slot
    public void RemoveItem(Item item){

        if(!IsEmpty){

            //Remove item from the stack
            items.Pop();
        }
    }

    public void OnPointerClick(PointerEventData eventData){

        //If the item is clicked (left click on mouse) -> then
        if(eventData.button == PointerEventData.InputButton.Left){

            //Use the item
            UseItem();
        }
    }

    public void UseItem(){

        if(MyItem is IUsable){

            (MyItem as IUsable).Use();
        }
    }

    public bool StackItem(Item item){

        //If the slot is not Empty AND the item about to be added the same as the one in the bag? -> then
        if(!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize){

             //Check if the item can stack, if yes then it will Push item on top of the stack
                items.Push(item);
                item.MySlot = this;
                return true;
        }
        return false;
    }

    private void UpdateSlot(){

        UIManager.MyInstance.UpdateStackSize(this);
    }
}
