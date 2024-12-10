using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    public float boundaryX = 10f; // Set this to the x position limit

    void Update()
    {
        // Check if the enemy's x position exceeds the boundary
        if (Mathf.Abs(transform.position.x) > boundaryX)
        {
            Destroy(gameObject); // Destroy the enemy
        }
    }
}