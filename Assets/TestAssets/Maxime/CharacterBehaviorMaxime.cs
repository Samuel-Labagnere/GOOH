using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBehaviorMaxime : MonoBehaviour
{
    private GameObject character;
    private Rigidbody2D rb2D;
    [SerializeField] private float speed;
    private PlayerInput pInput;
    private Vector2 move;
    private InputAction moveAction;
    private InputAction flashlightAction;

    void Start()
    {
        character = gameObject;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        pInput = gameObject.GetComponent<PlayerInput>();
        moveAction = pInput.actions.FindAction("Move");
        flashlightAction = pInput.actions.FindAction("Flashlight");
        moveAction.Enable();
        flashlightAction.Enable();
    }

    public void Move(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void Flashlight(InputAction.CallbackContext context){
        // Get Flashlight Key
    }

    public void Interact(InputAction.CallbackContext context){
        // Get Interact Key
    }

    void FixedUpdate(){
        // CONTROLS
        Vector2 newPos = rb2D.position;

            if (move.x < 0f)
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.left;
            }
            if (move.x > 0f)
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.right;
            }
            if (move.y > 0f)
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.up;
            }
            if (move.y < 0f)
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.down;
            }
            rb2D.MovePosition(newPos);
    }

    void Update()
    {
        
    }
}