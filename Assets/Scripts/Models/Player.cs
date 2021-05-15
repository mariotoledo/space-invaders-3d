using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform playerPosition;

    void Start()
    {
        playerPosition = GetComponent<Transform>();
    }

    public Vector3 GetPosition() {
        return playerPosition.position;
    }

    public void SetPosition(Vector3 position) {
        playerPosition.position = position;
    }
}
