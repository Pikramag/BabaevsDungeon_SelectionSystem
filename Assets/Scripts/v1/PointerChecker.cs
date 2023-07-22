using UnityEngine;
using UnityEngine.EventSystems;

public class PointerChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnitAndGridManager unitAndGridManager;
    public SelectionController selectionController;

    private void Start()
    {
        unitAndGridManager = FindObjectOfType<UnitAndGridManager>();
        selectionController = FindObjectOfType<SelectionController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Object = {this.name}");
        CheckObjectUnderPointer(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"{this.name} is not under pointer");
        CheckObjectUnderPointer(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.TryGetComponent<UnitController>(out UnitController unitController) && unitAndGridManager.isUnitSelection)
        {
            unitAndGridManager.DeselectObjects();
            unitAndGridManager.UnitsSelected.Add(this.gameObject);
            this.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        if (this.TryGetComponent<GridCellController>(out GridCellController gridCellController) && !unitAndGridManager.isUnitSelection)
        {
            unitAndGridManager.DeselectObjects();
            unitAndGridManager.GridCellsSelected.Add(this.gameObject);
            this.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    private void CheckObjectUnderPointer(bool isUnderPointer)
    {
        if (this.tag == "Unit" && unitAndGridManager.isUnitSelection)
        {
            selectionController.isUnitUnderPointer = isUnderPointer;
            if (isUnderPointer)
            {
                selectionController.SetPointerColor(Color.red);
            }
            else
            {
                selectionController.SetPointerColor(Color.white);
            }
        }
        else if (this.tag == "GridCell" && !unitAndGridManager.isUnitSelection)
        {
            selectionController.isGridCellUnderPointer = isUnderPointer;
            if (isUnderPointer)
            {
                selectionController.SetPointerColor(Color.blue);
            }
            else
            {
                selectionController.SetPointerColor(Color.white);
            }
        }
    }
}