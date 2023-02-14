using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC{

    [SerializeField]
    //Canvasgroup for the healthbar
    private CanvasGroup healthGroup;

    //create reference to the interface IState
    private IState currentState;

    [SerializeField]
    private int damage;

    //for animations reset damage
    private bool canDoDamage = true;

    [SerializeField]
    private float attackRange;

    //Enemy's attack range
    public float MyAttackRange{ 
        get{
            return attackRange;
        } 
        set {
            attackRange = value;
        }
    }

    public float MyAttackTime{ get; set; }

    public Vector3 MyStartPosition{ get; set; }

    [SerializeField]
    private float initialAggroRange;
    
    public float MyAggroRange{ get; set; }

    public bool InRange{
        get{
            //If the distance between the enemy and player is less than Aggro Range then it means they are in range
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    protected void Awake(){
        MyStartPosition = transform.position;

        //Change the state into idle state from the start
        ChangeState(new IdleState());
    }

    protected override void Update(){

        if(IsAlive){
           
            if(!IsAttacking){
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();
            }

            if(MyTarget != null && !Player.MyInstance.IsAlive){

                ChangeState(new EvadeState());
            }

            if(!IsAlive){
            
                Destroy(gameObject, 2);
            }
            base.Update();
        }
        

    //When the Enemy is Selected
    public override Transform Select(){

        if(IsAlive){
        //Show the Healthbar of the enemy
        healthGroup.alpha = 1;
        }
        return base.Select();
    }

    //When the Enemy is Deselected
    public override void DeSelect(){

        if(!IsAlive){
            //Hide the Healthbar of the enemy
            healthGroup.alpha = 0;  
        }
        base.DeSelect();
    }

    //Enemy Takes Damage when Gets Hit
    public override void TakeDamage(float damage, Transform source){

        //If the current state is not Evade -> then
        if(!(currentState is EvadeState)){

            //Take Damage
            SetTarget(source);
            base.TakeDamage(damage, source);
            OnHealthChanged(health.MyCurrentValue);
        }
    }

    public void DoDamage(){

        if(canDoDamage){
            Player.MyInstance.TakeDamage(damage, transform);
            canDoDamage = false;
        }
    }

    public void CanDoDamage(){
        canDoDamage = true;
    }
   
    //Change Enemy's State
   public void ChangeState(IState newState){

        //If there is a state -> then
        if(currentState != null){

            //call exit state
            currentState.Exit();
        }

        //Set the new state
        currentState = newState;

        //Call Enter of the new state
        currentState.Enter(this);
   }

   public void SetTarget(Transform target){
        if(MyTarget == null && !(currentState is EvadeState)){
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initialAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
   }

   public void Reset(){

        this.MyTarget = null;

        //Reset Aggro Range
        this.MyAggroRange = initialAggroRange;

        //Reset the actual health
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;

        //Reset health on the unit frame
        OnHealthChanged(health.MyCurrentValue);
    }

    public override void Interact(){
        
        if(!IsAlive){
            Player.MyInstance.MyAttackers.Remove(this);
        }
    } 
}