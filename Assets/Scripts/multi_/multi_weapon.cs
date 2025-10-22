using Oculus.Interaction;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class multi_weapon : MonoBehaviourPun
{
    public int attack;
    private int type;// 0atk 1mix 2sup

    private bool oneThreeReady = true;
    private bool twoFourReady = true;

    GameObject[] players;

    GrabInteractable _interactable;

    //[CustomEditor(typeof(Weapon),true)]
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(refreshPlayerEvery3sec());
        _interactable = gameObject.GetComponentInChildren<GrabInteractable>();
        judgeThisType();
    }

    // Update is called once per frame
    void Update()
    {
        handleSpecialWeapon();
    }

    IEnumerator refreshPlayerEvery3sec()
    {
        while (true)
        {
            refreshPlayer();
            yield return new WaitForSeconds(3);
        }
    }

    void refreshPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void judgeThisType()
    {

        if (this.tag == "attack_weapon")
        {
            type = 0;
        }
        else if (this.tag == "mix_weapon")
        {
            type = 1;
        }
        else if (this.tag == "support_weapon")
        {
            type = 2;
        }
    }

    void handleSpecialWeapon()
    {
        var hand = _interactable.Interactors.FirstOrDefault<GrabInteractor>();

        if (hand != null)
        {
            if (type == 1)
            {

                if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
                {
                    //asdf
                    oneThreeReady = false;
                    //code need
                    //add atk
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].GetComponent<multi_player>().atk *= 1.1f;
                    }
                    StartCoroutine(wait15sBuff());
                    resetAfter2s(oneThreeReady);
                }

            }
            else if (type == 2)
            {
                if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
                {
                    //asdf
                    oneThreeReady = false;
                    //code need
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].GetComponent<multi_player>().atk *= 1.1f;
                    }
                    StartCoroutine(wait15sBuff());
                    resetAfter2s(oneThreeReady);
                }
                else if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four))
                {
                    twoFourReady = false;

                    //code need
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].GetComponent<multi_player>().hp += attack;
                    }

                    resetAfter2s(twoFourReady);
                }
            }
        }
    }

    void resetAfter2s(bool needToReset)
    {
        StartCoroutine(crtwait2s(needToReset));
    }

    IEnumerator crtwait2s(bool needToReset)
    {
        yield return new WaitForSeconds(2);
        needToReset = true;
    }
    IEnumerator wait15sBuff()
    {
        yield return new WaitForSeconds(15);
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<multi_player>().atk /= 1.1f;
        }
    }
}
