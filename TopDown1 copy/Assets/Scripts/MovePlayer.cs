using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovePlayer : MonoBehaviour
{
    public float gravity = 1.5f;
    Vector2 direction;
    Vector3 currentMove;
    public Transform lookAtTarget;
    PlayerController pc;
    CharacterController charCon;
    float moveSpeed = 5.5f;
    public gun Gun;
    public bool canMove;

    bool shootDown;

    public static MovePlayer instance;
    void Awake()
    {
        pc = new PlayerController();
        pc.InGame.Movement.started += handleMovement;
        pc.InGame.Movement.performed += handleMovement;
        pc.InGame.Movement.canceled += handleMovement;

        pc.InGame.Shoot.started += handleShoot;
        pc.InGame.Shoot.canceled += handleShoot;
        instance = this;

    }

    void OnEnable()
    {
        pc.InGame.Enable();
    }

    void OnDisable(){
        pc.InGame.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        
        charCon.Move(currentMove * Time.deltaTime);

        if(shootDown)
        {
            Gun.Shoot();
        } 
    }
    
    void handleMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        
        
        currentMove.x = direction.x * moveSpeed;
        currentMove.z = direction.y * moveSpeed;
    }

    void handleShoot(InputAction.CallbackContext context)
    {
        shootDown = context.ReadValueAsButton();
    }

    void handleRotation()
    {
        transform.LookAt(lookAtTarget);
    }
  
}
