using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    //Audio sources
    [SerializeField] private AudioSource earthquake;
    [SerializeField] private AudioSource thunder;
    [SerializeField] private AudioSource laught;
    [SerializeField] private AudioSource ceramic;
    [SerializeField] private AudioSource glass;
    //Audio Deley
    [SerializeField] private float thunderDelay;
    [SerializeField] private float earthquakeDelay;
    [SerializeField] private float laughtDelay;


    //GameOgjects
    [SerializeField] private GameObject windows;

    // Start is called before the first frame update
    void Start()
    {
        //Set Audio Volume
        // thunder.volume = PlayerPrefs.GetFloat("volume");
        // earthquake.volume = PlayerPrefs.GetFloat("volume");
        // laught.volume = PlayerPrefs.GetFloat("volume");
        // ceramic.volume = PlayerPrefs.GetFloat("volume");
        // glass.volume = PlayerPrefs.GetFloat("volume");
        
        windows.SetActive(false);
        StartCoroutine("AbianceCoroutine");
        
    }
    
    void FixedUpdate(){
        
    }
    // Update is called once per frame
    void Update()
    {   
        
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Ceramic"){
            ceramic.Play(0);
            Destroy(col.gameObject);
        }
        if(col.tag == "Window"){ 
            glass.Play(0);
            Destroy(col.gameObject);
        }
    }
       
    IEnumerator AbianceCoroutine(){
        while(true){
            yield return new WaitForSeconds(thunderDelay);
            thunder.Play(0);
            windows.SetActive(true);
            yield return new WaitForSeconds(5.5f);
            windows.SetActive(false);

            yield return new WaitForSeconds(earthquakeDelay);
            earthquake.Play(0);
        
            yield return new WaitForSeconds(laughtDelay);
            laught.Play(0);
            yield return new WaitForSeconds(10f);
        }
    }

}
