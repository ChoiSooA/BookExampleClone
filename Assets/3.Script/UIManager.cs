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
            startBT.onClick.AddListener(action);    //unityAction�� �̿��� ���

            optionBT.onClick.AddListener(delegate { OnButtonClick(optionBT.name); });   //����޼��� ���

            shopBT.onClick.AddListener(() => OnButtonClick(shopBT.name));   //���ٽ� �̿���
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
