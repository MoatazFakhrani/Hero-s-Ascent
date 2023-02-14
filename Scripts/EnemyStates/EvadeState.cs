using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState{

    private Enemy parent;

    public void Enter(Enemy parent){

        this.parent = parent;
    }

    public void Exit(){

        parent.Direction = Vector2.zero;
        parent.Reset();
    }

    public void Update(){

        //calculate the direction of the enemy between the starting point and the current position
        parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized;
        
        //Measure the distance between the starting point and the current position of the enemy
        float distance = Vector2.Distance(parent.MyStartPosition, parent.transform.position);

        //If the enemy went back to his start point position -> then
        if(distance <= 0.1f){

            //Change to Idle state
            parent.ChangeState(new IdleState());
        }
    }
}
