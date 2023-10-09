using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [HideInInspector] Vector2 input;

    void Update()
    {
        input = joystick.Direction;
        if(input != Vector2.zero)
        {
            Debug.Log(input);
        }
    }
}
