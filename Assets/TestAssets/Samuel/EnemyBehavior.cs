using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    private float[] speedArray = new float[3];
    private Rigidbody2D rb2D;
    private GameObject enemy;
    private Vector2 randomDirection;
    [SerializeField] private Vector2 direction;

    void Start(){
        enemy = gameObject;
        rb2D = enemy.GetComponent<Rigidbody2D>();
        speedArray[0] = -1f;
        speedArray[1] = 0f;
        speedArray[2] = 1f;
        // randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomDirection = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
        direction = randomDirection;
    }

    void Update(){

    }

    void FixedUpdate(){
        // MOVEMENT
        Vector2 newPos = rb2D.position;
        newPos += Time.fixedDeltaTime * direction;
        if(direction == new Vector2(0f, 0f)){
            direction = new Vector2(speedArray[Random.Range(0, 2)], Random.Range(0, 2));
        }
        rb2D.MovePosition(newPos);
        // Debug.Log("x = " + direction.x + " y = " + direction.y);
    }

    void OnCollisionEnter2D(Collision2D col){
        Debug.Log("Collision!");
        // Debug.Log(col.contacts);
        // direction = new Vector2(-1*direction.x*Random.Range(0.1f, 1f), -1*direction.y*Random.Range(0.1f, 1f));
        Vector2 oldDirection = direction;
        rb2D.AddForce(new Vector2(0f, 0f));
        if(oldDirection.x == 1f){
            direction.x = speedArray[Random.Range(0, 1)];
        }else if(oldDirection.x == -1f){
            direction.x = speedArray[Random.Range(1, 2)];
        }
        if(oldDirection.y == 1f){
            direction.y = speedArray[Random.Range(0, 1)];
        }else if(oldDirection.y == -1f){
            direction.y = speedArray[Random.Range(1, 2)];
        }
        // direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
    }
}
