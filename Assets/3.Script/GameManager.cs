using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    public List<GameObject> monsterPool = new List<GameObject>();
    public List<GameObject> itemPool = new List<GameObject>();

    public int maxMonster = 10;

    public GameObject monster;
    public GameObject item;

    public float createTime = 3f;

    bool isGameOver;


    public float currHP;
    private Image hpBar;

    public bool IsGameOver
    {
        get { return isGameOver; }
        set {
            isGameOver = value;
            if (isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }


    public void GameManagerStart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }
        InvokeRepeating("CreateMonster", 2f, createTime);
        InvokeRepeating("CreateItem", 2f, 10f);
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();
        Quaternion q = Quaternion.Euler(new Vector3(points[idx].rotation.x, Random.Range(0f,360f), points[idx].rotation.z));

        _monster?.transform.SetPositionAndRotation(points[idx].position, q);
        _monster?.SetActive(true);
    }
    void CreateItem()
    {
        int idx = Random.Range(0, points.Count);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _item = GetItemInPool();

        _item?.transform.SetPositionAndRotation(
            new Vector3(points[idx].position.x, 1, points[idx].position.z), points[idx].rotation);
        _item?.SetActive(true);
    }

    public void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonster; i++)
        {
            var _monster = Instantiate<GameObject>(monster);
            _monster.name = "Monster_" + i;
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
        for (int i = 0; i < maxMonster; i++)
        {
            var _item = Instantiate<GameObject>(item);
            _item.name = "item" + i;
            _item.SetActive(false);

            itemPool.Add(_item);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            if (_monster.activeSelf == false)
            {
                return _monster;
            }
        }
        return null;
    }
    public GameObject GetItemInPool()
    {
        foreach (var _item in itemPool)
        {
            if (_item.activeSelf == false)
            {
                return _item;
            }
        }
        return null;
    }

    public void DisplayHealth()
    {
        hpBar.fillAmount = GameManager.instance.currHP / 120;
    }


}