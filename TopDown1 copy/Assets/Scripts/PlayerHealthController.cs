using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;


    CharacterController charCon;
    public int health = 10;
    public float maxhealth = 10.5f;
    public float invinceFrames = 1.5f;
    public float knockBack = 1.5f;
    float timeSinceLastHit;
    public Transform damageParticles;

    public Image healthBar;     

    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        timeSinceLastHit += Time.deltaTime;
    }
    public void takeDamage(int damage, Transform source, float knockBack)
    {
        health -= damage;
        Vector3 knockBackVector = transform.position - source.position;
        knockBackVector.Normalize();
        charCon.Move(knockBackVector*knockBack);
        Instantiate(damageParticles, transform.position, Quaternion.identity);

        healthBar.fillAmount = health / maxhealth;
        if(health <= 0)
        {
            handleDeath();
        }
    }
    void handleDeath()
    {
        levelManager.instance.deathScreen.SetActive(true);
        levelManager.instance.livingMap.SetActive(false);
        levelManager.instance.deathMap.SetActive(true);
        Destroy(gameObject);
        

    }
}
