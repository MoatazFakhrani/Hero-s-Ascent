using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : CollidableObject{

    private bool z_Interacted = false;
    public Animator myAnim;

    protected override void Oncollided(GameObject collidedObject){

        if (Input.GetKey(KeyCode.E)){
            OnInteract();
        }
    }

    protected virtual void OnInteract(){

        if (!z_Interacted){

            myAnim = GetComponent<Animator>();
            myAnim.Play("Open_Chest");
            z_Interacted = true;
            InventoryScript.MyInstance.AddPotion();
        }
    }
}