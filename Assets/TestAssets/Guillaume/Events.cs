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
    // Start is called before the first frame update
    void Start()
    {
        windows.SetActive(false);
        StartCoroutine("ThunderCoroutine");
    }
    
    void FixedUpdate(){
        
    }
    // Update is called once per frame
    void Update()
    {   

    }
       
    IEnumerator ThunderCoroutine(){
        while(true){
            yield return new WaitForSeconds(thunderDelay);
            thunder.Play(0);
            windows.SetActive(true);
             yield return new WaitForSeconds(4.5f);
            windows.SetActive(false);
        }
    }
}
