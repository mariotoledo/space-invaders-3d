﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Transform bullet; 

    void Start()
    {
        bullet = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bullet.position += Vector3.up * GameController.instance.spaceShipBulletSpeed;

        if(GameController.instance.camera.WorldToScreenPoint(bullet.position).y >= Screen.height) {
            Destroy(gameObject);
            GameController.instance.OnBullerLeaveScreen();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Player") {
            Destroy(gameObject);
            GameController.instance.OnBulletHitCollider(other);
        }
    }
}
