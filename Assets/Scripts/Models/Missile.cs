using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private GameObject target;
    // Start is called before the first frame update
    public void SetTarget(GameObject target) {
        this.target = target;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target != null) {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, GameController.instance.missileSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Player") {
            Destroy(gameObject);
            GameController.instance.OnMissileHitCollider(other);
        }
    }
}
