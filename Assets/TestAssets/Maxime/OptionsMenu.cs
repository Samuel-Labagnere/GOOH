using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject option_selector;
    [SerializeField] private GameObject sound_selector;
    private string where_selector;
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private Text upText;
    [SerializeField] private Text leftText;
    [SerializeField] private Text downText;
    [SerializeField] private Text rightText;
    [SerializeField] private Text lampText;
    [SerializeField] private Text interactText;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Text sliderText;
    private PlayerInput pInput;
    private InputAction moveAction;
    private InputAction lampAction;
    private InputAction interactAction;
    private InputActionRebindingExtensions.RebindingOperation rbOperation;
    private int up;
    private int left;
    private int down;
    private int right;


        
    // Start is called before the first frame update
    void Start()
    {
        pInput = gameObject.GetComponent<PlayerInput>();

        moveAction = pInput.actions.FindAction("Move");
        lampAction = pInput.actions.FindAction("Flashlight");
        interactAction = pInput.actions.FindAction("Interact");
        up = moveAction.bindings.IndexOf(x => x.name == "up");
        left = moveAction.bindings.IndexOf(x => x.name == "left");
        down = moveAction.bindings.IndexOf(x => x.name == "down");
        right = moveAction.bindings.IndexOf(x => x.name == "right");

        option_selector.transform.position = new Vector2(-4.42f, 0.39f);
        where_selector = "up";
        sound_selector.SetActive(false);

        sliderText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        sliderText.text = soundSlider.value + "%";
        PlayerPrefs.SetFloat("volume", soundSlider.value);
        
        upText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[up].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        leftText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[left].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        downText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[down].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        rightText.text = InputControlPath.ToHumanReadableString(moveAction.bindings[right].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        lampText.text = InputControlPath.ToHumanReadableString(lampAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        interactText.text = InputControlPath.ToHumanReadableString(interactAction.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        var keyboard = Keyboard.current;
        if(keyboard.downArrowKey.wasPressedThisFrame){
            moveSound.Play();
            switch(where_selector){
                case "up":
                    option_selector.transform.position = new Vector2(-2.27f, 0.39f);
                    where_selector = "left";
                    break;
                case "left":
                    option_selector.transform.position = new Vector2(-0.1f, 0.39f);
                    where_selector = "down";
                    break;
                case "down":
                    option_selector.transform.position = new Vector2(2.06f, 0.39f);
                    where_selector = "right";
                    break;
                case "right":
                    option_selector.transform.position = new Vector2(-3.75f, -1.4f);
                    where_selector = "lamp";
                    break;
                case "lamp":
                    option_selector.transform.position = new Vector2(1.24f, -1.4f);
                    where_selector = "interact";
                    break;
                case "interact":
                    option_selector.SetActive(false);
                    sound_selector.SetActive(true);
                    where_selector = "sound";
                    break;
                case "sound":
                    sound_selector.SetActive(false);
                    option_selector.SetActive(true);
                    option_selector.transform.position = new Vector2(5.56f, 2.98f);
                    where_selector = "close";
                    break;
                case "close":
                    option_selector.transform.position = new Vector2(-4.42f, 0.39f);
                    where_selector = "up";
                    break;
            }
        }

        if(keyboard.upArrowKey.wasPressedThisFrame){
            moveSound.Play();
            switch(where_selector){
                case "up":
                    option_selector.transform.position = new Vector2(5.56f, 2.98f);
                    where_selector = "close";
                    break;
                case "left":
                    option_selector.transform.position = new Vector2(-4.42f, 0.39f);
                    where_selector = "up";
                    break;
                case "down":
                    option_selector.transform.position = new Vector2(-2.27f, 0.39f);
                    where_selector = "left";
                    break;
                case "right":
                    option_selector.transform.position = new Vector2(-0.1f, 0.39f);
                    where_selector = "down";
                    break;
                case "lamp":
                    option_selector.transform.position = new Vector2(2.06f, 0.39f);
                    where_selector = "right";
                    break;
                case "interact":
                    option_selector.transform.position = new Vector2(-3.75f, -1.4f);
                    where_selector = "lamp";
                    break;
                case "sound":
                    sound_selector.SetActive(false);
                    option_selector.SetActive(true);
                    option_selector.transform.position = new Vector2(1.24f, -1.4f);
                    where_selector = "interact";
                    break;
                case "close":
                    sound_selector.SetActive(true);
                    option_selector.SetActive(false);
                    where_selector = "sound";
                    break;
            }
        }
        if(keyboard.enterKey.wasPressedThisFrame){
            // selectSound.Play();
            switch(where_selector){
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
                case "lamp":
                    RemapFlashlight();
                    break;
                case "interact":
                    RemapInteract();
                    break;
                case "close":
                    SceneManager.LoadScene("Samuel");
                    break;
            }
        }
        if(where_selector == "sound"){
            if(keyboard.rightArrowKey.wasPressedThisFrame){
                soundSlider.value += 10f;
            }
            if(keyboard.leftArrowKey.wasPressedThisFrame){
                soundSlider.value -= 10f;
            }
        }
    }

    void RemapUp(){
        Debug.Log("remap go");
        moveAction.Disable();
        rbOperation = moveAction.PerformInteractiveRebinding(up)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                // SaveUserRebinds(pInput);
            });
    }
    void RemapDown(){
        Debug.Log("remap go");
        moveAction.Disable();
        rbOperation = moveAction.PerformInteractiveRebinding(down)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                // SaveUserRebinds(pInput);
            });
    }
    void RemapLeft(){
        Debug.Log("remap go");
        moveAction.Disable();
        rbOperation = moveAction.PerformInteractiveRebinding(left)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                // SaveUserRebinds(pInput);
            });
    }
    void RemapRight(){
        Debug.Log("remap go");
        moveAction.Disable();
        rbOperation = moveAction.PerformInteractiveRebinding(right)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                Debug.Log("saved");
                moveAction.Enable();
                moveAction.Dispose();
                // SaveUserRebinds(pInput);
            });
    }
    void RemapFlashlight(){
        Debug.Log("remap go");
        lampAction.Disable();
        rbOperation = lampAction.PerformInteractiveRebinding(0)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                Debug.Log("saved");
                lampAction.Enable();
                lampAction.Dispose();
                // SaveUserRebinds(pInput);
            });
    }
    void RemapInteract(){
        Debug.Log("remap go");
        interactAction.Disable();
        rbOperation = interactAction.PerformInteractiveRebinding(0)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start().OnCancel(op =>
            {
                Debug.Log("cancel");
 
            }).OnComplete(op =>
            {
                Debug.Log("saved");
                interactAction.Enable();
                interactAction.Dispose();
                // SaveUserRebinds(pInput);
            });
    }
}
