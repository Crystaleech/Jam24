using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTexts : MonoBehaviour

{
    public GameObject pickupText1;
    public GameObject pickupText2;
    public float detectionRange = 2f;

    private GameObject player1;
    private GameObject player2;

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("P1");
        player2 = GameObject.FindGameObjectWithTag("P2");

        // Hide both pickup texts at the start
        pickupText1.SetActive(false);
        pickupText2.SetActive(false);
    }

    private void Update()
    {
        // Check distance to Player 1 (P1)
        float distanceToP1 = Vector2.Distance(transform.position, player1.transform.position);
        if (distanceToP1 <= detectionRange)
        {
            pickupText1.SetActive(true);
        }
        else
        {
            pickupText1.SetActive(false);
        }

        // Check distance to Player 2 (P2)
        float distanceToP2 = Vector2.Distance(transform.position, player2.transform.position);
        if (distanceToP2 <= detectionRange)
        {
            pickupText2.SetActive(true);
        }
        else
        {
            pickupText2.SetActive(false);
        }
    }
}
