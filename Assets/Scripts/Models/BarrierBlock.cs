using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBlock : MonoBehaviour
{
    public int numberOfLives;

    // Start is called before the first frame update
    void Start()
    {
        numberOfLives = GameController.instance.maxBarrierLifes;
    }

    // Update is called once per frame
    public void TakeHit() {
        numberOfLives--;

        Debug.Log("numberOfLives: " + numberOfLives);

        Color materialColor = GetComponent<MeshRenderer>().material.color;
        materialColor.a = (float)numberOfLives / (float)GameController.instance.maxBarrierLifes;
        Debug.Log(materialColor.a);
        GetComponent<MeshRenderer>().material.color = materialColor;

        if(numberOfLives == 0) {
            Destroy(gameObject);
        }
    }
}
