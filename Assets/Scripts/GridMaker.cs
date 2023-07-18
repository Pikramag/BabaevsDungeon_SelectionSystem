using UnityEngine;

public class GridMaker : MonoBehaviour
{
    [SerializeField] private GameObject GridCellPrefab;
    [SerializeField] private Vector2 GridWidthAndHeight;
    [SerializeField] private float GridYHeight;
    public UnitAndGridManager unitAndGridManager;

    void Start()
    {
        GridCellPrefab.GetComponent<GridCellController>().unitAndGridManager = unitAndGridManager;
    }

    public void GenerateGrid()
    {
        for (int y = 0; y < GridWidthAndHeight.y; y++)
        {
            for (int x = 0; x < GridWidthAndHeight.x; x++)
            {
                GameObject spawnedGridCell = Instantiate(
                    GridCellPrefab,
                    new Vector3(
                        transform.position.x + x,
                        GridYHeight,
                        transform.position.z + y
                    ),
                    Quaternion.identity);
                spawnedGridCell.transform.SetParent(transform, true);
            }
        }
    }
}