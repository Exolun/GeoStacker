using Assets.Scripts.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyController : MonoBehaviour
{
    public static HierarchyController Instance { get; private set; }

    public RectTransform scrollContentContainer;
    public GameObject sceneGeoPrototype;

    private readonly float elementPadding = 2f;
    private float scrollContentContainerOriginalHeight;
    private Stack<KeyValuePair<GameObject, GameObject>> sceneHierarchyStack = new Stack<KeyValuePair<GameObject, GameObject>>();
    private RectTransform sceneGeoProtoRect;

    public void Start()
    {
        Instance = this;
        scrollContentContainerOriginalHeight = this.scrollContentContainer.rect.height;
        this.sceneGeoProtoRect = this.sceneGeoPrototype.GetComponent<RectTransform>();
    }

    public void AddGeometry(GameObject newGeometry)
    {
        var hierarchyEntry = Instantiate(sceneGeoPrototype, scrollContentContainer);
        var geoRect = hierarchyEntry.GetComponent<RectTransform>();
        int containerSizeIncrease = (int)(geoRect.rect.height + this.elementPadding);
        scrollContentContainer.sizeDelta = new Vector2(0, scrollContentContainer.rect.height + containerSizeIncrease);
        int sceneGeoEntryCount = GameObject.FindGameObjectsWithTag("SceneGeo").Length + 1;
        geoRect.anchoredPosition = new Vector2(geoRect.anchoredPosition.x, geoRect.anchoredPosition.y - ((geoRect.rect.height - this.elementPadding) * sceneGeoEntryCount));

        hierarchyEntry.SetActive(true);

        // Set Strings
        var geoController = hierarchyEntry.GetComponent<HierarchyEntryController>();
        geoController.SetValues(newGeometry.name, sceneGeoEntryCount);

        sceneHierarchyStack.Push(new KeyValuePair<GameObject, GameObject>(newGeometry, hierarchyEntry));

        // Set color if intersect mode
        if (!BooleanModeSelector.Instance.IsUnionMode)
        {
            var imgComponent = hierarchyEntry.GetComponent<Image>();
            imgComponent.color = new Color(.88f, .38f, .41f, .39f);
        }
    }

    public void SetSelected(GameObject go)
    {
        foreach (var kvp in this.sceneHierarchyStack)
        {
            kvp.Value.GetComponent<HierarchyEntryController>().nameLabel.fontStyle = TMPro.FontStyles.Normal;
        }

        var pair = this.sceneHierarchyStack.FirstOrDefault(kvp => kvp.Key == go);
        pair.Value.GetComponent<HierarchyEntryController>().nameLabel.fontStyle = TMPro.FontStyles.Underline;
    }

    public void SelectionClicked(GameObject hierarchyEntry)
    {
        var pair = this.sceneHierarchyStack.FirstOrDefault(kvp => kvp.Value == hierarchyEntry);
        SelectionManager.Instance.SetSelection(pair.Key);
        this.SetSelected(pair.Key);
    }

    public void DeleteClicked(GameObject hierarchyEntry)
    {
        if (!this.sceneHierarchyStack.Any(kvp => kvp.Value == hierarchyEntry))
        {
            return;
        }

        var entryClicked = this.sceneHierarchyStack.FirstOrDefault(kvp => kvp.Value == hierarchyEntry);
        var deleteCommand = new DeleteGeometryCommand(entryClicked.Key);
        CommandManager.Instance.ExecuteCommand(deleteCommand);
    }

    public void DeleteGeometry(GameObject toDelete)
    {
        int containerSizeIncrease = (int)(this.sceneGeoProtoRect.rect.height + this.elementPadding);
        scrollContentContainer.sizeDelta = new Vector2(0, scrollContentContainer.rect.height - containerSizeIncrease);

        var hierarchyList = this.sceneHierarchyStack.Reverse().ToList();
        for (int i = hierarchyList.Count - 1; i >= 0; i--)
        {
            var current = hierarchyList[i];
            if (current.Key == toDelete)
            {
                Destroy(current.Value);
                hierarchyList.Remove(current);
                continue;
            }

            current.Value.GetComponent<RectTransform>().anchoredPosition = this.sceneGeoProtoRect.anchoredPosition;
        }

        int count = 1;
        foreach (var pair in hierarchyList)
        {
            var controller = pair.Value.GetComponent<HierarchyEntryController>();
            controller.SetValues(pair.Key.name, count);

            var geoRect = pair.Value.GetComponent<RectTransform>();
            geoRect.anchoredPosition = new Vector2(geoRect.anchoredPosition.x, geoRect.anchoredPosition.y - ((geoRect.rect.height - this.elementPadding) * count));
            count++;
        }

        this.sceneHierarchyStack = new Stack<KeyValuePair<GameObject, GameObject>>(hierarchyList);
    }
}
