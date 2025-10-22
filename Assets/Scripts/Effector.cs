using Oculus.Interaction.GrabAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour
{

    public int funcChoose = 0;//0 def 1 atk 2 hp
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            switch(funcChoose)
            {
                case 2:
                    other.gameObject.GetComponentInParent<PlayerHandler>().atk *= 1.1f;
                    break;
                case 1:
                    other.gameObject.GetComponentInParent<PlayerHandler>().hp += 15;
                    break;
                default:
                    other.gameObject.GetComponentInParent<PlayerHandler>().def += 5;
                    break;
            }
            Destroy(this.gameObject);
            
        }
    }
}
