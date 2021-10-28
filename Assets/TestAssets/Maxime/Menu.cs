using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour

{
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject homeMenu;
    [SerializeField] private GameObject optionSelector;
    [SerializeField] private GameObject homeSelector;
    [SerializeField] private GameObject soundSelector;
    private string whereSelector;
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioSource sliderSound;
    [SerializeField] private AudioSource confirmSound;
    [SerializeField] private Text upText;
    [SerializeField] private Text leftText;
    [SerializeField] private Text downText;
    [SerializeField] private Text rightText;
    [SerializeField] private Text flashlightText;
    [SerializeField] private Text interactText;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Text sliderText;
    [SerializeField] private string performRebinding;
    private PlayerInput pInput;
    private InputAction moveAction;
    private InputAction flashlightAction;
    private InputAction interactAction;
    private InputActionRebindingExtensions.RebindingOperation rbOperation;
    private int up;
    private int left;
    private int down;
    private int right;


        
    // Start is called before the first frame update
    void Start()
    {
        optionMenu.SetActive(false);

        homeSelector.transform.position = new Vector2(0f, 0.27f);
        whereSelector = "play";

        pInput = gameObject.GetComponent<PlayerInput>();

        moveAction = pInput.actions.FindAction("Move");
        flashlightAction = pInput.actions.FindAction("Flashlight");
        interactAction = pInput.actions.FindAction("Interact");
        up = moveAction.bindings.IndexOf(x => x.name == "up");
        left = moveAction.bindings.IndexOf(x => x.name == "left");
        down = moveAction.bindings.IndexOf(x => x.name == "down");
        right = moveAction.bindings.IndexOf(x => x.name == "right");

        sliderText.text = "";

        upText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[up].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        leftText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[left].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        downText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[down].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        rightText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[right].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        flashlightText.text = InputControlPath.ToHumanReadableString(flashlightAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        interactText.text = InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    // Update is called once per frame
    void Update()
    {
        moveSound.volume = PlayerPrefs.GetFloat("volume");
        sliderSound.volume = PlayerPrefs.GetFloat("volume");
        confirmSound.volume = PlayerPrefs.GetFloat("volume");

        sliderText.text = soundSlider.value + "%";
        PlayerPrefs.SetFloat("volume", soundSlider.value/100);

        if(optionMenu.activeSelf){

            handleOptions();

        }else{

            handleHome();

        }
    }

    void handleOptions(){
        var keyboard = Keyboard.current;
        if(keyboard.downArrowKey.wasPressedThisFrame){
            moveSound.Play();
            switch(whereSelector){
                case "up":
                    optionSelector.transform.position = new Vector2(-2.27f, 0.39f);
                    whereSelector = "left";
                    break;
                case "left":
                    optionSelector.transform.position = new Vector2(-0.1f, 0.39f);
                    whereSelector = "down";
                    break;
                case "down":
                    optionSelector.transform.position = new Vector2(2.06f, 0.39f);
                    whereSelector = "right";
                    break;
                case "right":
                    optionSelector.transform.position = new Vector2(-2.79f, -1.4f);
                    whereSelector = "flashlight";
                    break;
                case "flashlight":
                    optionSelector.transform.position = new Vector2(1.24f, -1.4f);
                    whereSelector = "interact";
                    break;
                case "interact":
                    optionSelector.SetActive(false);
                    soundSelector.SetActive(true);
                    whereSelector = "sound";
                    break;
                case "sound":
                    soundSelector.SetActive(false);
                    optionSelector.SetActive(true);
                    optionSelector.transform.position = new Vector2(5.56f, 2.98f);
                    whereSelector = "close";
                    break;
                case "close":
                    optionSelector.transform.position = new Vector2(-4.42f, 0.39f);
                    whereSelector = "up";
                    break;
            }
        }

        if(keyboard.upArrowKey.wasPressedThisFrame){
            moveSound.Play();
            switch(whereSelector){
                case "up":
                    optionSelector.transform.position = new Vector2(5.56f, 2.98f);
                    whereSelector = "close";
                    break;
                case "left":
                    optionSelector.transform.position = new Vector2(-4.42f, 0.39f);
                    whereSelector = "up";
                    break;
                case "down":
                    optionSelector.transform.position = new Vector2(-2.27f, 0.39f);
                    whereSelector = "left";
                    break;
                case "right":
                    optionSelector.transform.position = new Vector2(-0.1f, 0.39f);
                    whereSelector = "down";
                    break;
                case "flashlight":
                    optionSelector.transform.position = new Vector2(2.06f, 0.39f);
                    whereSelector = "right";
                    break;
                case "interact":
                    optionSelector.transform.position = new Vector2(-2.79f, -1.4f);
                    whereSelector = "flashlight";
                    break;
                case "sound":
                    soundSelector.SetActive(false);
                    optionSelector.SetActive(true);
                    optionSelector.transform.position = new Vector2(1.24f, -1.4f);
                    whereSelector = "interact";
                    break;
                case "close":
                    soundSelector.SetActive(true);
                    optionSelector.SetActive(false);
                    whereSelector = "sound";
                    break;
            }
        }

        if(keyboard.enterKey.wasPressedThisFrame){
            confirmSound.Play();
            switch(whereSelector){
                case "up":
                    RemapUp();
                    break;
                case "left":
                    RemapLeft();
                    break;
                case "down":
                    RemapDown();
                    break;
                case "right":
                    RemapRight();
                    break;
                case "flashlight":
                    RemapFlashlight();
                    break;
                case "interact":
                    RemapInteract();
                    break;
                case "close":
                    homeMenu.SetActive(true);
                    optionMenu.SetActive(false);
                    whereSelector = "options";
                    break;
            }
        }

        if(whereSelector == "sound"){
            if(keyboard.rightArrowKey.wasPressedThisFrame){
                soundSlider.value += 10f;
                sliderSound.Play();

            }
            if(keyboard.leftArrowKey.wasPressedThisFrame){
                soundSlider.value -= 10f;
                sliderSound.Play();
            }
        }
    }

    void handleHome(){
        var keyboard = Keyboard.current;
        if(keyboard.downArrowKey.wasPressedThisFrame){
            moveSound.Play();
            switch(whereSelector){
                case "play":
                    homeSelector.transform.position = new Vector2(0f, -1.21f);
                    whereSelector = "options";
                    break;
                case "options":
                    homeSelector.transform.position = new Vector2(0f, -2.61f);
                    whereSelector = "quit";
                    break;
                case "quit":
                    homeSelector.transform.position = new Vector2(-4.72f, -2.73f);
                    whereSelector = "credits";
                    break;
                case "credits":
                    homeSelector.transform.position = new Vector2(0f, 0.27f);
                    whereSelector = "play";
                    break;
            }
        }

        if(keyboard.upArrowKey.wasPressedThisFrame){
            moveSound.Play();
            switch(whereSelector){
                case "play":
                    homeSelector.transform.position = new Vector2(-4.72f, -2.73f);
                    whereSelector = "credits";
                    break;
                case "options":
                    homeSelector.transform.position = new Vector2(0f, 0.27f);
                    whereSelector = "play";
                    break;
                case "quit":
                    homeSelector.transform.position = new Vector2(0f, -1.21f);
                    whereSelector = "options";
                    break;
                case "credits":
                    homeSelector.transform.position = new Vector2(0f, -2.61f);
                    whereSelector = "quit";
                    break;
            }
        }
        
        if(keyboard.enterKey.wasPressedThisFrame){
            confirmSound.Play();
            switch(whereSelector){
                case "play":
                    SceneManager.LoadScene("Samuel");
                    break;
                case "options":
                    homeMenu.SetActive(false);
                    optionMenu.SetActive(true);
                    whereSelector = "up";
                    optionSelector.transform.position = new Vector2(-4.42f, 0.39f);
                    soundSelector.SetActive(false);
                    break;
                case "quit":
                    Application.Quit();
                    break;
                case "credits":
                    // homeMenu.SetActive(false);
                    // credits.SetActive(true);
                    break;
            }
        }
    }

    void RemapUp(){
        Debug.Log("remap go");
        moveAction.Disable();
        upText.text = performRebinding;
        rbOperation = moveAction.PerformInteractiveRebinding(up)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                sliderSound.Play();
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                upText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[up].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            });
    }
    void RemapDown(){
        Debug.Log("remap go");
        moveAction.Disable();
        downText.text = performRebinding;
        rbOperation = moveAction.PerformInteractiveRebinding(down)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                sliderSound.Play();
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                downText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[down].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            });
    }
    void RemapLeft(){
        Debug.Log("remap go");
        moveAction.Disable();
        leftText.text = performRebinding;
        rbOperation = moveAction.PerformInteractiveRebinding(left)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                sliderSound.Play();
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                leftText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[left].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            });
    }
    void RemapRight(){
        Debug.Log("remap go");
        moveAction.Disable();
        rightText.text = performRebinding;
        rbOperation = moveAction.PerformInteractiveRebinding(right)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                sliderSound.Play();
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                rightText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[right].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            });
    }
    void RemapFlashlight(){
        Debug.Log("remap go");
        flashlightAction.Disable();
        flashlightText.text = performRebinding;
        rbOperation = flashlightAction.PerformInteractiveRebinding(0)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                sliderSound.Play();
                Debug.Log("saved");
                flashlightAction.Enable();
                flashlightAction.Dispose();
                flashlightText.text = InputControlPath.ToHumanReadableString(flashlightAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            });
    }
    void RemapInteract(){
        Debug.Log("remap go");
        interactAction.Disable();
        interactText.text = performRebinding;
        rbOperation = interactAction.PerformInteractiveRebinding(0)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                sliderSound.Play();
                Debug.Log("saved");
                interactAction.Enable();
                interactAction.Dispose();
                interactText.text = InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            });
    }
}
