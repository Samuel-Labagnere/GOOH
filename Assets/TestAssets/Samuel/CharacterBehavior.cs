using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBehavior : MonoBehaviour
{
    // GAMEOBJECTS
    private GameObject character;
    private GameObject flashlight;

    // INPUTACTIONS
    private PlayerInput pInput;
    private InputAction moveAction;
    private InputAction flashlightAction;
    private InputAction interactAction;
    private InputAction sprintAction;

    // MOVEMENT
    private Rigidbody2D rb2D;
    private bool isAbleToSprint;
    private Vector2 move;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintDuration;
    [SerializeField] private float sprintCooldown;

    // BATTERY
    [SerializeField] private float battery;
    [SerializeField] private float batteryMax;
    [SerializeField] private float batterySpeed;
    [SerializeField] private float newBattery;

    void Start()
    {
        // SET VARS
        character = gameObject;
        flashlight = character.transform.GetChild(0).gameObject;
        flashlight.SetActive(false);

        // battery = batteryMax;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        pInput = gameObject.GetComponent<PlayerInput>();

        // INPUTACTIONS
        moveAction = pInput.actions.FindAction("Move");
        flashlightAction = pInput.actions.FindAction("Flashlight");
        interactAction = pInput.actions.FindAction("Interact");
        sprintAction = pInput.actions.FindAction("Sprint");
        moveAction.Enable();
        flashlightAction.Enable();
        interactAction.Enable();
        sprintAction.Enable();
    }

    public void Move(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void Flashlight(InputAction.CallbackContext context){
        if(flashlightAction.triggered){
            if(!flashlight.activeSelf && battery > 0){
                flashlight.SetActive(true);
            }else{
                flashlight.SetActive(false);
            }
        }
    }

    public void Interact(InputAction.CallbackContext context){
        if(interactAction.triggered){
            Debug.Log("Interact!");
        }
    }

    public void Sprint(InputAction.CallbackContext context){
        if(sprintAction.triggered){
            StartCoroutine("SprintCoroutine");
        }
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

    void Update(){
        // FLASHLIGHT
        if(flashlight.activeSelf){
            battery -= Time.deltaTime * batterySpeed;
            if(battery <= 0){
                battery = 0;
                flashlight.SetActive(false);
            }
        }
        if(battery > batteryMax){
            battery = batteryMax;
        }

        // SPRINT
        if(sprintCooldown > 0){
            isAbleToSprint = false;
            sprintCooldown -= Time.deltaTime;
        }else if(sprintCooldown <= 0){
            isAbleToSprint = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        // GAIN BATTERY
        if(col.tag == "Battery"){
            battery += newBattery;
            Destroy(col.gameObject);
        }
    }

    IEnumerator SprintCoroutine(){
        if(isAbleToSprint){
            sprintCooldown = 5f + sprintDuration;
            speed += sprintSpeed;
            yield return new WaitForSeconds(sprintDuration);
            speed -= sprintSpeed;
        }
    }
}
