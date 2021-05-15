using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float spaceShipSpeed = 0.01f;

    private float minScreenBound;
    private float maxScreenBound;

    public Player player;
    public Camera camera;

    void Start() {
        CalculateScreenBounds();
    }

    void Update()
    {
        UpdatePlayerMovement();
    }

    /*
        Calculates screen bounds based on Screen height for 1:1 perspective
    */
    void CalculateScreenBounds() {
        float middleScreenPoint = Screen.width / 2;

        minScreenBound = middleScreenPoint - (Screen.height / 2);
        maxScreenBound = middleScreenPoint + (Screen.height / 2);
    }

    void UpdatePlayerMovement() {
        float horizontalTrigger = Input.GetAxis("Horizontal");

        float playerPositionScreenX = camera.WorldToScreenPoint(player.GetPosition()).x;

        if((playerPositionScreenX < minScreenBound && horizontalTrigger < 0) || (playerPositionScreenX > maxScreenBound && horizontalTrigger > 0)) {
            horizontalTrigger = 0;
        }

        player.SetPosition(player.GetPosition() + (Vector3.right * horizontalTrigger * spaceShipSpeed));
    }
}
