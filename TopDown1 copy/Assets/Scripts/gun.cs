using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    Transform origin;
    Transform player;
    public float range;
    public int minDamage; 
    public int maxDamage;
    public float shootDelay; 
    float timeOfLastShot;
    public float knockBack;
    public Transform target;
    Vector3 hitLocation;
    Vector3 hitPosition;
    public bool canShoot;

    public TrailRenderer bulletTrail;
    public static gun instance;

    
    public AudioSource gunSound;
    
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.GetChild(0);
        player = transform.parent;
        timeOfLastShot = Time.time;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if(timeOfLastShot +shootDelay < Time.time)
        {
            timeOfLastShot= Time.time;
            
            gunSound.Play();

        Vector3 directionOfShot = getDirection();

        Ray ray = new Ray(origin.position, directionOfShot * range);
        if(Physics.Raycast(ray, out RaycastHit hitData, range))
        {//hits something
            Vector3 hitLocation = hitData.point;

            TrailRenderer trail = Instantiate(bulletTrail, origin.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitLocation));
            if(hitData.transform.CompareTag("Enemy"))
            {
                int damage = Random.Range(minDamage,maxDamage+1);
                hitData.transform.GetComponent<ZombieController>().takeDamage(damage,knockBack,player);
            }
            Debug.DrawRay(origin.position, directionOfShot * hitData.distance, Color.green, 0.5f);
        }
        else
        {//hits nothing
            Vector3 hitLocation = origin.position + (directionOfShot*range);

            TrailRenderer trail = Instantiate(bulletTrail, origin.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitLocation));
            Debug.DrawRay(origin.position, directionOfShot * range, Color.green, 0.5f);
        }
        }
        
    }
    Vector3 getDirection()
    {
        Vector3 direction = target.position - origin.position;
        direction.y = 0;
        direction.Normalize();
        return direction;

    }
    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPosition)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while(time<1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hitPosition, time);
            time+= Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hitPosition;
        Destroy(trail.gameObject, trail.time);
    }
}
