using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPosition {
    public Vector3 position;
    public GameObject enemy;
}

public class EnemyGroup : MonoBehaviour
{
    private Transform enemyGroup;

    public List<Vector3> enemySpawnPositions;

    [SerializeField]
    public List<EnemyPosition> enemySpawn;

    public GameObject loseLine;

    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemyGroup = GetComponent<Transform>();
    }

    public void CreateEnemies() {
        foreach(EnemyPosition enemySpawnItem in enemySpawn) {
            GameObject enemy = Instantiate(enemySpawnItem.enemy, enemySpawnItem.position, enemyPrefab.transform.rotation);
            enemy.transform.SetParent(gameObject.transform);
        }
    }

    // Update is called once per frame
    public void MoveEnemies() {
        enemyGroup.position += Vector3.right * GameController.instance.enemySpeed;

        foreach(Transform child in enemyGroup) {
            if(child.tag == "Enemy") {
                Transform enemy = child;
                float enemyPositionScreenX = GameController.instance.camera.WorldToScreenPoint(enemy.transform.position).x;
                if(enemyPositionScreenX < GameController.instance.minScreenBound || 
                    enemyPositionScreenX > GameController.instance.maxScreenBound) {
                    GameController.instance.enemySpeed *= -1;
                    enemyGroup.position += Vector3.down * 0.5f;
                    return;
                }

                if(enemy.position.y < loseLine.transform.position.y) {
                    GameController.instance.GameOver();
                }
            }
        }
    }
}
