using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    [SerializeField] private AudioSource thunder;
     [SerializeField] private float thunderDelay;
     [SerializeField] private GameObject windows;
     private bool thunderPlay;
    // Start is called before the first frame update
    void Start()
    {
        windows.SetActive(false);
        thunderPlay = false;
    }
    
    void FixedUpdate(){

    }
    // Update is called once per frame
    void Update()
    {   
        thunder.volume = PlayerPrefs.GetFloat("volume");
         if(thunderPlay == false){
            StartCoroutine("ThunderCoroutine");
            if(thunderPlay == true){
                thunder.Play();
                windows.SetActive(false);
            }
            windows.SetActive(true);
        }

    }

    IEnumerator ThunderCoroutine(){
        yield return new WaitForSeconds(thunderDelay);
        thunderPlay = true;
        Debug.Log(thunderPlay);
        yield return new WaitForSeconds(5f);
        thunderPlay = false;
        Debug.Log(thunderPlay);


    }
}
