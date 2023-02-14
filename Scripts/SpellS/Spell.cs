using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
//Implements IUsable interface
public class Spell : IUsable{

    [SerializeField]
    //Spell name
    private string name;

    [SerializeField]
    //Spell Damage
    private int damage;

    [SerializeField]
    //Spell Icon
    private Sprite icon;

    [SerializeField]
    //Spell Speed 
    private float speed;

    [SerializeField]
    ////Spell cast time
    private float castTime;

    [SerializeField]
    //Spell Prefab
    private GameObject spellPrefab;

    [SerializeField]
    //Spell Color
    private Color barColor;

    //Property for reading the name of the spell
    public string MyName{ 
        get {
            return name;
        }  
    }

    //Property for reading the damage of the spell
    public int MyDamage { 
        get{
            return damage; 
        } 
    }

    //Property for reading the icon of the spell
    public Sprite MyIcon{ 
        get{
            return icon;
        } 
    }

    //Property for reading the speed of the spell
    public float MySpeed{ 
        get{
            return speed; 
        } 
    }

    //Property for reading the cast time of the spell
    public float MyCastTime{ 
        get{
            return castTime;
        } 
    }

    //Property for reading the prefab of the spell
    public GameObject MySpellPrefab{ 
        get{
            return spellPrefab;
        } 
    }

    //Property for reading the color of the spell
    public Color MybarColor{
        get{
            return barColor;
        }
    }

    public void Use(){
        
        Player.MyInstance.CastSpell(MyName);
    } 
}   

