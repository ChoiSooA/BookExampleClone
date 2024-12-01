using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class PlayerCtrl : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    Animation anim;

    public float moveSpeed = 10.0f;
    public float turnSpeed = 800f;
    public float jumpForce = 30f;

    public int monsterDamage = 10;

    private readonly float initHP = 100f;
    //private float currHP;

    public TMP_Text scoreText;
    private int totScore = 0;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    public GameObject gameOverPannel;

    IEnumerator Start()
    {
        GameManager.instance.currHP = initHP;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();

        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);

        GameManager.instance.CreateMonsterPool();

        anim.Play("Idle");

        DisplayScore(0);

        //시작하고 화면 회전을 lock해주고 딜레이 줘서 화면 위치 고정할 수 있게 해줌
        turnSpeed = 0f;
        yield return new WaitForSeconds(0.3f);

        GameManager.instance.GameManagerStart();
        turnSpeed = 400f;

    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir * Time.deltaTime  * moveSpeed);
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        PlayerAnim(h, v);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up* jumpForce, ForceMode.Force);
        }
    }

    

    void PlayerAnim(float h, float v)
    {
        if (v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f);
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f);
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.currHP >= 0f && other.CompareTag("PUNCH"))
        {
            GameManager.instance.currHP -= monsterDamage;
            //Debug.Log("Player HP = " + currHP + "/" + initHP);
            GameManager.instance.DisplayHealth();
            if (GameManager.instance.currHP <= 0f)
            {
                PlayerDie();
            }
        }
        if (other.CompareTag("HP_ITEM"))
        {
            //먹는 소리 뾰로롱 이런거
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die ! ");

        OnPlayerDie();

        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver=true;

        GameManager.instance.IsGameOver = true;

        gameOverPannel.SetActive(true);

        GameManager.instance.monsterPool.Clear();
        GameManager.instance.itemPool.Clear();
        GameManager.instance.points.Clear();


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        this.enabled = false;

    }
    public void DisplayScore(int score)
    {
        totScore += score;
        scoreText.text = $"<color=#00ff00>Score : </color><color=#ff0000>{totScore:#,##0}</color>";

        PlayerPrefs.GetInt("TOT_SCORE", totScore);
    }

}
