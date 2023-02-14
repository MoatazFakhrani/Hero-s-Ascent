using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

 class FollowState : IState{

    //Reference to the parent
    private Enemy parent;

    //This state is called when we enter a specific state
    public void Enter(Enemy parent){

        Player.MyInstance.AddAttacker(parent);
        this.parent = parent;
    }

    //This state is called when we exit a specific state
    public void Exit(){
        
        //The enemy stops moving if there is no target within the range (ANYMORE) and start idling again
        parent.Direction = Vector2.zero;
    }

    public void Update(){
        
        //If there is a targer -> then
        if(parent.MyTarget != null){

            //Find Target's Direction
            parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;

            //Enemy Chases the Target
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyTarget.position, parent.Speed * Time.deltaTime);

            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            if(distance <= parent.MyAttackRange){
                parent.ChangeState(new AttackState());
            }
        }
        
        if(!parent.InRange){
            //If there is no target within the range of the enemy then change the state to Evade
            parent.ChangeState(new EvadeState());
        }
    }
}
