using UnityEngine;

public class CellController : MonoBehaviour
{
    public GridManager gridManager;
    public CellID CellID;

    void Start()
    {
        gridManager.Cells[CellID.horizontalPos, CellID.verticalPos] = gameObject;
    }

    private void OnDestroy()
    {
        gridManager.Cells[CellID.horizontalPos, CellID.verticalPos] = null;
    }
}