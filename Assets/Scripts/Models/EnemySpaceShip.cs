using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceShip : MonoBehaviour
{
    public void Restart() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(10f, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    public void Move()
    {
        gameObject.transform.position -= Vector3.right * GameController.instance.enemySpaceShipSpeed;
    }

    public void TakeHit() {
        gameObject.SetActive(false);
        GameController.instance.OnEnemySpaceShipKilled();
    }
}
