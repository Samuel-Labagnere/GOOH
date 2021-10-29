using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int coinsValue;
    [SerializeField] private Text scoreTxtObject;
    [SerializeField] private string scoreText;
    [SerializeField] private AudioSource coinSound;

    // CHANGE ROOM
    [SerializeField] private AudioSource doorOpen;
    [SerializeField] private AudioSource doorClose;
    [SerializeField] private AudioSource doorLock;
    [SerializeField] private AudioSource enemyLaugh;
    private bool level1TpStepped = false;
    private bool level2TpStepped = false;
    private bool level3TpStepped = false;
    private bool level1TpBackStepped = false;
    private bool level2TpBackStepped = false;
    private bool level3TpBackStepped = false;
    public bool level1Done = false;
    public bool level2Done = false;
    public bool level3Done = false;
    [SerializeField] private GameObject level1Spawner;
    [SerializeField] private GameObject level2Spawner;
    [SerializeField] private GameObject level3Spawner;
    [SerializeField] private GameObject hallSpawnerFromLevel1;
    [SerializeField] private GameObject hallSpawnerFromLevel2;
    [SerializeField] private GameObject hallSpawnerFromLevel3;
    public bool isOnLevel1;
    public bool isOnLevel2;
    public bool isOnLevel3;

    // TEXTS
    [SerializeField] private Text levelText;
    [SerializeField] private Text interactText;

    // SPRITES
    private SpriteRenderer characterSprite;
    [SerializeField] private Sprite upSprite1;
    [SerializeField] private Sprite upSprite2;
    [SerializeField] private Sprite upSprite3;
    [SerializeField] private Sprite downSprite1;
    [SerializeField] private Sprite downSprite2;
    [SerializeField] private Sprite downSprite3;
    [SerializeField] private Sprite leftSprite1;
    [SerializeField] private Sprite leftSprite2;
    [SerializeField] private Sprite rightSprite1;
    [SerializeField] private Sprite rightSprite2;

    // DEATH
    private bool isAbleToMove = true;
    public bool isDead = false;
    [SerializeField] private AudioSource die;
    [SerializeField] private RawImage goBackground;
    [SerializeField] private SpriteRenderer goBiteTop;
    [SerializeField] private SpriteRenderer goBiteBottom;
    [SerializeField] private RawImage goBiteTopBackground;
    [SerializeField] private RawImage goBiteBottomBackground;
    [SerializeField] private SpriteRenderer goGhost;
    [SerializeField] private Text goText;
    [SerializeField] private Text goRestartText;
    [SerializeField] private Text goQuitText;
    [SerializeField] private SpriteRenderer goSelect;
    [SerializeField] private AudioSource goSelectSound;
    [SerializeField] private AudioSource goConfirmSound;
    private bool goDone = false;
    private int whereSelect = 1;

    void Start()
    {
        // SET VARS
        character = gameObject;
        characterSprite = character.GetComponent<SpriteRenderer>();
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

        // DEATH
        goBackground.color = new Color(1f, 0f, 0f, 0f);
        goBiteTop.gameObject.SetActive(false);
        goBiteTop.gameObject.transform.localPosition = new Vector2(4.15f, 525f);
        goBiteBottom.gameObject.SetActive(false);
        goBiteBottom.gameObject.transform.localPosition = new Vector2(5.5f, -535f);
        goBiteTopBackground.color = new Color(0f, 0f, 0f, 0f);
        goBiteBottomBackground.color = new Color(0f, 0f, 0f, 0f);
        goGhost.color = new Color(1f, 1f, 1f, 0f);
        goText.text = "";
        goRestartText.text = "";
        goQuitText.text = "";
        goSelect.gameObject.SetActive(false);
        goSelect.transform.localPosition = new Vector2(-175.9f, -374.9f);
        goSelect.transform.localScale = new Vector2(64.8f, 108f);
    }

    public void Move(InputAction.CallbackContext context){
        if(isAbleToMove){
            move = context.ReadValue<Vector2>();
        }
    }

    public void Flashlight(InputAction.CallbackContext context){
        if(flashlightAction.triggered && !isDead){
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
        if(interactAction.triggered && !isDead){
            // Debug.Log("Interact!");
            if(level1TpStepped){
                rb2D.AddForce(new Vector2(0f, 0f));
                character.transform.position = level1Spawner.transform.position;
                level1TpStepped = false;
                StartCoroutine("DoorToLevel");
            }
            if(level2TpStepped){
                rb2D.AddForce(new Vector2(0f, 0f));
                character.transform.position = level2Spawner.transform.position;
                level2TpStepped = false;
                StartCoroutine("DoorToLevel");
            }
            if(level3TpStepped){
                rb2D.AddForce(new Vector2(0f, 0f));
                character.transform.position = level3Spawner.transform.position;
                level3TpStepped = false;
                StartCoroutine("DoorToLevel");
            }
            if(level1TpBackStepped){
                rb2D.AddForce(new Vector2(0f, 0f));
                character.transform.position = hallSpawnerFromLevel1.transform.position;
                level1TpBackStepped = false;
                StartCoroutine("DoorFromLevel");
            }
            if(level2TpBackStepped){
                rb2D.AddForce(new Vector2(0f, 0f));
                character.transform.position = hallSpawnerFromLevel2.transform.position;
                level2TpBackStepped = false;
                StartCoroutine("DoorFromLevel");
            }
            if(level3TpBackStepped){
                rb2D.AddForce(new Vector2(0f, 0f));
                character.transform.position = hallSpawnerFromLevel3.transform.position;
                level3TpBackStepped = false;
                StartCoroutine("DoorFromLevel");
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
        goSelectSound.volume = PlayerPrefs.GetFloat("volume");
        goConfirmSound.volume = PlayerPrefs.GetFloat("volume");
        die.volume = PlayerPrefs.GetFloat("volume");

        // SPRITES
        if(characterSprite.sprite == upSprite1 || characterSprite.sprite == upSprite2 || characterSprite.sprite == upSprite3){
            Vector3 temp = transform.rotation.eulerAngles;
            temp.z = 90f;
            flashlight.transform.rotation = Quaternion.Euler(temp);
            flashlight.transform.localPosition = new Vector2(0.088f, 0.2f);
        }else if(characterSprite.sprite == downSprite1 || characterSprite.sprite == downSprite2 || characterSprite.sprite == downSprite3){
            Vector3 temp = transform.rotation.eulerAngles;
            temp.z = -90f;
            flashlight.transform.rotation = Quaternion.Euler(temp);
            flashlight.transform.localPosition = new Vector2(-0.078f, -0.34f);
        }else if(characterSprite.sprite == leftSprite1 || characterSprite.sprite == leftSprite2){
            Vector3 temp = transform.rotation.eulerAngles;
            temp.z = 180f;
            flashlight.transform.rotation = Quaternion.Euler(temp);
            flashlight.transform.localPosition = new Vector2(-0.324f, -0.093f);
        }else if(characterSprite.sprite == rightSprite1 || characterSprite.sprite == rightSprite2){
            Vector3 temp = transform.rotation.eulerAngles;
            temp.z = 0f;
            flashlight.transform.rotation = Quaternion.Euler(temp);
            flashlight.transform.localPosition = new Vector2(0.334f, -0.075f);
        }

        //FLASHLIGHT
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

        // DEATH
        if(isDead){
            walkSound.Stop();
        }
        if(goDone){
            var keyboard = Keyboard.current;
            if(keyboard.leftArrowKey.wasPressedThisFrame){
                whereSelect -= 1;
                goSelectSound.Play();
            }else if(keyboard.rightArrowKey.wasPressedThisFrame){
                whereSelect += 1;
                goSelectSound.Play();
            }
            if(whereSelect <= 0){
                whereSelect = 2;
            }else if(whereSelect >= 3){
                whereSelect = 1;
            }

            switch(whereSelect){
                case 1:
                    goSelect.transform.localPosition = new Vector2(-175.9f, -374.9f);
                    goSelect.transform.localScale = new Vector2(64.8f, 108f);
                    if(keyboard.enterKey.wasPressedThisFrame){
                        goConfirmSound.Play();
                        SceneManager.LoadScene("Maxime");
                    }
                    break;
                case 2:
                    goSelect.transform.localPosition = new Vector2(175.9f, -374.9f);
                    goSelect.transform.localScale = new Vector2(42.4f, 108f);
                    if(keyboard.enterKey.wasPressedThisFrame){
                        goConfirmSound.Play();
                        Application.Quit();
                    }
                    break;
            }
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
        }else if(col.tag == "Coins"){
            coinSound.Play();
            score += coinsValue;
            scoreTxtObject.text = scoreText + score.ToString();
            Destroy(col.gameObject);
        }

        // CHANGE ROOM
        if(col.name == "Level1TP" && !level1Done){
            level1TpStepped = true;
            levelText.text = "<To level 1>";
            interactText.text = "[Press " + InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null) + " to interact]";
        }
        if(col.name == "Level2TP" && !level2Done && level1Done){
            level2TpStepped = true;
            levelText.text = "<To level 2>";
            interactText.text = "[Press " + InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null) + " to interact]";
        }
        if(col.name == "Level3TP" && !level3Done && level2Done){
            level3TpStepped = true;
            levelText.text = "<To level 3>";
            interactText.text = "[Press " + InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null) + " to interact]";
        }
        if(col.name == "Level1TPBack" && level1Done){
            level1TpBackStepped = true;
            levelText.text = "<Back to hall>";
            interactText.text = "[Press " + InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null) + " to interact]";
        }
        if(col.name == "Level2TPBack" && level2Done){
            level2TpBackStepped = true;
            levelText.text = "<Back to hall>";
            interactText.text = "[Press " + InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null) + " to interact]";
        }
        if(col.name == "Level3TPBack" && level3Done){
            level3TpBackStepped = true;
            levelText.text = "<Back to hall>";
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

        // DEATH
        if(col.gameObject.layer == 10 && !isDead){
            isDead = true;
            StartCoroutine("Death");
        }
    }

    void OnTriggerExit2D(Collider2D col){
        // CHANGE ROOM
        if(col.name == "Level1TP"){
            level1TpStepped = false;
            levelText.text = "";
            interactText.text = "";
        }
        if(col.name == "Level2TP"){
            level2TpStepped = false;
            levelText.text = "";
            interactText.text = "";
        }
        if(col.name == "Level3TP"){
            level3TpStepped = false;
            levelText.text = "";
            interactText.text = "";
        }
        if(col.name == "Level1TPBack"){
            level1TpStepped = false;
            levelText.text = "";
            interactText.text = "";
        }
        if(col.name == "Level2TPBack"){
            level2TpStepped = false;
            levelText.text = "";
            interactText.text = "";
        }
        if(col.name == "Level3TPBack"){
            level3TpStepped = false;
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

    IEnumerator DoorToLevel(){
        doorOpen.Play();
        yield return new WaitForSeconds(doorOpen.clip.length);
        doorClose.Play();
        yield return new WaitForSeconds(doorClose.clip.length);
        doorLock.Play();
        enemyLaugh.Play();
    }

    IEnumerator DoorFromLevel(){
        doorOpen.Play();
        yield return new WaitForSeconds(doorOpen.clip.length);
        doorClose.Play();
        yield return new WaitForSeconds(doorClose.clip.length);
        doorLock.Play();
    }

    IEnumerator Death(){
        die.Play();
        isAbleToMove = false;
        rb2D.AddForce(new Vector2(0f, 0f));
        float i = 0;
        while(i < 1f){
            goBackground.color = new Color(1f, 0f, 0f, i);
            yield return new WaitForSeconds(0.01f);
            i += 0.01f;
        }
        i = 0;
        float toGoTop = 525f;
        float toGoBottom = -535f;
        goBiteTop.gameObject.SetActive(true);
        goBiteBottom.gameObject.SetActive(true);
        goBiteTopBackground.color = new Color(0f, 0f, 0f, 1f);
        goBiteBottomBackground.color = new Color(0f, 0f, 0f, 1f);
        while(i < 760f){
            i += 20f;
            toGoTop -= 20f;
            toGoBottom += 20f;
            yield return new WaitForSeconds(0.000001f);
            goBiteTop.gameObject.transform.localPosition = new Vector2(4.15f, toGoTop);
            goBiteBottom.gameObject.transform.localPosition = new Vector2(4.15f, toGoBottom);
        }
        i = 0;
        goSelect.color = new Color(1f, 1f, 1f, 0f);
        goSelect.gameObject.SetActive(true);
        goText.color = new Color(1f, 1f, 1f, 0f);
        goRestartText.color = new Color(1f, 1f, 1f, 0f);
        goQuitText.color = new Color(1f, 1f, 1f, 0f);
        goText.text = "GAME OVER";
        goRestartText.text = "RESTART";
        goQuitText.text = "QUIT";
        while(i < 1f){
            goGhost.color = new Color(1f, 1f, 1f, i);
            goText.color = new Color(1f, 1f, 1f, i);
            goRestartText.color = new Color(1f, 1f, 1f, i);
            goQuitText.color = new Color(1f, 1f, 1f, i);
            goSelect.color = new Color(1f, 1f, 1f, i);
            yield return new WaitForSeconds(0.01f);
            i += 0.01f;
        }
        goDone = true;
    }
}
