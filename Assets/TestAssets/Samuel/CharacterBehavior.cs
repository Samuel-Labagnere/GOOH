using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField] private AudioSource music;

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

    // ANIMATOR
    [SerializeField] private Animator animCharacter;

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
    [SerializeField] private AudioSource flashlightToggleSound;
    [SerializeField] private AudioSource batteryMinusSound;
    [SerializeField] private AudioSource batteryPlusSound;
    [SerializeField] private AudioSource flashlightOffSound;
    [SerializeField] private AudioSource batteryEmptySound;
    private int nBars;

    // COIN
    [SerializeField] private int score;
    [SerializeField] private int coinValue;
    [SerializeField] private Text scoreTxtObject;
    [SerializeField] private string scoreText;
    [SerializeField] private AudioSource coinSound;

    // CHANGE ROOM
    private bool level1TpStepped = false;
    [SerializeField] private GameObject level1Spawner;
    public bool isOnLevel1;
    public bool isOnLevel2;
    public bool isOnLevel3;

    // TEXTS
    [SerializeField] private Text levelText;
    [SerializeField] private Text interactText;

    void Start()
    {
        // SET VARS
        character = gameObject;
        flashlight = character.transform.GetChild(0).gameObject;
        flashlight.SetActive(false);
        newBattery = batteryMax/3;
        isOnLevel1 = false;

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

        // TEXTS
        levelText.text = "";
        interactText.text = "";
    }

    public void Move(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void Flashlight(InputAction.CallbackContext context){
        if(flashlightAction.triggered){
            if(battery <= 0){
                batteryEmptySound.Play();
            }else{
                if(!flashlight.activeSelf){
                    flashlight.SetActive(true);
                    flashlightToggleSound.Play();
                    lightCircle.transform.localScale = new Vector3(lightCircleModifiedScale, lightCircleModifiedScale, 0f);
                }else{
                    flashlight.SetActive(false);
                    flashlightToggleSound.Play();
                    lightCircle.transform.localScale = new Vector3(lightCircleBaseScale, lightCircleBaseScale, 0f);
                }
            }
        }
    }

    public void Interact(InputAction.CallbackContext context){
        if(interactAction.triggered){
            // Debug.Log("Interact!");
            if(level1TpStepped){
                rb2D.AddForce(new Vector2(0f, 0f));
                character.transform.position = level1Spawner.transform.position;
                level1TpStepped = false;
            }
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

        animCharacter.SetFloat("MoveY", move.y);
        animCharacter.SetFloat("MoveX", move.x);

            if (move.x < 0f) // LEFT
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.left;
                animCharacter.SetTrigger("Left");
            }
            if (move.x > 0f) // RIGHT
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.right;
                animCharacter.SetTrigger("Right");
            }
            if (move.y > 0f) // UP
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.up;
                animCharacter.SetTrigger("Up");
            }
            if (move.y < 0f) // DOWN
            {
                newPos += Time.fixedDeltaTime * speed * Vector2.down;
                animCharacter.SetTrigger("Down");
            }

            if(newPos != rb2D.position && !walkSound.isPlaying){
                walkSound.Play();
            }
            rb2D.MovePosition(newPos);
    }

    void Update(){
        // SOUNDS
        walkSound.volume = PlayerPrefs.GetFloat("volume");
        flashlightToggleSound.volume = PlayerPrefs.GetFloat("volume");
        batteryMinusSound.volume = PlayerPrefs.GetFloat("volume");
        batteryPlusSound.volume = PlayerPrefs.GetFloat("volume");
        flashlightOffSound.volume = PlayerPrefs.GetFloat("volume");
        batteryEmptySound.volume = PlayerPrefs.GetFloat("volume") / 4f;
        coinSound.volume = PlayerPrefs.GetFloat("volume");
        music.volume = PlayerPrefs.GetFloat("volume");
        
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

        int previousNBars = nBars;

        if(battery >= (batteryMax/3)*2){
            flashlightBars.sprite = flashlight3Bars;
            nBars = 3;
        }else if(battery >= (batteryMax/3)){
            flashlightBars.sprite = flashlight2Bars;
            nBars = 2;
        }else if(battery < (batteryMax/3) && battery > 0){
            flashlightBars.sprite = flashlight1Bar;
            nBars = 1;
        }else{
            flashlightBars.sprite = flashlight0Bar;
            nBars = 0;
        }

        if(previousNBars > nBars){
            if(nBars == 0){
                flashlightOffSound.Play();
            }else{
                batteryMinusSound.Play();
            }
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
            batteryPlusSound.Play();
            Destroy(col.gameObject);
        }else if(col.tag == "Coin"){
            coinSound.Play();
            score += coinValue;
            scoreTxtObject.text = scoreText + score.ToString();
            Destroy(col.gameObject);
        }

        // CHANGE ROOM
        if(col.name == "Level1TP"){
            level1TpStepped = true;
            levelText.text = "<To level 1>";
            interactText.text = "[Press " + InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null) + " to interact]";
        }
        if(col.tag == "Level1"){
            isOnLevel1 = true;
        }
        if(col.tag == "Level2"){
            isOnLevel2 = true;
        }
        if(col.tag == "Level3"){
            isOnLevel3 = true;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        // CHANGE ROOM
        if(col.name == "Level1TP"){
            level1TpStepped = false;
            levelText.text = "";
            interactText.text = "";
        }
        if(col.tag == "Level1"){
            isOnLevel1 = false;
        }
        if(col.tag == "Level2"){
            isOnLevel2 = false;
        }
        if(col.tag == "Level3"){
            isOnLevel3 = false;
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
