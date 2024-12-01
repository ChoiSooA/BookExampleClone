using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTr;
    private Transform camTr;

    public float damping = 10.0f;

    [Range(2.0f, 20f)]
    public float distance = 10.0f;

    [Range(0f, 10f)]
    public float height = 2.0f;

    Vector3 velocity = Vector3.zero;

    public float targetOffset = 2f;

    private void Start()
    {
        camTr = GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        float r_ = Input.GetAxis("Mouse Y");

        Vector3 pos = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height) + new Vector3(0,r_,0);

        //camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        //camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime * damping);

        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);

        camTr.LookAt(targetTr.position+(targetTr.up*targetOffset));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MeshRenderer>() != null)
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //Debug.Log(other.name + " hit");
        }else if (other.transform.GetComponentInChildren<MeshRenderer>() != null)
        {
            other.transform.GetComponentInChildren<MeshRenderer>().enabled = false;
            //Debug.Log(other.name + " hit");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MeshRenderer>() != null)
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (other.transform.GetComponentInChildren<MeshRenderer>() != null)
        {
            other.transform.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
}
