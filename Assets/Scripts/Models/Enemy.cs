using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int lifes;

    public void Start() {
        lifes = GameController.instance.maxEnemyLifes;
    }
    
    public void TakeHit() {
        lifes--;

        if(lifes == 0) {
            Destroy(gameObject);
        }
    }
}
