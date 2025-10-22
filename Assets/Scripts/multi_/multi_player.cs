using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class multi_player : MonoBehaviourPun
{
    public float atk = 1;
    public int def;
    public int hp;

    public GameObject interactPlayerTranslate;

    public bool inHitLock = false;


    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            //寻找传参器
            return;
        }
        else
        {
            interactPlayerTranslate = GameObject.Find("RigMsgSender");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //更改：若此玩家血量为0，全体玩家显示you lose
        //方法：修改gameManager loseflag
        //在gamemanager中检测loseflag，判断是否结束
        //gamemanager条件：
        //此预制体的位置通过与传参器同步位置控制
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            
            return;
        }
        else
        {
            transform.position = interactPlayerTranslate.transform.position;
            //if the player prfb is local prfb, then synchronize position with camera rig
        }
        if(hp <= 0)
        {
            GameObject.Find("GameCore").GetComponent<multi_gamecore>().displayTotalMenu(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<multi_enemy>() != null && inHitLock == false)
        {
            //Debug.Log("asdfg");
            int enemyAtk = other.GetComponentInParent<multi_enemy>().atk;
            if (enemyAtk >= def + 1)
            {
                hp -= enemyAtk - def;
            }
            else
            {
                hp -= 1;
            }
            inHitLock = true;
            StartCoroutine(setLock());
        }
    }

    IEnumerator setLock()
    {
        yield return new WaitForSeconds(1);
        inHitLock = false;
    }
}
