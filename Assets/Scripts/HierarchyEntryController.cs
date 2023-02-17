using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class HierarchyEntryController : MonoBehaviour
{
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI numberLabel;

    public void SetValues(string name, int number)
    {
        this.nameLabel.SetText(name);
        this.numberLabel.SetText(number.ToString());
    }

    public void Select()
    {
        HierarchyController.Instance.SelectionClicked(this.gameObject);
    }

    public void Delete()
    {
        HierarchyController.Instance.DeleteClicked(this.gameObject);
    }
}
