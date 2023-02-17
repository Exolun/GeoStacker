using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxController : MonoBehaviour
{
    // Singleton ref
    private static ToolboxController _instance;
    public static ToolboxController Instance { get { return ToolboxController._instance; } }



    // Start is called before the first frame update
    void Start()
    {
        ToolboxController._instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
