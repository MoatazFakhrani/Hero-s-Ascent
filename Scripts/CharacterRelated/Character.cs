using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Character class is an abstract that all characters need to inhereit from as: Enemy, Player
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour{

    //Serialiaze is used to make a reference to it from Unity
    [SerializeField]
    //Character's movement speed
    private float speed;

    //Reference to charachter's Animator
    public Animator MyAnimator { get; set; }

    //Player's Direction
    private Vector2 direction;

    private SpriteRenderer mySpriteRenderer;

    //Rigidbody of the character
    private Rigidbody2D myRigidbody;

    //Check if the charachter is attacking
    public bool IsAttacking { get; set; }

    //Reference to attack coroutine
    protected Coroutine attackRoutine;

    [SerializeField]
    //To Get hitbox's position
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    //Property to access the target
    public Transform MyTarget{ get; set; }

    public Stat MyHealth{
        get{
            return health;
        }
    }

    [SerializeField]
    //Character's Start health
    public float initHealth;

    //Check if the character is moving 
    public bool IsMoving{
        get{
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    //Create Property of Direction and make it public to access it from other classes like: FollowState
    public Vector2 Direction{ 
        get {
            return direction;
        }  
        set{
            direction = value;
        } 
    }

    //Create Property of Speed and make it public to access it from other classes like: FollowState
    public float Speed{ 
        get{
            return speed;
        } 
        set{
            speed = value; 
        } 
    }

    public bool IsAlive{   
        get{
           return health.MyCurrentValue > 0;
        }
    }

    public SpriteRenderer MySpriteRenderer{
        get {
            return mySpriteRenderer;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start(){
        health.Initialize(initHealth, initHealth);

        //Reference to Animator
        MyAnimator = GetComponent<Animator>();

        //Reference to Rigidbody2D
        myRigidbody = GetComponent<Rigidbody2D>();

        //Reference to SpriteRenderer
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update method is virtual so it can be OVERRIDED to the subclasses
    protected virtual void Update(){
        HandleLayers();
    }

    private void FixedUpdate(){
        Move();
    }

    //Movement Equation
    public void Move(){
        //If the character is alive -> then
        if(IsAlive){

            //Characters can move
            myRigidbody.velocity = Direction.normalized * Speed;
        }
    }

    //Manage the layes by playing the right animator on a specific state
    public void HandleLayers(){
        if(IsAlive){
            //Check if the charcters is moving -> then
            if(IsMoving){
                //Activate Walks Animations if the character is walking
                ActivateLayer("WalkLayer");
        
                //Set the animation parameter so that the player faces to the correct direction
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y); 
            }
            //Activate Attacks Animations if the character is Attacking
            else if(IsAttacking){
                ActivateLayer("AttackLayer");
            }
            //Activate Idle Animations if the character is Standing or not doing anything
            else{
                ActivateLayer("IdleLayer");
            }
        }
        else{
            //Activate death layer if the character is not alive (dead)
            ActivateLayer("DeathLayer");
        }  
    }

    //Activate an Animation layer based on a string
    public void ActivateLayer(string layerName){
        for (int i = 0; i < MyAnimator.layerCount; i++){
            MyAnimator.SetLayerWeight(i,0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void TakeDamage(float damage, Transform source){

        health.MyCurrentValue -= damage;

        //If the health equals to 0 or less -> then
        if(health.MyCurrentValue <= 0){

            //Die
            //if the character is dead he shouldn't be able to move (by setting vector2 to 0)
            Direction = Vector2.zero;
            myRigidbody.velocity = Direction;
            MyAnimator.SetTrigger("die");
        }
    }
}
