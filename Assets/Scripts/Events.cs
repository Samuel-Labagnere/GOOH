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
    [SerializeField] private AudioSource laught2;
    [SerializeField] private AudioSource ceramic;
    [SerializeField] private AudioSource glass;
    [SerializeField] private AudioSource guillotine;
    [SerializeField] private AudioSource knocking;
    [SerializeField] private AudioSource metal;
    [SerializeField] private AudioSource wolf;

  
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
        // laught2.volume = PlayerPrefs.GetFloat("volume");
        // guillotine.volume = PlayerPrefs.GetFloat("volume");
        // knocking.volume = PlayerPrefs.GetFloat("volume");
        // metal.volume = PlayerPrefs.GetFloat("volume");
        // wolf.volume = PlayerPrefs.GetFloat("volume");
        
        windows.SetActive(false);
        StartCoroutine("AmbianceCoroutine");
        
    }
    
    void FixedUpdate(){
        
    }
    // Update is called once per frame
    void Update()
    {   
        
    }

       
    IEnumerator AmbianceCoroutine(){
        int rand;
        while(true){
            yield return new WaitForSeconds(Random.Range(10f, 30f));
            rand = Random.Range(1, 11); 
            switch(rand){
                case 1:
                    thunder.Play(0);
                    windows.SetActive(true);
                    yield return new WaitForSeconds(5.5f);
                    windows.SetActive(false);
                    break;
                case 2:
                    earthquake.Play(0);
                    break;
                case 3:
                    laught.Play(0);
                    break;
                case 4:
                    laught2.Play(0);
                    break;
                case 5:
                    ceramic.Play(0);
                    break;
                case 6:
                    glass.Play(0);
                    break;
                case 7:
                    guillotine.Play(0);
                    break;
                case 8:
                    knocking.Play(0);
                    break;
                case 9:
                    metal.Play(0);
                    break;
                case 10:
                    wolf.Play(0);
                    break;
            }

        }
    }

}
