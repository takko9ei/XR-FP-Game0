using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgTransportScript : MonoBehaviour
{
    public int difficulty;
    private static MsgTransportScript _instance;
    public MsgTransportScript MsgTransportInstance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
