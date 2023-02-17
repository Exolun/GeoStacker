using Assets.Scripts.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryButtonController : MonoBehaviour
{
    public GameObject geometryPrototype;
    public Material intersectGeoMat;

    public void OnClick()
    {
        var addGeoCommand = new AddGeometryCommand(geometryPrototype, BooleanModeSelector.Instance.IsUnionMode ? null : intersectGeoMat);
        CommandManager.Instance.ExecuteCommand(addGeoCommand);
    }
}
