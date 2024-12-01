using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public Button startBT;
    public Button optionBT;
    public Button shopBT;

    public Button homeBT;
    public Button RetryBT;

    UnityAction action;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            action = () => OnStartClick();
            startBT.onClick.AddListener(action);    //unityAction을 이용한 방식

            optionBT.onClick.AddListener(delegate { OnButtonClick(optionBT.name); });   //무명메서드 방식

            shopBT.onClick.AddListener(() => OnButtonClick(shopBT.name));   //람다식 이용방식
        }
        if (SceneManager.GetActiveScene().name == "Play_BackUp")
        {
            homeBT.onClick.AddListener(() => SceneManager.LoadScene("Main"));
            RetryBT.onClick.AddListener(() => SceneManager.LoadScene("Play_BackUp"));
        }
    }

    void OnButtonClick(string msg)
    {
        //Debug.Log("Click Button : " + msg);
    }
    
    public void OnStartClick()
    {
        SceneManager.LoadScene("Play_BackUp");
        //SceneManager.LoadScene("Level_01", LoadSceneMode.Additive);

    }
}
