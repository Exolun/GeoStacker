using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxController : MonoBehaviour
{
    // Singleton ref
    private static ToolboxController _instance;
    public static ToolboxController Instance { get { return ToolboxController._instance; } }

    public GameObject CubePrototype;
    public GameObject CylinderPrototype;
    public GameObject SpherePrototype;
    public Material IntersectGeoMat;

    void Start()
    {
        ToolboxController._instance = this;   
    }

    public GameObject GetPrototypeByName(string name)
    {
        switch (name.ToLower()) {
            case "cube":
                return this.CubePrototype;
            case "cylinder":
                return this.CylinderPrototype;
            case "sphere":
                return this.SpherePrototype;
        }

        return null;
    }
}
