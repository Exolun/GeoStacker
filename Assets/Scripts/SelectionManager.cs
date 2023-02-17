//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using Assets.Scripts.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    public Material defaultMaterial;
    public Material highlightMaterial;
    public Material selectionMaterial;
    public GameObject objectSelectionHeader;

    public VectorInputController positionController;
    public VectorInputController rotationController;
    public VectorInputController scaleController;

    private Material originalMaterialHighlight;
    private Material originalMaterialSelection;
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    private TextMeshProUGUI selectedObjectText;

    private void Start()
    {
        Instance = this;
        this.selectedObjectText = objectSelectionHeader.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.GetComponent<MeshRenderer>().sharedMaterial = originalMaterialHighlight;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isPointerOverObject = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        if (!isPointerOverObject && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.GetComponent<MeshRenderer>().material != highlightMaterial)
                {
                    originalMaterialHighlight = highlight.GetComponent<MeshRenderer>().material;
                    highlight.GetComponent<MeshRenderer>().material = highlightMaterial;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0) && !isPointerOverObject)
        {
            if (highlight)
            {
                if (selection != null)
                {
                    selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                }
                selection = raycastHit.transform;
                if (selection.GetComponent<MeshRenderer>().material != selectionMaterial)
                {
                    SetSelection();
                }
                highlight = null;
            }
            else
            {
                if (selection)
                {
                    selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                    selection = null;
                }
            }
        }
    }

    public void SetSelection(GameObject newSelection = null)
    {
        if (newSelection != null)
        {
            if (selection)
            {
                selection.GetComponent<MeshRenderer>().material = this.defaultMaterial;
                selection = null;
            }

            this.selection = newSelection.transform;
        }

        originalMaterialSelection = originalMaterialHighlight;
        selection.GetComponent<MeshRenderer>().material = selectionMaterial;
        this.selectedObjectText.SetText(selection.gameObject.name);

        this.positionController.SetSelectedObject(selection.gameObject);
        this.rotationController.SetSelectedObject(selection.gameObject);
        this.scaleController.SetSelectedObject(selection.gameObject);

        HierarchyController.Instance.SetSelected(this.selection.gameObject);
    }

    public void DeleteSelected()
    {
        if (this.selection && this.selection.gameObject)
        {
            var deleteGeoCommand = new DeleteGeometryCommand(this.selection.gameObject);
            CommandManager.Instance.ExecuteCommand(deleteGeoCommand);
        }
    }

    public void ClearSelection()
    {
        this.selection = null;
        this.selectedObjectText.SetText("No Selection");
    }
}