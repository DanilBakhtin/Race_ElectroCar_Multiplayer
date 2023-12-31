using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UI_Controller : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private Text textTimeFinish;
    [SerializeField] private Text textPlacePlayer;

    private bool isPause;
    private bool isFinish;
    private bool isFail;

    void Start()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
        Time.timeScale = 1;

        isFail = false;
        failPanel.SetActive(isFail);

        isFinish = false;
        finishPanel.SetActive(isFinish);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isFinish && !isFail)
        {   
            
            isPause = !isPause;
            pausePanel.SetActive(isPause);
            //Time.timeScale = 0;
        }
    }
    public void ContinueGame()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
        //Time.timeScale = 1;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void toMainMenu()
    {
        Time.timeScale = 1;
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(0);
    }
    public void nextLevel()
    {

        if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            toMainMenu();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void finishTrace(float time)
    {
        //Time.timeScale = 0;
        isFinish = true;
        finishPanel.SetActive(true);
        textTimeFinish.text = time.ToString("F2") + " s";

    }
    public void failTrace()
    {
        //Time.timeScale = 0;
        isFail = true;
        failPanel.SetActive(true);
    }
    public void toStartLevel()
    {

    }

    public void finishMultyplayer(int place)
    {
        isFinish = true;
        finishPanel.SetActive(true);
        textPlacePlayer.text = "�� ������ " + place + " �����!";

    }
}
