using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EnemyPosition {
    public Vector3 position;
    public GameObject enemy;
}

public class EnemyGroup : MonoBehaviour
{
    private Transform enemyGroup;

    [SerializeField]
    public List<EnemyPosition> enemySpawn;

    public GameObject loseLine;

    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        enemyGroup = GetComponent<Transform>();
        InvokeRepeating("Shoot", GameController.instance.enemyShootingFrequency, GameController.instance.enemyShootingFrequency);
    }

    void Shoot() {
        List<GameObject> allChildren = enemyGroup.Cast<Transform>().Select(t=>t.gameObject).ToList();

        GameObject closestEnemy = allChildren.Aggregate((curMin, x) => 
            (curMin == null || x.transform.position.y < curMin.transform.position.y ? x : curMin));

        if(closestEnemy) {
            GameObject[] closestEnemies = allChildren.Where(enemy => enemy.transform.position.y == closestEnemy.transform.position.y).ToArray();

            int randomEnemyIndex = new System.Random().Next(0, closestEnemies.Length);
            closestEnemies[randomEnemyIndex].GetComponent<Enemy>().Shoot();
        }
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
