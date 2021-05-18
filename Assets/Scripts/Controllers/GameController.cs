using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public float spaceShipSpeed = 0.01f;
    public float spaceShipBulletSpeed = 0.01f;
    public float enemySpeed = 0.1f;
    public float enemyBulletSpeed = 0.01f;
    public float enemyShootingFrequency = 3f;
    public float stageDifficultMultiplier = 1.3f;

    public int maxBarrierLifes = 3;
    public int maxPlayerLifes = 3;
    public int maxEnemyLifes = 1;
    public int newStageTextSecondsDelay = 3;

    public float minScreenBound;
    public float maxScreenBound;

    public int points = 0;

    private int stage = 0;
    private int currentPlayerLifes = 0;

    private int enemyDeathCount = 0;

    public Player player;
    public Camera camera;

    private GameState gameState;

    public EnemyGroup enemyGroup;

    public GameObject gameOverText;
    public GameObject stageText;
    public GameObject lifesText;
    public GameObject pointsText;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start() {
        currentPlayerLifes = maxPlayerLifes;
        stageText.GetComponent<Text>().text = "Lifes: " + currentPlayerLifes;
        CalculateScreenBounds();
        StartNewStage();
    }

    void StartNewStage() {
        enemyDeathCount = 0;
        enemyGroup.CreateEnemies();
        gameState = GameState.Starting;
        stage++;
        enemySpeed = enemySpeed * stageDifficultMultiplier;
        stageText.GetComponent<Text>().text = "Stage " + stage;
        stageText.SetActive(true);

        Invoke("RunStage", newStageTextSecondsDelay);
    }

    void RunStage() {
        stageText.SetActive(false);
        gameState = GameState.Running;
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(3000);
    }

    void FixedUpdate() {
        if(gameState == GameState.Running) {
            UpdatePlayerMovement();
            UpdatePlayerShot();
            UpdateEnemyMovement();
        } else if (gameState == GameState.GameOver) {
            if(Input.GetButton("Fire1")) {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    /*
        Calculates screen bounds based on Screen height for 1:1 perspective
    */
    void CalculateScreenBounds() {
        float middleScreenPoint = Screen.width / 2;

        minScreenBound = middleScreenPoint - (Screen.height / 2);
        maxScreenBound = middleScreenPoint + (Screen.height / 2);
    }

    void UpdatePlayerMovement() {
        float horizontalTrigger = Input.GetAxis("Horizontal");
        player.Move(horizontalTrigger);
    }

    void UpdateEnemyMovement() {
        enemyGroup.MoveEnemies();
    }

    void UpdatePlayerShot() {
        if(Input.GetButton("Fire1")) {
            player.Shoot();
        }
    }

    public void OnBulletHitCollider(Collider2D collider) {
        if(collider.tag == "BarrierBlock") {
            collider.gameObject.GetComponent<BarrierBlock>().TakeHit();
        } else if (collider.tag == "Enemy") {
            collider.gameObject.GetComponent<Enemy>().TakeHit();
        }
        player.canShoot = true;
    }

    public void OnEnemyBulletHitCollider(Collider2D collider) {
        if(collider.tag == "BarrierBlock") {
            collider.gameObject.GetComponent<BarrierBlock>().TakeHit();
        } else if (collider.tag == "Player") {
            currentPlayerLifes--;
            lifesText.GetComponent<Text>().text = "Lifes: " + currentPlayerLifes;

            if(currentPlayerLifes == 0) {
                GameOver();
            }
        }
        player.canShoot = true;
    }

    public void OnBulletLeaveScreen() {
        player.canShoot = true;
    }

    public void OnEnemyKilled() {
        enemyDeathCount++;
        points += 100;

        pointsText.GetComponent<Text>().text = "Points: " + points;

        if(enemyDeathCount == enemyGroup.enemySpawn.Count) {
            StartNewStage();
        }
    }

    public void GameOver() {
        gameState = GameState.GameOver;
        gameOverText.SetActive(true);
    }
}
