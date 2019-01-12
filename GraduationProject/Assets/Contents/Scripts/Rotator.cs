using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Transform m_trans;

    public float Rotate_X;

    public float Rotate_Y;

    public float Rotate_Z;

    public Rotator()
    {
    }

    private void Start()
    {
   
    }

    private void FixedUpdate()
    {
        if (this.Rotate_X != 0f || this.Rotate_Y != 0f || this.Rotate_Z != 0f)
        {
            transform.Rotate(new Vector3(this.Rotate_X * Time.deltaTime, this.Rotate_Y * Time.deltaTime, this.Rotate_Z * Time.deltaTime));
        }
    }
}