using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR;
using UnityEngine;
using static OVRBoundary;

public class GameManagement : MonoBehaviour
{
    private int thisGameDiff;
    //int timeIntervalGenerating = 3;
    public Vector3[] boundaryPoints;
    private Vector3 initializePos = Vector3.zero;
    public int[] killEnemyCount;//0 enemy0, foo

    public GameObject[] enemyObj;
    public GameObject[] fxObj;

    //public GameObject winOrLoseCanvas;
    //public TextMeshPro winOrLoseText;

    public bool generateFlag = false;
    public bool loseFlag = false;

    public GameObject uiHandler;


    // Start is called before the first frame update
    void Start()
    {
        
        tryLoadBoundaryPoints();
        //Debug.Log("bbbbb");
        //Debug.Log("asdfafdsafasdf");
        thisGameDiff = 2;
        //StartCoroutine(continueGenerateEnemy(timeIntervalGenerating, thisGameDiff));
        StartCoroutine(continueGenerateBuff());
        

        //generateEnemy(0);

        //StartCoroutine(generateEnemy3sec());
        //Debug.Log("asdfafdsafasdf");
        //Instantiate(enemyObj[0], new Vector3(0,0,0), enemyObj[0].transform.rotation);
        StartCoroutine(timer40Sec());   

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

    void generateEnemy(int type)
    {
        switch (type)
        {
            case 0:
                //Instantiate(enemy0, pos);
                //enemy will refresh on the boundary of active space
                //TODO finish enemy prefab
                //Instantiate(enemyObj[0], getRandomGeneratePos(), enemyObj[0].transform.rotation);
                Instantiate(enemyObj[0], getRandomGeneratePos(), enemyObj[0].transform.rotation);
                break;
            case 1:
                //same
                Instantiate(enemyObj[1], getRandomGeneratePos(), enemyObj[1].transform.rotation);
                break;
            case 2:
                //same
                Instantiate(enemyObj[2], getRandomGeneratePos(), enemyObj[2].transform.rotation);
                break;
            default:
                return;
        }
    }

    IEnumerator continueGenerateEnemy(int time, int difficulty)
    {
        while (true)
        {
            //generate item
            if (difficulty == 0)
            {
                tutDiffEnemyGenerate();
            }
            else if (difficulty == 1)
            {
                lowDiffEnemyGenerate();
            }
            else if (difficulty == 2)
            {
                midDiffEnemyGenerate();
            }
            else
            {
                highDiffEnemyGenerate(3);
            }
            yield return new WaitForSeconds(time);
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

    IEnumerator continueGenerateBuff()
    {
        while (true)
        {
            //prepare prefab
            int a = Random.Range(0, 3);
            Vector3 v3 = new Vector3(Random.insideUnitCircle.x * 3, 0, Random.insideUnitCircle.y * 3);
            Quaternion rot = new Quaternion();
            rot.SetLookRotation(Random.insideUnitCircle, new Vector3(0, 1, 0));
            Instantiate(fxObj[a], v3, new Quaternion());
            yield return new WaitForSeconds(5);
        }
    }

    void tryLoadBoundaryPoints()
    {
        if (OVRManager.boundary.GetConfigured())
        {
            boundaryPoints = OVRManager.boundary.GetGeometry(BoundaryType.OuterBoundary);
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

    //TODO IMPLEMENT ENEMY AND EFFECTOR INSTANCIATE LOGICS

    IEnumerator generateEnemy3sec()
    {
        yield return new WaitForSeconds(3);
        generateEnemy(0);
        yield return new WaitForSeconds(3);
        generateEnemy(0);
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
            uiHandler.GetComponent<UniversalUIHandler>().showWinUI();
        }
        else
        {
            //lose text
            loseFlag = true;
            uiHandler.GetComponent<UniversalUIHandler>().showLoseUI();
        }
    }

    IEnumerator timer40Sec()
    {
        yield return new WaitForSeconds(25);
        //Debug.Log("asfffffff");
        if (!loseFlag)
        {
            displayTotalMenu(true);

        }
    }

    public int[] getTotalKills()
    {
        return killEnemyCount;
    }
}
