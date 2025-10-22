using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class multi_effector : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int funcChoose = 0;//need to change in prefab

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("ccc");
            switch (funcChoose)
            {
                case 2:
                    other.gameObject.GetComponent<multi_player>().hp += 10;
                    break;
                case 1:
                    other.gameObject.GetComponent<multi_player>().hp += 15;
                    break;
                default:
                    other.gameObject.GetComponent<multi_player>().def += 5;
                    break;
            }
            Destroy(this.gameObject);

        }
    }
}
