using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    private Vector2 startPoint;
    private Vector2 endPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject SelectionBoxImage;
    [SerializeField] private GameObject PointerImage;
    private RectTransform selectionBoxTransform;
    bool isClicked, isDragging;
    public bool isUnitUnderPointer, isGridCellUnderPointer;
    [HideInInspector] public UnitAndGridManager unitAndGridManager;
    Rect selectionBox;

    void Start()
    {
        SelectionBoxImage.SetActive(false);
        selectionBoxTransform = SelectionBoxImage.GetComponent<RectTransform>();
        DrawVisuals();
    }

    private void DrawVisuals()
    {
        Vector2 boxCenter = (startPoint + endPoint) / 2;
        selectionBoxTransform.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(startPoint.x - endPoint.x), Mathf.Abs(startPoint.y - endPoint.y));
        selectionBoxTransform.sizeDelta = boxSize;
    }

    private void CalculateBoxDisplay()
    {
        if (startPoint.x < endPoint.x)
        {
            selectionBox.xMin = startPoint.x;
            selectionBox.xMax = endPoint.x;
        }
        else
        {
            selectionBox.xMin = endPoint.x;
            selectionBox.xMax = startPoint.x;
        }

        if (startPoint.y < endPoint.y)
        {
            selectionBox.yMin = startPoint.y;
            selectionBox.yMax = endPoint.y;
        }
        else
        {
            selectionBox.yMin = endPoint.y;
            selectionBox.yMax = startPoint.y;
        }
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isClicked = true;
            isDragging = true;
            SelectionBoxImage.SetActive(true);
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            isDragging = false;
            CheckSelection();
            SelectionBoxImage.SetActive(false);
            startPoint = Vector2.zero;
            endPoint = Vector2.zero;
            selectionBoxTransform.position = Vector2.zero;
            selectionBoxTransform.sizeDelta = Vector2.zero;
        }
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (isClicked == true)
        {
            startPoint = context.ReadValue<Vector2>();
            isClicked = false;
        }
        else
        {
            if (isDragging)
            {
                unitAndGridManager.DeselectObjects();
                endPoint = context.ReadValue<Vector2>();
                DrawVisuals();
                CalculateBoxDisplay();
            }
        }

        PointerImage.transform.position = context.ReadValue<Vector2>();
    }

    private void CheckSelection()
    {
        if (startPoint != endPoint && startPoint != Vector2.zero && endPoint != Vector2.zero)
        {
            SelectObjects();
        }
        else
        {
            if (!isUnitUnderPointer && !isGridCellUnderPointer)
            {
                unitAndGridManager.DeselectObjects();
            }
        }
    }

    private void SelectObjects() {
        if (unitAndGridManager.isUnitSelection)
        {
            for (int i = 0; i < unitAndGridManager.Units.Count; i++)
            {
                if (selectionBox.Contains(mainCamera.WorldToScreenPoint(unitAndGridManager.Units[i].transform.position)))
                {
                    unitAndGridManager.Units[i].GetComponent<MeshRenderer>().material.color = Color.green;
                    unitAndGridManager.UnitsSelected.Add(unitAndGridManager.Units[i]);
                }
                else
                {
                    unitAndGridManager.Units[i].GetComponent<MeshRenderer>().material.color = Color.white;
                    unitAndGridManager.UnitsSelected.Remove(unitAndGridManager.Units[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < unitAndGridManager.GridCells.Count; i++)
            {
                if (selectionBox.Contains(mainCamera.WorldToScreenPoint(unitAndGridManager.GridCells[i].transform.position)))
                {
                    unitAndGridManager.GridCells[i].GetComponent<MeshRenderer>().material.color = Color.green;
                    unitAndGridManager.GridCellsSelected.Add(unitAndGridManager.GridCells[i]);
                }
                else
                {
                    unitAndGridManager.GridCells[i].GetComponent<MeshRenderer>().material.color = Color.white;
                    unitAndGridManager.GridCellsSelected.Remove(unitAndGridManager.GridCells[i]);
                }
            }
        }
    }

    public void SetPointerColor(Color color)
    {
        PointerImage.GetComponent<Image>().color = color;
    }
}