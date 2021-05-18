using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform bullet; 

    void Start()
    {
        bullet = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        bullet.position += Vector3.down * GameController.instance.enemyBulletSpeed;

        if(GameController.instance.camera.WorldToScreenPoint(bullet.position).y < 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Enemy") {
            Destroy(gameObject);
            GameController.instance.OnEnemyBulletHitCollider(other);
        }
    }
}
