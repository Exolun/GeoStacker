using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoController : MonoBehaviour
{
    public static UndoController Instance { get; private set; }

    public Button UndoButton;

    void Start()
    {
        Instance = this;       
    }

    public void UpdateInteractive(bool canInteract)
    {
        this.UndoButton.interactable = canInteract;
    }

    public void UndoClicked()
    {
        CommandManager.Instance.Undo();
    }
}
