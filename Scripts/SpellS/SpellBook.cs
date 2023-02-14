using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour{

    //Singleton 
    private static SpellBook instance;

    public static SpellBook MyInstance{
        get{
            if(instance == null){
                instance = FindObjectOfType<SpellBook>();
            }
            return instance;
        }
    }

    [SerializeField]
    //Reference to the casting bar
    private Image castingBar;

    [SerializeField]
    //Reference to the spell name on the casting bar
    private Text currentSpell;

    [SerializeField]
    //Reference to the cast time on the casting bar
    private Text castTime;

    [SerializeField]
    //Reference to the icon on casting bar
    private Image icon;

    [SerializeField]
    //Reference to the canvas group
    private CanvasGroup canvasGroup;

    [SerializeField]
    //Reference to the spells
    private Spell[] spells;

    private Coroutine spellRoutine;
    private Coroutine fadeRoutine;

    public Spell CastSpell(string spellName){

        //Find the spell based on its name
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        //Reset the fillamount on the bar
        castingBar.fillAmount = 0;      

        //Change the color on the bar based on a specific spell
        castingBar.color = spell.MybarColor;

        //Change the text on the bar based on a specific spell
        currentSpell.text = spell.MyName;

        //Change the icon on the bar based on a specific spell
        icon.sprite = spell.MyIcon;

        //Start casting
        spellRoutine = StartCoroutine(Progress(spell));

        //Fade the bar
        fadeRoutine = StartCoroutine(FadeBar());

        //Return the casted spell
        return spell;
    }

    
    private IEnumerator Progress(Spell spell){

        float timePassed = Time.deltaTime;
        float rate = 1.0f /spell.MyCastTime;
        
        float progress = 0.0f;

        while(progress <= 1.0){
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (spell.MyCastTime - timePassed).ToString("F1");

            if(spell.MyCastTime - timePassed < 0){
                castTime.text = "0.0";
            }

            yield return null;
        }
        StopCasting();
    }

    private IEnumerator FadeBar(){
        
        float rate = 1.0f / 0.5f;

        float progress = 0.0f;

        while(progress <= 1.0){
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    public void StopCasting(){
        if(fadeRoutine != null){
            //fade cast bar after its done
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
        if(spellRoutine != null){
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    public Spell GetSpell(string spellName){

        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }
}
