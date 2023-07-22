using UnityEngine;
using UnityEngine.EventSystems;

public class PointerInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GridManager gridManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*Debug.Log($"Object = ");
        this.GetComponent<CellController>().columnID.PrintPos();*/
        if (gridManager.isSelectingObjects && gridManager.StartObject != null)
        {
            gridManager.EndObject = this.gameObject;
            gridManager.CheckObjects(false);
        }
        else
        {
            gridManager.StartObject = this.gameObject;
            this.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        gridManager.LastFoundObject = this.gameObject;
        gridManager.canMakeSelections = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*Debug.Log($"Deselected object at ");
        this.GetComponent<CellController>().columnID.PrintPos();*/
        if (!gridManager.isSelectingObjects)
            if(!gridManager.SelectedCells.Contains(this.gameObject))
                this.GetComponent<MeshRenderer>().material.color = Color.white;
            else
                this.GetComponent<MeshRenderer>().material.color = Color.blue;
        gridManager.canMakeSelections = false;
    }
}