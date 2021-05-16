using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBlock : MonoBehaviour
{
    public int numberOfLives;

    private Material currentMaterial;

    void Start()
    {
        numberOfLives = GameController.instance.maxBarrierLifes;
        currentMaterial = gameObject.GetComponent<Renderer>().material;
    }

    public void TakeHit() {
        numberOfLives--;

        Color oldColor = currentMaterial.color;
        float colorFrequency = (float)numberOfLives / (float)GameController.instance.maxBarrierLifes;
        Color newColor = new Color(colorFrequency, colorFrequency, colorFrequency, colorFrequency);
        currentMaterial.SetColor("_Color", newColor);

        if(numberOfLives == 0) {
            Destroy(gameObject);
        }
    }
}
