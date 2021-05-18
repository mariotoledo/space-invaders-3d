using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    private Transform playerPosition;
    public PlayerBullet bullet;

    public bool canShoot = true;

    void Start()
    {
        playerPosition = GetComponent<Transform>();
    }

    public Vector3 GetPosition() {
        return playerPosition.position;
    }

    public void SetPosition(Vector3 position) {
        playerPosition.position = position;
    }

    public void Shoot() {
        if(canShoot == true) {
            Instantiate(bullet.gameObject, playerPosition.position, playerPosition.rotation);
            canShoot = false;
        }
    }

    public void Move(float horizontalTrigger) {
        float playerPositionScreenX = GameController.instance.camera.WorldToScreenPoint(GetPosition()).x;

        if(
            (playerPositionScreenX < GameController.instance.minScreenBound && horizontalTrigger < 0) || 
            (playerPositionScreenX > GameController.instance.maxScreenBound && horizontalTrigger > 0)) {
            horizontalTrigger = 0;
        }

        SetPosition(GetPosition() + (Vector3.right * horizontalTrigger * GameController.instance.spaceShipSpeed));
    }
}
