using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public float spaceShipSpeed = 0.01f;
    public float spaceShipBulletSpeed = 0.01f;

    public int maxBarrierLifes = 3;

    private float minScreenBound;
    private float maxScreenBound;

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

        float playerPositionScreenX = camera.WorldToScreenPoint(player.GetPosition()).x;

        if((playerPositionScreenX < minScreenBound && horizontalTrigger < 0) || (playerPositionScreenX > maxScreenBound && horizontalTrigger > 0)) {
            horizontalTrigger = 0;
        }

        player.SetPosition(player.GetPosition() + (Vector3.right * horizontalTrigger * spaceShipSpeed));
    }

    void UpdatePlayerShot() {
        if(Input.GetButton("Fire1")) {
            player.Shoot();
        }
    }

    public void OnBulletHitCollider(Collider2D collider) {
        if(collider.tag == "BarrierBlock") {
            collider.gameObject.GetComponent<BarrierBlock>().TakeHit();
        }
        player.canShoot = true;
    }

    public void OnBullerLeaveScreen() {
        player.canShoot = true;
    }
}
