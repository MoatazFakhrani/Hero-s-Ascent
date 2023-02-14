using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{

    //Singleton 
    private static UIManager instance;

    public static UIManager MyInstance{
        get{
            if(instance == null){
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    //Refrence to Action Buttons from unity
    public ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetFrame;
    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private CanvasGroup keybindMenu;
    private GameObject[] keybindButtons;

    void Awake(){
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    // Start is called before the first frame update
    void Start(){

        healthStat = targetFrame.GetComponentInChildren<Stat>();

        //Hardcoded to set the abilities on the Action Buttons
        SetUsable(actionButtons[0], SpellBook.MyInstance.GetSpell("Fireball"));
        SetUsable(actionButtons[1], SpellBook.MyInstance.GetSpell("Frostbolt"));
        SetUsable(actionButtons[2], SpellBook.MyInstance.GetSpell("Thunderbolt"));
    }

    // Update is called once per frame
    void Update(){

        if(Input.GetKeyDown(KeyCode.Escape)){
            OpenCloseMenu();
        }

        if(Input.GetKeyDown(KeyCode.B)){
            InventoryScript.MyInstance.OpenClose();
        }
    }

    public void ShowTargetFrame(NPC target){
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
    }

    public void HideTargetFrame(){
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health){
        healthStat.MyCurrentValue = health;
    }

    public void OpenCloseMenu(){
        
        //If alpha is larger than 0 put it back to zero, otherwise put it to 1
        keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1;

        //If blockraycast is true then set to false, otherwise it is true
        keybindMenu.blocksRaycasts = keybindMenu.blocksRaycasts == true ? false : true;

        //If the time scale is more than 0 put it to 0, otherwise put it to 1 -> used to pause the game
        Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    //Function used to show the text of the keybinds on the menu
    public void UpdateKeyText(string key, KeyCode code){

        //in the array we check if the key is equals to the key we want to bind then get the children which is a text and we can change its value
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    //Function for clicking action based on the name
    public void ClickActionButton(string buttonName){
        //Look through the arrat of the action buttons, define each action button as x check its name and invoke that button 
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void SetUsable(ActionButton btn, IUsable usable){

        btn.MyButton.image.sprite = usable.MyIcon;
        btn.MyButton.image.color = Color.white;
        btn.MyUsable = usable;
    }


    //Function to update stack size on slots
    public void UpdateStackSize(IClickable clickable){

        if(clickable.MyCount > 1){
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else{
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
        //If the clickble item is Empty -> then
        if(clickable.MyCount == 0){

            //Hide the icon of the item
            clickable.MyIcon.color = new Color(0, 0, 0, 0);

            //Hide the text of the item
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }
}