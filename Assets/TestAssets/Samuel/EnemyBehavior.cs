using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public CharacterBehavior characterScript;
    private bool isAbleToMove;
    [SerializeField] private float speed;
    private float[] speedArray = new float[3];
    private Rigidbody2D rb2D;
    private GameObject enemy;
    private GameObject deathField;
    private SpriteRenderer enemySprite;
    private Vector2 randomDirection;
    private bool invulnerable;
    private BoxCollider2D col2D;
    [SerializeField] private int enemyNb;
    [SerializeField] private Vector2 direction;
    [SerializeField] private GameObject spawn;
    [SerializeField] private GameObject EnemyStats;
    [SerializeField] private float runSpeed;
    [SerializeField] private int freezeDuration;
    [SerializeField] private int runDuration;
    [SerializeField] private int respawnDuration;
    [SerializeField] private AudioSource dieSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource disappearSound;
    [SerializeField] private AudioSource laughtSound;
    [SerializeField] private int lives;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject heartSpawner;
    [SerializeField] private SpriteRenderer icon;

    [SerializeField] private LayerMask rayCastMask;
    private GameObject[] heartArray;

    // MOVEMENT
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    // DETECTOR
    [SerializeField] private SpriteRenderer detector;
    [SerializeField] private Sprite detectorNeutral;
    [SerializeField] private Sprite detectorOrange;
    [SerializeField] private Sprite detectorRed;
    [SerializeField] private BoxCollider2D orangeZone;
    [SerializeField] private BoxCollider2D redZone;
    [SerializeField] private AudioSource ledSound;
    [SerializeField] private AudioSource alertSound;

    // DOOR
    [SerializeField] private AudioSource doorLock;

    void Start(){

        EnemyStats.SetActive(false);

        enemy = gameObject;
        deathField = enemy.transform.GetChild(0).gameObject;
        rb2D = enemy.GetComponent<Rigidbody2D>();
        speedArray[0] = -1f;
        speedArray[1] = 0f;
        speedArray[2] = 1f;
        // randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        // randomDirection = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
        // direction = randomDirection;

        col2D = enemy.GetComponent<BoxCollider2D>();

        enemySprite = enemy.GetComponent<SpriteRenderer>();
        enemySprite.color = new Color(1f, 1f, 1f, 0f);

        isAbleToMove = false;
        invulnerable = false;
        int rightSpace = 0;
        int leftSpace = 0;
        heartArray = new GameObject[lives];
        for(int i=0; i < lives; i++){
            Vector2 heartSpawnerPos = new Vector2(0f, 0f);
            if(lives%2 == 0){
                heartSpawnerPos = new Vector2(heartSpawner.transform.position.x-0.5f, heartSpawner.transform.position.y);
            }else{
                heartSpawnerPos = heartSpawner.transform.position;
            }
            GameObject heartInst = Instantiate(heart, new Vector2(heartSpawnerPos.x, heartSpawnerPos.y), Quaternion.identity);
            heartArray[i] = heartInst;

            if(i%2 == 0 && i != 0){
                rightSpace += 1;
                heartInst.transform.position = new Vector2(heartSpawnerPos.x-rightSpace, heartSpawnerPos.y);
            }else if(i%2 == 1 && i != 0){
                leftSpace += 1;
                heartInst.transform.position = new Vector2(heartSpawnerPos.x+leftSpace, heartSpawnerPos.y);
            }
            heartInst.transform.SetParent(heartSpawner.transform);
        }
    }

    void Update(){
        // CHARACTER BEHAVIOR
        if(characterScript.isOnLevel1 && enemyNb == 1){
            isAbleToMove = true;
            EnemyStats.SetActive(true);
        }else if(characterScript.isOnLevel2 && enemyNb == 2){
            isAbleToMove = true;
            EnemyStats.SetActive(true);
        }else if(characterScript.isOnLevel3 && enemyNb == 3){
            isAbleToMove = true;
            EnemyStats.SetActive(true);
        }else{
            EnemyStats.SetActive(false);
        }

        if(characterScript.isDead){
            enemySprite.color = new Color(1f, 1f, 1f, 1f);
            alertSound.Stop();
            ledSound.Stop();
        }

        // MOVEMENT
        if(direction.y > 0f){
            enemySprite.sprite = upSprite;
        }else if(direction.y < 0f){
            enemySprite.sprite = downSprite;
        }else if(direction.x > 0f){
            enemySprite.sprite = rightSprite;
        }else if(direction.x < 0f){
            enemySprite.sprite = leftSprite;
        }

        // SOUNDS
        dieSound.volume = PlayerPrefs.GetFloat("volume");
        hitSound.volume = PlayerPrefs.GetFloat("volume");
        disappearSound.volume = PlayerPrefs.GetFloat("volume");
        laughtSound.volume = PlayerPrefs.GetFloat("volume");
        ledSound.volume = PlayerPrefs.GetFloat("volume")/4f;
        alertSound.volume = PlayerPrefs.GetFloat("volume")/4f;

        if(!invulnerable && isAbleToMove){
            // MOVEMENT
            Vector2 selfSize = GetComponent<BoxCollider2D>().bounds.size;
            Vector2[] possibleDirections = new Vector2[8];

            RaycastHit2D upHit = Physics2D.Raycast(enemy.transform.position, new Vector2(0,1), selfSize.y, rayCastMask);
            RaycastHit2D rightHit = Physics2D.Raycast(enemy.transform.position, new Vector2(1,0), selfSize.x, rayCastMask);
            RaycastHit2D downHit = Physics2D.Raycast(enemy.transform.position, new Vector2(0,-1), selfSize.y, rayCastMask);
            RaycastHit2D leftHit = Physics2D.Raycast(enemy.transform.position, new Vector2(-1,0), selfSize.x, rayCastMask);

            Debug.DrawRay(enemy.transform.position, new Vector2(0, selfSize.y));
            Debug.DrawRay(enemy.transform.position, new Vector2(selfSize.x, 0));
            Debug.DrawRay(enemy.transform.position, new Vector2(0, -selfSize.y));
            Debug.DrawRay(enemy.transform.position, new Vector2(-selfSize.x, 0));

            int i = 0;
            if(upHit.collider == null){
                possibleDirections[i] = new Vector2(0,1);
                i += 1;
            }
            if(rightHit.collider == null){
                possibleDirections[i] = new Vector2(1,0);
                i += 1;
            }
            if(downHit.collider == null){
                possibleDirections[i] = new Vector2(0,-1);
                i += 1;
            }
            if(leftHit.collider == null){
                possibleDirections[i] = new Vector2(-1,0);
                i += 1;
            }

            if(upHit.collider == null && rightHit.collider == null){
                possibleDirections[i] = new Vector2(1,1);
                i += 1;
            }
            if(upHit.collider == null && leftHit.collider == null){
                possibleDirections[i] = new Vector2(-1,1);
                i += 1;
            }
            if(downHit.collider == null && rightHit.collider == null){
                possibleDirections[i] = new Vector2(1,-1);
                i += 1;
            }
            if(downHit.collider == null && leftHit.collider == null){
                possibleDirections[i] = new Vector2(-1,-1);
                i += 1;
            }

            bool keepGoing = false;
            for(int j = 0; j < i; j++) 
            {
                if(possibleDirections[j] == direction){
                    keepGoing = true;
                    break;
                }
            }

            if(!keepGoing){
                direction = possibleDirections[Random.Range(0, i)];
            }
        }
    }

    void FixedUpdate(){
        // MOVEMENT
        Vector2 newPos = rb2D.position;
        newPos += Time.fixedDeltaTime * speed * direction;
        if(direction == new Vector2(0f, 0f) && !invulnerable){
            direction = new Vector2(speedArray[Random.Range(0, 2)], Random.Range(0, 2));
        }
        if(isAbleToMove){
            rb2D.MovePosition(newPos);
        }
    }

    // void OnCollisionEnter2D(Collision2D col){
    //     // Debug.Log("Collision!");
    //     Vector2 oldDirection = direction;
    //     float newDirectionX = 0f;
    //     float newDirectionY = 0f;
    //     rb2D.AddForce(new Vector2(0f, 0f));

    //     if(oldDirection.x == 1f){
    //         newDirectionX = speedArray[Random.Range(0, 2)];
    //     }else if(oldDirection.x == -1f){ 
    //         newDirectionX = speedArray[Random.Range(1, 3)]; 
    //     }

    //     if(oldDirection.y == 1f){
    //         newDirectionY = speedArray[Random.Range(0, 2)];
    //     }else if(oldDirection.y == -1f){
    //         newDirectionY = speedArray[Random.Range(1, 3)];
    //     }

    //     direction = new Vector2(newDirectionX, newDirectionY);
    //     // direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
    // }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Flashlight" && !invulnerable){
            StartCoroutine("Run");
        }

        // DETECTOR
        if(col == orangeZone && col != redZone){
            detector.sprite = detectorOrange;
            ledSound.Play();
        }else if(col == redZone){
            detector.sprite = detectorRed;
            ledSound.Play();
            alertSound.Play();
        }else{
            detector.sprite = detectorNeutral;
            ledSound.Play();
        }
    }

    void OnTriggerExit2D(Collider2D col){
        // DETECTOR
        if(col == redZone){
            detector.sprite = detectorOrange;
            ledSound.Play();
            alertSound.Stop();
        }if(col == orangeZone){
            detector.sprite = detectorNeutral;
            ledSound.Play();
        }
    }

    IEnumerator Run(){
        deathField.SetActive(false);
        hitSound.Play();
        invulnerable = true;
        rb2D.AddForce(new Vector2(0f, 0f));
        direction = new Vector2(0f, 0f);
        enemySprite.color = new Color(1f, 1f, 1f, 1f);
        float oldSpeed = speed;

        yield return new WaitForSeconds(freezeDuration);

        Destroy(heartArray[lives-1]);
        lives -= 1;
        if(lives == 0){
            dieSound.Play();
            EnemyStats.SetActive(false);
            if(enemyNb == 1){
                characterScript.level1Done = true;
            }else if(enemyNb == 2){
                characterScript.level2Done = true;
            }else if(enemyNb == 3){
                characterScript.level3Done = true;
            }
            doorLock.Play();
            Destroy(enemy);
        }else{
            col2D.enabled = false;
            laughtSound.Play();
            direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
            if(direction == new Vector2(0,0)){
                direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
            }
            speed += runSpeed;

            yield return new WaitForSeconds(runDuration);

            disappearSound.Play();
            enemySprite.color = new Color(1f, 1f, 1f, 0f);
            rb2D.AddForce(new Vector2(0f, 0f));
            direction = new Vector2(0f, 0f);
            enemy.transform.position = spawn.transform.position;
            col2D.enabled = true;

            yield return new WaitForSeconds(respawnDuration);

            invulnerable = false;
            speed = oldSpeed;
            direction = new Vector2(speedArray[Random.Range(0, 2)], speedArray[Random.Range(0, 2)]);
            deathField.SetActive(true);
        }
    }

    
}
