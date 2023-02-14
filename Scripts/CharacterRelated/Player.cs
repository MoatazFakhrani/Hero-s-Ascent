using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Character{   

    //Singleton to use its instances from other places
    private static Player instance;

    public static Player MyInstance{
        get{
            if(instance == null){
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    
    private List<Enemy> attackers = new List<Enemy>();

    public List<Enemy> MyAttackers{
        get{
            return attackers;
        }
        set{
            attackers = value;
        }
    }

    [SerializeField]
    private TMP_Text SpawningSoon;

    [SerializeField]
    //Array of blocks used to prevent the player from attacking on wrong sights
    private Block[] blocks;

    [SerializeField]
    //Exitpoints for the spells
    private Transform[] exitPoints;

    //Index Tracks which exitpoint to use
    private int exitIndex = 2;

    private IInteractable interactable;

    private SpellBook spellBook;

    private Vector3 min;
    private Vector3 max;
    
    // Start is called before the first frame update
    protected override void Start(){
       
       spellBook = GetComponent<SpellBook>();
       base.Start();
    }

    //Update is called once per frame
    protected override void Update(){   

        //Call GetInpt Function
        GetInpt();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
        Mathf.Clamp(transform.position.y, min.y, max.y), 
        transform.position.z);

        //Access The Abstract Class and Rn Update
        base.Update(); 
    }
    
    //Inactive
    public void Interact(){

        if(interactable != null){
            interactable.Interact();
        }
    }

    //Inactive
    public void OnTriggerEnter2D(Collider2D collision){
        
        if(collision.tag == "Interactable"){
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    //Inactive
     public void OnTriggerExit2D(Collider2D collision){
        
        if(collision.tag == "Interactable"){
            if(interactable != null)
            {
                interactable.StopInteract();
                interactable = null;
            }
        }
    }

    //Player Move Method Configurrations
    private void GetInpt(){

        Direction = Vector2.zero;

        //Move Up
        if(Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"])){
            exitIndex = 0;
            Direction += Vector2.up;
        }

        //Move Left
         if(Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"])){
            exitIndex = 3;
            Direction += Vector2.left;
        }

        //Move Down
         if(Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"])){
            exitIndex = 2;
            Direction += Vector2.down;
        }

        //Move Right
         if(Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"])){
            exitIndex = 1;
            Direction += Vector2.right;
        }

        //If the player is moving interrupt the attack
        if(IsMoving){
            StopAttack();
        }

        //For all the keys saved in the keybind manager -> 
        foreach(string action in KeybindManager.MyInstance.ActionBinds.Keys){

            //Check if one of then is pressed
            if(Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action])){
                UIManager.MyInstance.ClickActionButton(action);
            }
        }
    }

    //Set Limits for the player so he can't leave the world of the game
    public void SetLimits(Vector3 min, Vector3 max){

        //Minimum position of the player
        this.min = min;

         //Maximum position of the player
        this.max = max;
    }

    private IEnumerator Attack(string spellName){

        Transform currentTarget = MyTarget;

        //Create new spell to use it in the game
        Spell newSpell = spellBook.CastSpell(spellName);

        //Check if we are Attacking
        IsAttacking = true;

        //Start the Animations of Attaking
        MyAnimator.SetBool("attack", IsAttacking);
        yield return new WaitForSeconds(newSpell.MyCastTime);

        if(currentTarget != null && InLineOfSight()){
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget, newSpell.MyDamage, transform);
        }
        StopAttack();
    }

    public void CastSpell(string spellName){

        Block();
            if(MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight()){
                attackRoutine = StartCoroutine(Attack(spellName));
            }
    }

    //Check if the target is in line of sight
    private bool InLineOfSight(){

        if(MyTarget != null){

            //Target's Direction
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            //Throw Raycast in the Direction of the target/ Blocks don't the cast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            //If the target being attacked is on the sight of the player
            if(hit.collider == null){
                return true;
            }
        }
            //If the target being attacked is not the sight of the player/ Blocks prevent the cast
            return false;
    }

    //Change the blocks based on player's direction
    private void Block(){

        foreach (Block b in blocks){  

            b.Deactivate();
        }
        blocks[exitIndex].Activate();
    }

    //Stop Attacking Function
    public void StopAttack(){

        //Stop casting 
        spellBook.StopCasting();

        //Set attacking to false
        IsAttacking = false;

        //Stop Attack Animation
        MyAnimator.SetBool("attack", IsAttacking);
        
        //Check if there is a reference to an attack coroutine
        if(attackRoutine != null){

            StopCoroutine(attackRoutine);
        } 
    }

    public IEnumerator Respawn(){
        SpawningSoon.text = "YOU DIED! SPAWNING SOON..";
        MySpriteRenderer.enabled = false;
        yield return new WaitForSeconds(5f);
        health.Initialize(initHealth, initHealth);
        SpawningSoon.text = "";
        MySpriteRenderer.enabled = true;
        MyAnimator.SetTrigger("respawn");
        MainMenu.MyInstance.OnPlayerDeath();
    }

    //Used to target the enemy with Tab key(contain/specify enemies)
    public void AddAttacker(Enemy enemy){

        if(!MyAttackers.Contains(enemy)){

            MyAttackers.Add(enemy);
        }
    }

    private bool isMuted = false;
    private void ToggleMute(){
        isMuted = ! isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }
}