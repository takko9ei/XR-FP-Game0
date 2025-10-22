using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mirror_Enemy : MonoBehaviour
{
    public int atk;
    public float hp;
    public float speed;
    public int def;
    [SerializeField]
    Slider hpDisplay;
    GameObject[] players;
    private bool addedCount = false;

    private bool inShakeBack = false;
    private bool coolDown = true;
    private Animator animator;

    public float attackDis = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        ikite();
    }

    void moveTowardsPlayer()
    {
        animator.SetInteger("Status", 1);
    }

    GameObject getClosestPlayer()
    {
        GameObject ply;
        ply = players[0];
        //float curMinDist = (ply.transform.position - transform.position).sqrMagnitude;//MARK XZ
        float curMinDist = new Vector3(ply.transform.position.x - transform.position.x, 0, ply.transform.position.z - transform.position.z).sqrMagnitude;
        for (int i = 0; i < players.Length; i++)
        {
            float curDist = new Vector3(players[i].transform.position.x - transform.position.x, 0, players[i].transform.position.z - transform.position.z).sqrMagnitude;
            if (curDist < curMinDist)
            {
                curMinDist = curDist;
                ply = players[i];
            }
        }
        return ply;
    }

    void attack()
    {

        animator.SetInteger("Status", Random.Range(2, 4));

    }

    private void behavior()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject currentPlayer = getClosestPlayer();
        //Transform thisTransform = transform;
        Vector3 lapos = new Vector3(currentPlayer.transform.position.x, 0, currentPlayer.transform.position.z);
        transform.LookAt(lapos);//face to player
        float dist = new Vector3(currentPlayer.transform.position.x - transform.position.x, 0, currentPlayer.transform.position.z - transform.position.z).sqrMagnitude;
        if (dist < attackDis)
        {
            attack();
        }
        else
        {
            moveTowardsPlayer();
        }
    }

    private void ikite()
    {
        if (hp > 0)
        {
            behavior();
        }
        isDead();
    }

    private void onHit()
    {
        //1get attacker
        inShakeBack = true;
        animator.ResetTrigger("toHit");
        animator.SetTrigger("toHit");
        //gethit
        StartCoroutine(waitShakeBackEnd());//one monster can only be heat per 0.64 sec
    }

    private IEnumerator waitShakeBackEnd()
    {
        yield return new WaitForSeconds(0.64f);
        inShakeBack = false;
    }

    private void isDead()
    {
        if (hp <= 0)
        {

            animator.SetInteger("Status", 4);
            StartCoroutine(distroyThisEnemy());
        }
    }

    IEnumerator distroyThisEnemy()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)//when get hit
    {
        GameObject ot = other.gameObject;
        //bool isAttached = ot.GetComponent<Weapon>().isAttached;
        if ((other.transform.parent.gameObject.tag == "attack_weapon" || other.transform.parent.gameObject.tag == "mix_weapon") && !inShakeBack)
        {
            onHit();
            int weaponatk;
            //float playeratk;
            weaponatk = ot.GetComponentInParent<Weapon>().attack;
            //playeratk = ot.GetComponentInParent<PlayerHandler>().atk;
            this.hp -= 18;

        }
    }

    
}
