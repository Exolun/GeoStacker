using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BooleanModeSelector : MonoBehaviour
{
    private static BooleanModeSelector instance;
    public static BooleanModeSelector Instance { get { return BooleanModeSelector.instance; } }

    public bool IsUnionMode { get { return this.UnionButton.IsInteractable(); } }

    public Button UnionButton;
    public Button IntersectButton;

    public void Start()
    {
        BooleanModeSelector.instance = this;
    }

    public void ToggleButtons()
    {
        this.UnionButton.interactable = !this.UnionButton.interactable;
        this.IntersectButton.interactable = !this.UnionButton.interactable;
    }
}
