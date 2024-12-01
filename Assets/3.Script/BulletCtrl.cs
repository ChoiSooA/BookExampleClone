using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    //ÃÑ¾Ë µ¥¹ÌÁö
    public float damege = 20.0f;
    //ÃÑ¾Ë ¹ß»ç Èû(¼Óµµ)
    public float force = 2000f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * force);
    }
}
