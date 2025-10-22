using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static OVRBoundary;

public class multi_gamecore : MonoBehaviourPun
{
    private int thisGameDiff;
    //int timeIntervalGenerating = 3;
    public Vector3[] boundaryPoints;
    private Vector3 initializePos = Vector3.zero;
    public int[] killEnemyCount;//0 enemy0, foo

    //public GameObject[] enemyObj;
    //public GameObject[] fxObj;//todo

    //public GameObject winOrLoseCanvas;
    //public TextMeshPro winOrLoseText;

    public bool generateFlag = false;
    public bool loseFlag = false;

    public GameObject uiHandler;
    // Start is called before the first frame update
    void Start()
    {
        tryLoadBoundaryPoints();
        thisGameDiff = 3;
        StartCoroutine(continueGenerateBuff());
        StartCoroutine(generateFirst4Enemy());
        StartCoroutine(timer40Sec());
        if (photonView.IsMine)
        {
            PhotonNetwork.Instantiate("Player", new Vector3(0,0,0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (generateFlag)
        {
            //generate enemy
            //if ((killEnemyCount[0] + killEnemyCount[1] + killEnemyCount[2])%3 == 0)
            //{
            Debug.Log("generate__");
            generateEnemyByDifficult();
            //}
            generateFlag = false;
        }
    }

    int randomGenerator()//generate random int between 0 and 100
    {
        return Random.Range(0, 101);
    }

    void tutDiffEnemyGenerate()//every 5 sec call
    {
        for (int i = 0; i < 3; i++)
        {
            int type = randomGenerator();
            if (type < 80)
            {
                generateEnemy(0);
            }
            else
            {
                generateEnemy(1);
            }
        }
    }

    void lowDiffEnemyGenerate()//every 4 sec call
    {
        for (int i = 0; i < 3; i++)
        {
            int type = randomGenerator();
            if (type < 60)
            {
                generateEnemy(0);
            }
            else
            {
                generateEnemy(1);
            }
        }
    }

    void midDiffEnemyGenerate()//every 3 sec call
    {
        for (int i = 0; i < 3; i++)
        {
            int type = randomGenerator();
            if (type < 50)
            {
                generateEnemy(0);
            }
            else if (type < 90)
            {
                generateEnemy(1);
            }
            else
            {
                generateEnemy(2);
            }
        }
    }

    void highDiffEnemyGenerate(int a)   //every 3 sec call
                                        //PAY ATTENTION TO NUM, DIFFERENT FROM OTHERS
    {
        for (int i = 0; i < a; i++)
        {
            int type = randomGenerator();
            if (type < 40)
            {
                generateEnemy(0);
            }
            else if (type < 80)
            {
                generateEnemy(1);
            }
            else
            {
                generateEnemy(2);
            }
        }
    }
    void generateEnemyByDifficult()
    {
        if (thisGameDiff == 0)
        {
            tutDiffEnemyGenerate();
        }
        else if (thisGameDiff == 1)
        {
            lowDiffEnemyGenerate();
        }
        else if (thisGameDiff == 2)
        {
            midDiffEnemyGenerate();
        }
        else
        {
            highDiffEnemyGenerate(3);
        }
    }

    void generateEnemy(int type)//*******************neeeeed option
    {
        //Vector3 v3 = new Vector3(Random.insideUnitCircle.x * 3, 0, Random.insideUnitCircle.y * 3);
        switch (type)
        {
            case 0:
                //Instantiate(enemy0, pos);
                //enemy will refresh on the boundary of active space
                //TODO finish enemy prefab
                //Instantiate(enemyObj[0], getRandomGeneratePos(), enemyObj[0].transform.rotation);
                //Instantiate(enemyObj[0], getRandomGeneratePos(), enemyObj[0].transform.rotation);
                PhotonNetwork.Instantiate("Enemy0",getRandomGeneratePos(),Quaternion.identity);
                break;
            case 1:
                //same
                PhotonNetwork.Instantiate("Enemy1", getRandomGeneratePos(), Quaternion.identity);
                break;
            case 2:
                //same
                PhotonNetwork.Instantiate("Enemy2", getRandomGeneratePos(), Quaternion.identity);
                break;
            default:
                return;
        }
    }

    void tryLoadBoundaryPoints()
    {
        if (OVRManager.boundary.GetConfigured())
        {
            boundaryPoints = OVRManager.boundary.GetGeometry(BoundaryType.OuterBoundary);
        }
    }

    IEnumerator continueGenerateBuff()//need option
    {
        while (true)
        {
            //prepare prefab
            int a = Random.Range(0, 3);
            Vector3 v3 = new Vector3(Random.insideUnitCircle.x * 3, 0, Random.insideUnitCircle.y * 3);
            Quaternion rot = new Quaternion();
            rot.SetLookRotation(Random.insideUnitCircle, new Vector3(0, 1, 0));
            //Instantiate(fxObj[a], v3, new Quaternion());
            PhotonNetwork.Instantiate("Effector" + a.ToString(), v3, rot);
            yield return new WaitForSeconds(8);
        }
    }

    Vector3 getRandomGeneratePos()
    {
        if (boundaryPoints.Length > 0)
        {
            return boundaryPoints[Random.Range(0, boundaryPoints.Length)];
        }
        else return Vector3.zero;
    }

    IEnumerator timer40Sec()
    {
        yield return new WaitForSeconds(20);
        if (!loseFlag)
        {
            displayTotalMenu(true);

        }
    }

    public int[] getTotalKills()
    {
        return killEnemyCount;
    }

    public void displayTotalMenu(bool win)
    {
        //winOrLoseCanvas.SetActive(true);
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < go.Length; i++)
        {
            Destroy(go[i]);
        }
        if (win)
        {
            //win text
            uiHandler.GetComponent<multi_ui_ingame>().showWinUI();
        }
        else
        {
            //lose text
            loseFlag = true;
            uiHandler.GetComponent<multi_ui_ingame>().showLoseUI();
        }
    }

    private IEnumerator generateFirst4Enemy() {
        PhotonNetwork.Instantiate("Enemy0", getRandomGeneratePos(), Quaternion.identity);
        yield return new WaitForSeconds(2);
        PhotonNetwork.Instantiate("Enemy1", getRandomGeneratePos(), Quaternion.identity);
        yield return new WaitForSeconds(2);
        PhotonNetwork.Instantiate("Enemy2", getRandomGeneratePos(), Quaternion.identity);
        yield return new WaitForSeconds(2);
        PhotonNetwork.Instantiate("Enemy1", getRandomGeneratePos(), Quaternion.identity);
    }
}
