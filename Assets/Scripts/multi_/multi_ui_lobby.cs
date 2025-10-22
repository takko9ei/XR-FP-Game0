using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multi_ui_lobby : MonoBehaviour
{
    private bool isActive = true;

    public GameObject controllingCanvas;
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
    public void connectToArena()
    {
        //dosomething
    }

    public void backMainMenu()
    {
        //disconect
        //back main menu
    }
    #endregion

    #region private methods
    private void disconnectPUN()
    {

    }
    #endregion
}
