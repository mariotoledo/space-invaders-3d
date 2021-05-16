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
}
