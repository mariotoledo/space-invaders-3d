using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public float enemySpeed = 0.1f;
    public float enemyBulletSpeed = 0.01f;
    public float enemyShootingFrequency = 3f;
    public float enemySpaceShipFrequency = 15f;
    public float enemySpaceShipSpeed = 0.1f;
    public float stageDifficultMultiplier = 1.4f;

    public List<PlayerShipConfig> playerShipConfigs;

    public int maxBarrierLifes = 3;
    public int maxEnemyLifes = 1;
    public int newStageTextSecondsDelay = 3;

    public float minScreenBound;
    public float maxScreenBound;

    public int points = 0;
    public int normalEnemyPoints = 100;
    public int specialEnemyPoints = 300;

    private int stage = 0;
    private int currentPlayerLifes = 0;

    private int enemyDeathCount = 0;
    public int missileTriggerCount = 3;

    public float missileSpeed = 0.01f;

    public Player player;
    public Camera camera;

    private GameState gameState;

    public EnemyGroup enemyGroup;
    public EnemySpaceShip enemySpaceShip;

    public GameObject gameOverText;
    public GameObject stageText;
    public GameObject lifesText;
    public GameObject pointsText;
    private GameObject missileTarget;
    private int playerShootCount = 0;
    public GameObject targetImage;

    private PlayerShipConfig currentPlayerShipConfig;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public PlayerShipConfig GetChosenShipConfig() {
        return currentPlayerShipConfig;
    }

    void FindMissileTarget() {
        missileTarget = enemyGroup.FindClosestEnemy();
        targetImage.SetActive(true);
    }

    void Start() {
        int chosenShip = GlobalVariables.Get<int>("chosenShip");
        currentPlayerShipConfig = playerShipConfigs[chosenShip];
        currentPlayerLifes = currentPlayerShipConfig.maxPlayerLifes;
        lifesText.GetComponent<Text>().text = "Lifes: " + currentPlayerLifes;
        CalculateScreenBounds();
        StartNewStage();
    }

    void StartNewStage() {
        CancelInvoke();
        enemyDeathCount = 0;
        enemyGroup.CreateEnemies();
        gameState = GameState.Starting;
        stage++;
        enemySpeed = enemySpeed * stageDifficultMultiplier;
        stageText.GetComponent<Text>().text = "Stage " + stage;
        stageText.SetActive(true);

        Invoke("RunStage", newStageTextSecondsDelay);
        InvokeRepeating("InitializeEnemySpaceShip", enemySpaceShipFrequency, enemySpaceShipFrequency);
        InvokeRepeating("EnemyShoot", enemyShootingFrequency, enemyShootingFrequency);
    }

    void EnemyShoot() {
        enemyGroup.Shoot();
    }

    void RunStage() {
        stageText.SetActive(false);
        gameState = GameState.Running;
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(3000);
    }

    void Update() {
        UpdatePlayerShot();
        UpdatePlayerMissileShot();
    }

    void FixedUpdate() {
        if(gameState == GameState.Running) {
            UpdatePlayerMovement();
            UpdatePlayerShot();
            UpdateEnemyMovement();
            enemySpaceShip.Move();

            if(missileTarget != null) {
                targetImage.transform.position = missileTarget.transform.position;
            }
        } else if (gameState == GameState.GameOver) {
            if(Input.GetKeyDown(KeyCode.Space) == true) {
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
        if(Input.GetKeyDown(KeyCode.Space) == true) {
            player.Shoot();
        }
    }

    void UpdatePlayerMissileShot() {
        if (Input.GetKeyDown(KeyCode.LeftControl) == true && targetImage.gameObject.activeSelf){
            player.LaunchMissile(missileTarget);
            targetImage.SetActive(false);
        }
    }

    public void InitializeEnemySpaceShip() {
        enemySpaceShip.Restart();
    }

    public void OnBulletHitCollider(Collider2D collider) {
        EvaluateCollision(collider);
        player.canShoot = true;
    }

    public void OnMissileHitCollider(Collider2D collider) {
        EvaluateCollision(collider);
        player.canShootMissile = true;
    }

    private void EvaluateCollision(Collider2D collider) {
        if(collider.tag == "BarrierBlock") {
            collider.gameObject.GetComponent<BarrierBlock>().TakeHit();
        } else if (collider.tag == "Enemy") {
            if(collider.gameObject == missileTarget) {
                targetImage.SetActive(false);
            }
            collider.gameObject.GetComponent<Enemy>().TakeHit();
        } else if (collider.tag == "EnemySpaceShip") {
            collider.gameObject.GetComponent<EnemySpaceShip>().TakeHit();
        }
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
    }

    public void OnBulletLeaveScreen() {
        player.canShoot = true;
    }

    public void OnEnemySpaceShipKilled() {
        AddPoints(specialEnemyPoints);
    }

    public void OnPlayerFireBullet() {
        playerShootCount++;

        if(playerShootCount == missileTriggerCount) {
            playerShootCount = 0;
            FindMissileTarget();
        }
    }

    private void AddPoints (int points) {
        this.points += points;
        pointsText.GetComponent<Text>().text = "Points: " + this.points;
    }

    public void OnEnemyKilled() {
        enemyDeathCount++;
        AddPoints(normalEnemyPoints);

        if(enemyDeathCount == enemyGroup.enemySpawn.Count) {
            StartNewStage();
        }
    }

    public void GameOver() {
        gameState = GameState.GameOver;
        gameOverText.SetActive(true);
        CancelInvoke();
    }
}