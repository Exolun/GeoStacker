using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TransformType
{
    Position,
    Rotation,
    Scale
}

public class VectorInputController : MonoBehaviour
{
    public Transform selectedObjTransform;
    public TransformType type;
    public GameObject xText;
    public GameObject yText;
    public GameObject zText;

    private TMPro.TMP_InputField xValueText;
    private TMPro.TMP_InputField yValueText;
    private TMPro.TMP_InputField zValueText;

    public void Start()
    {
        this.xValueText = xText.GetComponent<TMPro.TMP_InputField>();
        this.yValueText = yText.GetComponent<TMPro.TMP_InputField>();
        this.zValueText = zText.GetComponent<TMPro.TMP_InputField>();
    }

    public void SetSelectedObject(GameObject obj)
    {
        this.selectedObjTransform = obj.transform;

        this.updateTransformStrings();   
    }

    public void OnValueEdited()
    {
        this.setTransformValues();
    }

    public void OnValueChanged()
    {
        this.updateTransformStrings();
    }

    public void setTransformValues()
    {
        if (selectedObjTransform == null)
        {
            return;
        }

        float xValue = float.Parse(this.xValueText.text.Trim());
        float yValue = float.Parse(this.yValueText.text.Trim());
        float zValue = float.Parse(this.zValueText.text.Trim());

        if (type == TransformType.Position)
        {
            this.selectedObjTransform.position = new Vector3(xValue, yValue, zValue);
        }
        else if (type == TransformType.Rotation)
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            //this.selectedObjTransform.localRotation.SetEulerAngles(xValue, yValue, zValue);
            selectedObjTransform.SetPositionAndRotation(selectedObjTransform.position, Quaternion.EulerRotation(xValue, yValue, zValue));
            #pragma warning restore CS0618 // Type or member is obsolete
        }
        else if (type == TransformType.Scale)
        {
            this.selectedObjTransform.localScale = new Vector3(xValue, yValue, zValue);
        }
    }

    private void updateTransformStrings()
    {
        if (type == TransformType.Position)
        {
            this.setVectors(this.selectedObjTransform.position.x.ToString(),
                this.selectedObjTransform.position.y.ToString(),
                this.selectedObjTransform.position.z.ToString()
            );
        }
        else if (type == TransformType.Rotation)
        {
            this.setVectors(this.selectedObjTransform.rotation.eulerAngles.x.ToString(),
                this.selectedObjTransform.rotation.eulerAngles.y.ToString(),
                this.selectedObjTransform.rotation.eulerAngles.z.ToString()
            );
        }
        else if (type == TransformType.Scale)
        {
            this.setVectors(this.selectedObjTransform.localScale.x.ToString(),
                this.selectedObjTransform.localScale.y.ToString(),
                this.selectedObjTransform.localScale.z.ToString()
            );
        }
    }

    private void setVectors(string x, string y, string z)
    {
        this.xValueText.SetTextWithoutNotify(x);
        this.yValueText.SetTextWithoutNotify(y);
        this.zValueText.SetTextWithoutNotify(z);
    }
}
