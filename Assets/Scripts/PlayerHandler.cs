using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    public float atk = 1;
    public int def;
    public int hp;

    public bool inHitLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            Debug.Log("find");
            GameObject.Find("GameManager").GetComponent<GameManagement>().displayTotalMenu(false);
            Debug.Log("finded");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Enemy>() != null && inHitLock == false)
        {
            Debug.Log("asdfg");
            int enemyAtk = other.GetComponentInParent<Enemy>().atk;
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
