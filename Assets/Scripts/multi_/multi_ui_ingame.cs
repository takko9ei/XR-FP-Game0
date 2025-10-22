using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static OVRHaptics;
using static Unity.Burst.Intrinsics.X86.Avx;

public class multi_ui_ingame : MonoBehaviour
{
    private bool isActive = false;

    public GameObject controllingCanvas;
    public Button ctn;
    public TextMeshProUGUI tmp;
    public GameObject gameManager;
    public TextMeshProUGUI win;
    public TextMeshProUGUI lose;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (getIsActive())
            {
                setActive(false);
            }
            else
            {
                setActive(true);
            }
        }
    }

    #region getset
    public void setIsActive(bool active)
    {
        if (active)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
    public bool getIsActive()
    {
        return isActive;
    }
    public void setActive(bool active)
    {
        if (active)
        {
            setIsActive(true);
            getControllingCanvas().SetActive(true); 
        }
        else
        {
            setIsActive(false);
            getControllingCanvas().SetActive(false);
        }
    }
    public GameObject getControllingCanvas()
    {
        return controllingCanvas;
    }
    #endregion

    #region button methods
    public void continueGame()
    {
        //dosomething
        setActive(false);
    }

    public void backLobby()
    {
        //disconect
        //back main menu
    }
    #endregion

    #region private methods
    private void disconnectRoom()
    {

    }
    #endregion

    public void showWinUI()
    {

    }

    public void showLoseUI()
    {

    }
    
    private void showUI()
    {
        setActive(true);
        ctn.gameObject.SetActive(false);
        tmp.gameObject.SetActive(true);
        displayKill();
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

    private int[] getCount()
    {
        return gameManager.GetComponent<multi_gamecore>().getTotalKills();
    }
}
