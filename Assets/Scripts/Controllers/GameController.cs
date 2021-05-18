using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public float spaceShipSpeed = 0.01f;
    public float spaceShipBulletSpeed = 0.01f;
    public float enemySpeed = 0.1f;
    public float enemyMoveFrequency = 0.1f;

    public int maxBarrierLifes = 3;
    public int maxPlayerLifes = 3;
    public int maxEnemyLifes = 1;

    public float minScreenBound;
    public float maxScreenBound;

    public int points = 0;

    public Player player;
    public Camera camera;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start() {
        CalculateScreenBounds();
    }

    void FixedUpdate()
    {
        UpdatePlayerMovement();
        UpdatePlayerShot();
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
            points += 100;
        }
        player.canShoot = true;
    }

    public void OnBulletLeaveScreen() {
        player.canShoot = true;
    }

    public void GameOver() {
        Time.timeScale = 0;
    }
}
