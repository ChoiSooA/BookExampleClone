using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;

    public AudioClip fireSfx;

    new AudioSource audio;
    MeshRenderer muzzleFlash;

    RaycastHit hit;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }

    private void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10f, Color.green);
        if (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.F))
        {
            Fire();

            if(Physics.Raycast(firePos.position, firePos.forward, out hit, 100f))
            {
                //Debug.Log("Hit = " + hit.transform.name);
                hit.transform.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);
            }
        }

    }
    
    void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        audio.PlayOneShot(fireSfx, 1f);
        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        muzzleFlash.material.mainTextureOffset = offset;
        //texture의 tilling을 0.5배로 해둬서 4조각 나있음
        //그걸 0.5이동시켜서 옆면보고 밑면보고 하는 거
        //매번 다른 모양

        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
        //매번 다른 모양의 회전값도 매번 다르게

        float scale = Random.Range(0f, 2f);
        muzzleFlash.transform.localScale = Vector3.one * scale;
        //스케일도 매번 다르게

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;
    }
}
