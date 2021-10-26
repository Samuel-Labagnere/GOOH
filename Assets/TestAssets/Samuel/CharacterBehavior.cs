using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] private AudioSource walkSound;
    private float sprintCldn;

    // BATTERY
    [SerializeField] private float battery;
    [SerializeField] private float batteryMax;
    [SerializeField] private float batterySpeed;
    private float newBattery;
    [SerializeField] private SpriteRenderer flashlightBars;
    [SerializeField] private Sprite flashlight3Bars;
    [SerializeField] private Sprite flashlight2Bars;
    [SerializeField] private Sprite flashlight1Bar;
    [SerializeField] private Sprite flashlight0Bar;
    [SerializeField] private GameObject lightCircle;
    [SerializeField] private float lightCircleBaseScale;
    [SerializeField] private float lightCircleModifiedScale;

    // COIN
    [SerializeField] private int score;
    [SerializeField] private int coinValue;
    [SerializeField] private Text scoreTxtObject;
    [SerializeField] private string scoreText;

    void Start()
    {
        // SET VARS
        character = gameObject;
        flashlight = character.transform.GetChild(0).gameObject;
        flashlight.SetActive(false);
        newBattery = batteryMax/3;

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        pInput = gameObject.GetComponent<PlayerInput>();
        lightCircle.transform.localScale = new Vector3(lightCircleBaseScale, lightCircleBaseScale, 0f);

        // COIN
        scoreTxtObject.text = scoreText + score.ToString();

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
                lightCircle.transform.localScale = new Vector3(lightCircleModifiedScale, lightCircleModifiedScale, 0f);
            }else{
                flashlight.SetActive(false);
                lightCircle.transform.localScale = new Vector3(lightCircleBaseScale, lightCircleBaseScale, 0f);
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

            if (move.x < 0f) // LEFT
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.left;
            }
            if (move.x > 0f) // RIGHT
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.right;
            }
            if (move.y > 0f) // UP
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.up;
            }
            if (move.y < 0f) // DOWN
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.down;
            }

            if(newPos != rb2D.position && !walkSound.isPlaying){
                walkSound.Play();
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
                lightCircle.transform.localScale = new Vector3(lightCircleBaseScale, lightCircleBaseScale, 0f);
            }
        }
        if(battery > batteryMax){
            battery = batteryMax;
        }

        if(battery >= (batteryMax/3)*2){
            flashlightBars.sprite = flashlight3Bars;
        }else if(battery >= (batteryMax/3)){
            flashlightBars.sprite = flashlight2Bars;
        }else if(battery < (batteryMax/3) && battery > 0){
            flashlightBars.sprite = flashlight1Bar;
        }else{
            flashlightBars.sprite = flashlight0Bar;
        }

        // SPRINT
        if(sprintCldn > 0){
            isAbleToSprint = false;
            sprintCldn -= Time.deltaTime;
        }else if(sprintCldn <= 0){
            isAbleToSprint = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        // GAIN BATTERY
        if(col.tag == "Battery"){
            battery += newBattery;
            Destroy(col.gameObject);
        }else if(col.tag == "Coin"){
            score += coinValue;
            scoreTxtObject.text = scoreText + score.ToString();
            Destroy(col.gameObject);
        }
    }

    IEnumerator SprintCoroutine(){
        if(isAbleToSprint){
            sprintCldn = sprintCooldown + sprintDuration;
            speed += sprintSpeed;
            yield return new WaitForSeconds(sprintDuration);
            speed -= sprintSpeed;
        }
    }
}
