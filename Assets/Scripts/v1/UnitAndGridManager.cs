using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitAndGridManager : MonoBehaviour
{
    public List<GameObject> Units = new List<GameObject>();
    public List<GameObject> UnitsSelected = new List<GameObject>();
    public List<GameObject> GridCells = new List<GameObject>();
    public List<GameObject> GridCellsSelected = new List<GameObject>();
    public bool isUnitSelection = false;
    
    [SerializeField] private SelectionController selectionController;
    [SerializeField] private GridMaker gridMaker;
    void Start()
    {
        isUnitSelection = true;
        selectionController.unitAndGridManager = this;
        gridMaker.unitAndGridManager = this;
        gridMaker.GenerateGrid();
    }

    public void ChangeSelectionType(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isUnitSelection = !isUnitSelection;
            selectionController.SetPointerColor(Color.white);
            DeselectObjects();
        }
    }

    public void OnDeleteObject(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            if (isUnitSelection && UnitsSelected.Count > 0)
            {
                int SelectedUnitsAmmount = UnitsSelected.Count;
                for (int i = 0; i < SelectedUnitsAmmount; i++)
                {
                    Destroy(UnitsSelected[i].gameObject);
                }
                UnitsSelected = new List<GameObject>();
            }

            if (!isUnitSelection && GridCellsSelected.Count > 0)
            {
                int SelectedGridCellsAmmount = GridCellsSelected.Count;
                for (int i = 0; i < SelectedGridCellsAmmount; i++)
                {
                    Destroy(GridCellsSelected[i].gameObject);
                }
                GridCellsSelected = new List<GameObject>();
            }
        }
    }

    public void DeselectObjects()
    {
        if (UnitsSelected.Count != 0)
        {
            foreach (GameObject unit in UnitsSelected)
            {
                unit.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        if (GridCellsSelected.Count != 0)
        {
            foreach (GameObject gridCell in GridCellsSelected)
            {
                gridCell.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        UnitsSelected = new List<GameObject>();
        GridCellsSelected = new List<GameObject>();
    }
}