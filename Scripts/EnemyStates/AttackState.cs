using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState{

    //Reference to parent's state
    private Enemy parent;
    
    private float attackCooldown = 1;

    private float extraRange = 0.3f;

    //State's constructor
    public void Enter(Enemy parent){
        this.parent = parent;
    }
    
    public void Exit(){

    }

    public void Update(){

        if(parent.MyAttackTime >= attackCooldown && !parent.IsAttacking){

            parent.MyAttackTime = 0;
            parent.StartCoroutine(Attack());
        }
        
        //if there is a target -> then
        if(parent.MyTarget != null){

            //The distance between the target and the enemy
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position); 

            //if the distance is larget than the attack range -> then
            if(distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking){

                //Enemy follows the player again
                parent.ChangeState(new FollowState());
            }
            //Check range and attack
        }
        else{

            //Start Idling -> If the player gets out of range while the enemy following attacking him 
            parent.ChangeState(new IdleState());
        }
    } 

    public IEnumerator Attack(){

        parent.IsAttacking = true;
        parent.MyAnimator.SetTrigger("attack");

        //Number2 is the index of attack layer
        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);
        parent.IsAttacking = false;
    }
}
