using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler{

    //Usable is the item on the action bar
    public IUsable MyUsable{ get;  set; }

    public Button MyButton{ get; /*private*/ set; }

    // Start is called before the first frame update
    /*void Start(){

        //Reference to the button in Unity
        MyButton = GetComponent<Button>();
        
        //Add listener to the button to execute OnClick function 
        MyButton.onClick.AddListener(OnClick);
    }*/

    public void Awake(){

        //Reference to the button in Unity
        MyButton = GetComponent<Button>();
        
        //Add listener to the button to execute OnClick function 
        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void OnClick(){

        //If there is an item in the action bar (not null)
        if(MyUsable != null){

            //Then we are able to use it
            MyUsable.Use();

        }
    }

    public void OnPointerClick(PointerEventData eventData){
        
    }
}
