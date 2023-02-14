using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour{

    [SerializeField]
    private Player player;
    private NPC currentTarget;
    private int targetIndex;

    // Update is called once per frame
    void Update(){
        ClickTarget();
        NextTarget();
    }

    private void ClickTarget(){

        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero, Mathf.Infinity, 512);

            if(hit.collider != null){

                DeSelectTarget();
                SelectTarget(hit.collider.GetComponent<Enemy>());
            }
            else{

                UIManager.MyInstance.HideTargetFrame();
                DeSelectTarget();
                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()){

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero, Mathf.Infinity, 512);

            if(hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable")){

                //hit.collider.GetComponent<NPC>().Interact();
                player.Interact();
            }
        }
    }

    private void NextTarget(){
        if(Input.GetKeyDown(KeyCode.Tab)){

            DeSelectTarget();
            
            if(Player.MyInstance.MyAttackers.Count > 0){
                SelectTarget(Player.MyInstance.MyAttackers[targetIndex]);
                targetIndex++;

                if(targetIndex >= Player.MyInstance.MyAttackers.Count){
                    targetIndex = 0;
                }
            }
        }
    }
    
    private void SelectTarget(Enemy enemy){

        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UIManager.MyInstance.ShowTargetFrame(currentTarget);
    }

    private void DeSelectTarget(){

        if(currentTarget != null){

            currentTarget.DeSelect();
        }
    }
}
