using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;
    public Texture[] textures;
    public float radius = 15f;
    new MeshRenderer renderer;

    public LayerMask layerMask;

    Transform tr;
    Rigidbody rb;

    int hitCount = 0;

    Collider[] colls = new Collider[10];

    private void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        renderer = GetComponentInChildren<MeshRenderer>();

        int idx = Random.Range(0, textures.Length);

        renderer.material.mainTexture = textures[idx];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            collision.gameObject.SetActive(false);
            if (++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);

        IndirectDamage(tr.position);

        Destroy(exp, 1.0f);

        Destroy(gameObject, 3.0f);
    }


    void IndirectDamage(Vector3 pos)
    {
        colls =  Physics.OverlapSphere(pos, radius/*, layerMask*/);


        foreach(var coll in colls)
        {
            if (coll.gameObject.CompareTag("BARREL"))
            {
                //Debug.Log(coll.name);
                rb = coll.GetComponent<Rigidbody>();
                rb.mass = 1.0f;
                rb.constraints = RigidbodyConstraints.None;
                rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
                rb.mass = 20.0f;
            }
            if (coll.gameObject.CompareTag("MONSTER"))
            {
                coll.gameObject.GetComponent<MonsterCtrl>().Damage(1000);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(this.transform.position, radius);
    }
}
