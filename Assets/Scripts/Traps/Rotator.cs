using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        RotatorTrap();
    }
    public void RotatorTrap()
    {
        transform.Rotate(0, speed, 0);
    }
}
