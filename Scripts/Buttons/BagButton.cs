using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler{

    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    public Bag MyBag{
        get{
            return bag;
        }
        set{
            if(value != null){
                GetComponent<Image>().sprite = full;
            }
            else{
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

   public void OnPointerClick(PointerEventData eventData){

        //If there is a bag -> then
        if(bag != null){

            //Open it
            bag.MyBagScript.OpenClose();

        }
   }
}
