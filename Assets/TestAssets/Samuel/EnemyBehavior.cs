using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    private float[] speedArray = new float[3];
    private Rigidbody2D rb2D;
    private GameObject enemy;
    private SpriteRenderer enemySprite;
    private Vector2 randomDirection;
    private bool invulnerable;
    private BoxCollider2D collider2D;
    [SerializeField] private Vector2 direction;
    [SerializeField] private GameObject spawn;
    [SerializeField] private float runSpeed;
    [SerializeField] private int freezeDuration;
    [SerializeField] private int runDuration;
    [SerializeField] private int respawnDuration;

    void Start(){
        enemy = gameObject;
        rb2D = enemy.GetComponent<Rigidbody2D>();
        speedArray[0] = -1f;
        speedArray[1] = 0f;
        speedArray[2] = 1f;
        // randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomDirection = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
        direction = randomDirection;

        collider2D = enemy.GetComponent<BoxCollider2D>();

        enemySprite = enemy.GetComponent<SpriteRenderer>();
        enemySprite.color = new Color(1f, 1f, 1f, 0f);

        invulnerable = false;
    }

    void Update(){

    }

    void FixedUpdate(){
        // MOVEMENT
        Vector2 newPos = rb2D.position;
        newPos += Time.fixedDeltaTime * speed * direction;
        if(direction == new Vector2(0f, 0f) && !invulnerable){
            direction = new Vector2(speedArray[Random.Range(0, 2)], Random.Range(0, 2));
        }
        rb2D.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D col){
        // Debug.Log("Collision!");
        Vector2 oldDirection = direction;
        float newDirectionX = 0f;
        float newDirectionY = 0f;
        rb2D.AddForce(new Vector2(0f, 0f));

        if(oldDirection.x == 1f){
            newDirectionX = speedArray[Random.Range(0, 2)];
        }else if(oldDirection.x == -1f){ 
            newDirectionX = speedArray[Random.Range(1, 3)]; 
        }

        if(oldDirection.y == 1f){
            newDirectionY = speedArray[Random.Range(0, 2)];
        }else if(oldDirection.y == -1f){
            newDirectionY = speedArray[Random.Range(1, 3)];
        }

        Debug.Log("OLD X = " + oldDirection.x);
        direction = new Vector2(newDirectionX, newDirectionY);
        Debug.Log("NEW X = " + direction.x);
        // direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Flashlight" && !invulnerable){
            StartCoroutine("Run");
        }
    }

    IEnumerator Run(){
        invulnerable = true;
        rb2D.AddForce(new Vector2(0f, 0f));
        direction = new Vector2(0f, 0f);
        enemySprite.color = new Color(1f, 1f, 1f, 1f);
        float oldSpeed = speed;

        yield return new WaitForSeconds(freezeDuration);

        collider2D.enabled = false;
        direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
        speed += runSpeed;

        yield return new WaitForSeconds(runDuration);

        enemySprite.color = new Color(1f, 1f, 1f, 0f);
        rb2D.AddForce(new Vector2(0f, 0f));
        direction = new Vector2(0f, 0f);
        enemy.transform.position = spawn.transform.position;
        collider2D.enabled = true;

        yield return new WaitForSeconds(respawnDuration);

        invulnerable = false;
        speed = oldSpeed;
        direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
    }
}
