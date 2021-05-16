using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Transform bullet;

    public delegate void PlayerBulletDestroyedDelegate();
    public static event PlayerBulletDestroyedDelegate OnBulletDestroyed;   

    void Start()
    {
        bullet = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameController.instance);
        bullet.position += Vector3.up * GameController.instance.spaceShipBulletSpeed;

        Debug.Log(GameController.instance.camera.WorldToScreenPoint(bullet.position).y);

        if(GameController.instance.camera.WorldToScreenPoint(bullet.position).y >= Screen.height) {
            Destroy(gameObject);
            OnBulletDestroyed();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Enemy") {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
        OnBulletDestroyed();
    }
}
