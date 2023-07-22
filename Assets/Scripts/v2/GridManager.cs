using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Vector3 _CellSize;
    [SerializeField] private Vector2 _GridSize;
    [SerializeField] private float _GridHeight;
    [SerializeField] private GameObject _CellPrefab;
    [SerializeField] private InputActionsController _InputActionsController;
    [HideInInspector] public GameObject[,] Cells;
    public List<GameObject> SelectedCells = new List<GameObject>();
    public GameObject StartObject;
    public GameObject EndObject;
    public GameObject LastFoundObject;
    public bool isSelectingObjects;
    public bool isAdditionalSelection;
    public bool canMakeSelections;

    void Start()
    {
        _CellSize = _CellPrefab.transform.localScale;
        _CellPrefab.GetComponent<CellController>().gridManager = this;
        _CellPrefab.GetComponent<PointerInteractions>().gridManager = this;
        _InputActionsController.gridManager = this;
        isSelectingObjects = false;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Cells = new GameObject[(int)Mathf.Abs(Mathf.Floor(_GridSize.y)), (int)Mathf.Abs(Mathf.Floor(_GridSize.x))];
        for (int y = 0; y < Mathf.Abs(Mathf.Floor(_GridSize.y)); y++)
        {
            for (int x = 0; x < Mathf.Abs(Mathf.Floor(_GridSize.x)); x++)
            {
                GameObject spawnedColumn = Instantiate(
                    _CellPrefab,
                    new Vector3(
                        transform.position.x - x * _CellSize.x,
                        transform.position.y + _GridHeight,
                        transform.position.z + y * _CellSize.z
                    ),
                    Quaternion.identity
                );
                spawnedColumn.transform.SetParent(transform, true);
                spawnedColumn.GetComponent<CellController>().CellID = new CellID(x, y);
            }
        }
    }

    public void CheckObjects(bool isSelecting)
    {
        if (!isAdditionalSelection)
        {
            foreach (GameObject column in SelectedCells)
            {
                column.GetComponent<MeshRenderer>().material.color = Color.white;
            }

            if (isSelecting)
                SelectedCells = new List<GameObject>();
        }

        CellID startPos = ObjectCheckingPosition(true);
        CellID endPos = ObjectCheckingPosition(false);
        Debug.Log("Ending pos = ");
        endPos.PrintPos();

        for (int y = 0; y != Mathf.Abs(Mathf.Floor(_GridSize.y)); y++)
        {
            for (int x = 0; x != Mathf.Abs(Mathf.Floor(_GridSize.y)); x++)
            {
                if(x <= endPos.horizontalPos && x >= startPos.horizontalPos && y <= endPos.verticalPos && y >= startPos.verticalPos)
                {
                    if (isSelecting)
                        CheckObjectInSelected(x, y, isSelecting);
                    else
                        Cells[x, y].GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    if (SelectedCells.Contains(Cells[x, y]))
                        Cells[x, y].GetComponent<MeshRenderer>().material.color = Color.blue;
                    else
                        Cells[x, y].GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
        }

        if (isSelecting)
        {
            foreach (GameObject column in Cells)
            {
                if (SelectedCells.Contains(column))
                {
                    column.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    column.GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
        }
    }

    public void DeselectAll()
    {
        for (int y = 0; y < Mathf.Abs(Mathf.Floor(_GridSize.y)); y++)
        {
            for (int x = 0; x < Mathf.Abs(Mathf.Floor(_GridSize.x)); x++)
            {
                Cells[x, y].GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        SelectedCells = new List<GameObject>();
        StartObject = null;
        EndObject = null;
        LastFoundObject = null;
    }

    private void CheckObjectInSelected(int horizontalPos, int verticalPos, bool isSelecting)
    {
        if (!SelectedCells.Contains(Cells[horizontalPos, verticalPos]))
        {
            if(isSelecting)
                SelectedCells.Add(Cells[horizontalPos, verticalPos]);
        }
        else
        {
            if(isSelecting)
                SelectedCells.Remove(Cells[horizontalPos, verticalPos]);
        }
    }

    private CellID ObjectCheckingPosition(bool isStartObject)
    {
        CellID startObjectPos = StartObject.GetComponent<CellController>().CellID;
        CellID endObjectPos = EndObject.GetComponent<CellController>().CellID;
        CellID columnCoords = new CellID(0, 0);

        if (startObjectPos.horizontalPos > endObjectPos.horizontalPos)
        {
            if (isStartObject)
            {
                columnCoords.horizontalPos = endObjectPos.horizontalPos;
            }
            else
            {
                columnCoords.horizontalPos = startObjectPos.horizontalPos;
            }
        }
        else
        {
            if (isStartObject)
            {
                columnCoords.horizontalPos = startObjectPos.horizontalPos;
            }
            else
            {
                columnCoords.horizontalPos = endObjectPos.horizontalPos;
            }
        }

        if (startObjectPos.verticalPos > endObjectPos.verticalPos)
        {
            if (isStartObject)
            {
                columnCoords.verticalPos = endObjectPos.verticalPos;
            }
            else
            {
                columnCoords.verticalPos = startObjectPos.verticalPos;
            }
        }
        else
        {
            if (isStartObject)
            {
                columnCoords.verticalPos = startObjectPos.verticalPos;
            }
            else
            {
                columnCoords.verticalPos = endObjectPos.verticalPos;
            }
        }

        return columnCoords;
    }
}

public class CellID
{
    public int horizontalPos;
    public int verticalPos;

    public CellID(int horizontalPos, int verticalPos)
    {
        this.horizontalPos = horizontalPos;
        this.verticalPos = verticalPos;
    }

    public void PrintPos()
    {
        Debug.Log($"Horizontal = {horizontalPos}, Vertical = {verticalPos}");
    }
}