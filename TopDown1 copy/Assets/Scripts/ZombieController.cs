using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieController : MonoBehaviour
{
    public Transform target;
    Vector3 currentMove;
    public float moveSpeed = 1;
    CharacterController charCon;
    public AudioSource sound;

    public float chaseDistance;
    float distance;
    bool chasing = false;
    public float attackRange = 1.5f;

    public Transform damageParticles;
    public Transform damageNumber;

    //
    public float knockBack =1 ;
    public int damage = 1;
    public int health;
    //public AudioSource deathSound;



    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        charCon.Move(currentMove * Time.deltaTime);
        handleGravity();
        if(chasing && target != null)
        {
            handleMovement();
            dmgRange();
        }
        else
        {
            handleChase();
        }
    }
    void handleMovement()
    {
        Vector3 direction = getDirection();
        //change current movespeed based on direction * speed.
        currentMove.x = direction.x * moveSpeed;

        currentMove.z = direction.z * moveSpeed;
    }
    
    Vector3 getDirection()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        return direction;

        //change current move based on direction * speed
    }
    void handleGravity()
    {
        currentMove.y = -0.5f;
    }

    void handleChase()
    {
        distance = Vector3.Distance(target.position, transform.position);
        Ray ray = new Ray(transform.position, getDirection()*chaseDistance);
        Debug.DrawRay(transform.position, getDirection()*chaseDistance,Color.red);
        if(distance <= chaseDistance)
        {
            if(Physics.Raycast(ray, out RaycastHit hitData))
            {
                if(hitData.transform.gameObject.CompareTag("Player"))
                {
                    startChase();
                }
            }
        }
    }
    void startChase()
    {
        //Audio play
        //setup variables
        sound.Play();
        chasing = true;
    }
    public void takeDamage(int damage, float knockBack, Transform source)
    {
        health = health - damage;

        Vector3 knockBackMove = transform.position - source.position;
        knockBackMove.Normalize(); 
        charCon.Move(knockBackMove*knockBack);
        Instantiate(damageParticles, transform.position, Quaternion.identity);
        Instantiate(damageNumber,transform.position,Quaternion.identity).GetComponent<PopUp>().setup(damage, source.position); 
        //Source.position turns source (a transform) into a vector 3 By focussing on its positional values (transforms are Vector3's & Quaternions at the same time)
        if(health<=0)
        {
            handleDeath();
        }
        //hurt sound
        //amount of dmg we take
        //dies

    }
    private void handleDeath()
    {
        //hurtsound
        Destroy(gameObject);
    }
    void dealDamage()
    {
        target.GetComponent<PlayerHealthController>().takeDamage(damage, transform, knockBack);
    }
    void dmgRange()
    {
        distance = Vector3.Distance(target.position, transform.position);
        if(distance < attackRange)
        {
            dealDamage(); 
        }
    }
}
