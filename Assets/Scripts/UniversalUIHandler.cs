using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UniversalUIHandler : MonoBehaviour
{
    public GameObject cvs;//the canvas, use for show and hide
    private bool currentMode;
    public Slider sld;
    public Image spr0;
    public Image spr1;
    private bool isActive = true;
    public Camera sceneCamera;

    public TextMeshProUGUI win;
    public TextMeshProUGUI lose;
    public TextMeshProUGUI tmp;
    public GameObject gameManager;
    public Button conti;

    //need to be deleted
    //public Text difficultttt;
    //public Text hello;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (isActive)
            {
                setActiveFalse();
            }
            else
            {
                setActiveTrue();
            }
        }

        //need delete
        //difficultttt.text = getSliderVal().ToString();
    }

    void startSingleMode(int diff)
    {
        //GameObject.Find("MsgTransport");
        
        //hadness = diff
        //GameObject.Find("MsgTransport").GetComponent<MsgTransportScript>().difficulty = diff;
        SceneManager.LoadScene("single");
        //hello.text = "hello";
    }

    void startMultiMode(int diff)
    {
        //GameObject.Find("MsgTransport");
        //hardness = diff
        //GameObject.Find("MsgTransport").GetComponent<MsgTransportScript>().difficulty = diff;
        SceneManager.LoadScene("multi_lobby");
    }

    int getSliderVal()
    {
        int val = 0;
        val = (int)sld.value;
        return val;
    }

    public void changeCurrentModeSingle()
    {
        currentMode = false;
        displayCurrentMode();
    }

    public void changeCurrentModeMulti()
    {
        currentMode = true;
        displayCurrentMode();
    }

    void displayCurrentMode()
    {
        if (!currentMode)
        {
            //display sprite0
            spr1.gameObject.SetActive(false);
            spr0.gameObject.SetActive(true);
        }
        else
        {
            //display sprite1
            spr1.gameObject.SetActive(true);
            spr0.gameObject.SetActive(false);
        }
    }

    public void confirmAndStart()
    {
        if (!currentMode)
        {
            
            startSingleMode(getSliderVal());
        }
        else
        {
            startMultiMode(getSliderVal());
        }
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void setActiveFalse()
    {
        cvs.SetActive(false);
        isActive = false;
    }

    public void backMainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void showUI()
    {
        setActiveTrue();
        conti.gameObject.SetActive(false);
        tmp.gameObject.SetActive(true);
        displayKill();
    }

    public void showWinUI()
    {
        showUI();
        win.gameObject.SetActive(true);
        
    }

    public void showLoseUI()
    {
        showUI();
        lose.gameObject.SetActive(true);
        
    }

    private int[] getCount()
    {
        return gameManager.GetComponent<GameManagement>().getTotalKills();
    }

    private void displayKill()
    {
        int a = getCount()[0];
        int b = getCount()[1];
        int c = getCount()[2];

        StringBuilder sb = new StringBuilder();
        sb.Append("Kills:\n");
        sb.Append("Noob:");
        sb.Append(a.ToString());
        sb.Append("\n");
        sb.Append("Skilled:");
        sb.Append(b.ToString());
        sb.Append("\n");
        sb.Append("Elite:");
        sb.Append(c.ToString());

        tmp.text = sb.ToString();
    }

    private void setActiveTrue()
    {
        cvs.SetActive(true);
        isActive = true;
    }

}
