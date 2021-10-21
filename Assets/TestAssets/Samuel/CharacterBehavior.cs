using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float speed;
    private Vector2 move;
    private InputAction moveAction;

    void Start()
    {
        // character = gameObject;
        // rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Move(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();

        Debug.Log(move.x);
        Debug.Log(move.y);
    }

    void FixedUpdate(){
        // CONTROLS
        Vector2 newPos = rb2D.position;

            if (move.x < 0f)
            {
                Debug.Log("hello");
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
