using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseShipController : MonoBehaviour
{
    public static ChooseShipController instance = null;

    public int currentShipIndex = 0;

    public float shipRotationSpeed = 10f;

    public Text shipDescription;

    public GameObject ships;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void UpdateActiveShip() {
        foreach(Transform child in ships.transform) {
            child.gameObject.SetActive(false);
        }
        GameObject currentShip = ships.transform.GetChild(currentShipIndex).gameObject;
        currentShip.SetActive(true);
        shipDescription.text = currentShip.GetComponent<ShipSelection>().description;
    }

    void Start() {
        UpdateActiveShip();
    }

    public void NextShip() {
        currentShipIndex++;
        if(currentShipIndex == ships.transform.childCount) {
            currentShipIndex = 0;
        }
        UpdateActiveShip();
    }

    public void PreviousShip() {
        currentShipIndex--;
        if(currentShipIndex == -1) {
            currentShipIndex = ships.transform.childCount - 1;
        }
        UpdateActiveShip();
    }

    void Update() {
        ships.transform.Rotate(0, shipRotationSpeed * Time.deltaTime, 0);
    }

    public void StartGame() {
        GlobalVariables.Set("chosenShip", currentShipIndex);
        SceneManager.LoadScene("MainScene");
    }
}
