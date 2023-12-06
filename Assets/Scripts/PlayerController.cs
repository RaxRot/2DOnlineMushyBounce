using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Control Settings")] 
    [SerializeField] private float moveSpeed=10;
    [SerializeField] private float xBorder =4.5f;
    private float _clickedScreenX;
    private float _clickedPlayerX;

    private void Start()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = -1.5f;
        transform.position = newPosition;
    }

    private void Update()
    {
        ManageControl();
    }

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _clickedScreenX = Input.mousePosition.x;
            _clickedPlayerX = transform.position.x;
        }else if (Input.GetMouseButton(0))
        {
            float xDifference = Input.mousePosition.x - _clickedScreenX;
            xDifference /= Screen.width;
            xDifference *= moveSpeed;

            float newXPosition = _clickedPlayerX + xDifference;
            newXPosition = Mathf.Clamp(newXPosition, -xBorder, xBorder);
            transform.position = new Vector2(newXPosition, transform.position.y);
        }
    }
}
