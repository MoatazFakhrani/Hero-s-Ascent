using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KeybindManager : MonoBehaviour{
    
    //Make keybind manager singelton so it can be reached from other classes
    private static KeybindManager instance;
    public static KeybindManager MyInstance{
        get{
            if(instance == null){

                instance = FindObjectOfType<KeybindManager>();
            }
            return instance;
        }
    }

    public Dictionary<string, KeyCode> Keybinds { get; set; }

    public Dictionary<string, KeyCode> ActionBinds { get; set; }

    private string bindName;

    // Start is called before the first frame update
    void Start(){
        
        Keybinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();

        //used in player script to do the input
        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        //Keybinds of action buttons name in Unity
        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind){

        //Bind keybinds
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        //if the keybind contains the action bind -> then 
        if(key.Contains("ACT")){

            //current dictionary equals to Action binds (not key binds)
            currentDictionary = ActionBinds;
        }

        //If you want to bind a new key to a keybind -> then
        if(!currentDictionary.ContainsKey(key)){

            //Add it to the dictionary
            currentDictionary.Add(key, keyBind);

            //Update the value of the text
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }

        //If the keybind is laready assigned the same value -> then
        else if(currentDictionary.ContainsValue(keyBind)){
            
            //Fid the key already bind to that keybind
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            //Unbild the original one 
            currentDictionary[myKey] = KeyCode.None;

            //Update the value of the text to None
            UIManager.MyInstance.UpdateKeyText(key, KeyCode.None);
        }

        //set the new key the user trying to bind 
        currentDictionary[key] = keyBind;

        //Update the value of the text
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    //Function for changing the keybinds on the menu
    public void KeyBindOnClick(string bindName){

        this.bindName =  bindName;
    }

    //Get event and check what key is being pressed
    private void OnGUI(){

        //If a key is being binded -> then
        if(bindName != string.Empty){

            //Check the event (could be any key press)
            Event e = Event.current;

            //If the event is a key press -> then
            if(e.isKey){

                //Bind the key pressed
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
