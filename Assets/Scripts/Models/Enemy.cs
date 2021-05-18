using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int lifes;

    public EnemyBullet enemyBullet;

    public void Start() {
        lifes = GameController.instance.maxEnemyLifes;
    }

    public void Shoot() {
        Instantiate(enemyBullet.gameObject, gameObject.transform.position, gameObject.transform.rotation);
    }
    
    public void TakeHit() {
        lifes--;

        if(lifes == 0) {
            Destroy(gameObject);
            GameController.instance.OnEnemyKilled();
        }
    }
}
