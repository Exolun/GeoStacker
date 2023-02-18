using Assets.Scripts.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryButtonController : MonoBehaviour
{
    public GameObject geometryPrototype;

    public void OnClick()
    {
        var addGeoCommand = new AddGeometryCommand(geometryPrototype, BooleanModeSelector.Instance.IsUnionMode ? null : ToolboxController.Instance.IntersectGeoMat);
        CommandManager.Instance.ExecuteCommand(addGeoCommand);
    }
}
