using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Thunder : MonoBehaviour
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
        //PlayerPrefs.GetFloat("volume")
        thunder.volume = 0.5f;
        
         if(!thunder.isPlaying){
             StartCoroutine("ThunderCoroutine");
            if(thunderPlay && !thunder.isPlaying){
                thunder.Play(0);
            }
            windows.SetActive(false);
        }else{
            windows.SetActive(true);
        }
    }

    IEnumerator ThunderCoroutine(){
        yield return new WaitForSeconds(thunderDelay);
        thunderPlay = true;
        yield return new WaitForSeconds(0.1f);
        thunderPlay = false;
    }
}
