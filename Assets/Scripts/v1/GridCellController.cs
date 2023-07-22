using UnityEngine;

public class GridCellController : MonoBehaviour
{
    public UnitAndGridManager unitAndGridManager;
    void Start()
    {
        unitAndGridManager.GridCells.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        unitAndGridManager.GridCells.Remove(this.gameObject);
    }
}