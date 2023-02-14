using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour{
    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float speed; 
    private Transform target;
    public Transform MyTargat{get; private set;}
    private Transform source;
    private int damage;

    // Start is called before the first frame update
    void Start(){

        myRigidBody = GetComponent<Rigidbody2D>(); 
    }

    public void Initialize(Transform target, int damage, Transform source){

        this.MyTargat = target;
        this.damage = damage;
        this.source = source;
    }

    private void FixedUpdate(){

        if(MyTargat != null){
            Vector2 direction = MyTargat.position - transform.position;
            myRigidBody.velocity = direction.normalized * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.tag == "HitBox" && collision.transform == MyTargat){
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTargat = null;
        }
    }
}
