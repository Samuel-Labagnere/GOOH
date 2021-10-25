using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouge : MonoBehaviour{

    [SerializeField] Rigidbody2D player;
    [SerializeField] int force = 100;
   
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        float y = Input.GetAxis("Vertical") * Time.fixedDeltaTime;

        if (x != 0) player.AddForce(Vector2.right * x * force);
        if (y != 0) player.AddForce(Vector2.up * y * force);
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
