using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    //�Ѿ� ������
    public float damege = 20.0f;
    //�Ѿ� �߻� ��(�ӵ�)
    public float force = 2000f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * force);
    }
}
