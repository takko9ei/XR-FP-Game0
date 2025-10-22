using Meta.WitAi;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mirror_UI : MonoBehaviour
{

    public NetworkManager nwm;

    public GameObject controllingCanvas;
    // Start is called before the first frame update
    public void sudahosu()
    {
        nwm.StartHost();
        nwm.networkAddress = "127.0.0.1";
        controllingCanvas.gameObject.SetActive(false);
    }

    public void sudakuri()
    {
        nwm.StartClient();
        nwm.networkAddress = "172.20.10.4";
        //controllingCanvas.gameObject.SetActive(false);
        StartCoroutine(waitAndSet());
    }

    IEnumerator waitAndSet()
    {
        yield return new WaitForSeconds(0.5f);
        nwm.StartClient();
        nwm.networkAddress = "172.20.10.4";
        controllingCanvas.gameObject.SetActive(false);
    }

}
