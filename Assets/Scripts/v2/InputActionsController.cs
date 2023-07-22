using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionsController : MonoBehaviour
{
    [HideInInspector] public GridManager gridManager;
    public void OnLMBUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled && gridManager.isSelectingObjects)
        {
            if (gridManager.canMakeSelections)
            {
                if (gridManager.StartObject == null)
                {
                    gridManager.StartObject = gridManager.LastFoundObject;
                }

                if (gridManager.EndObject == null)
                {
                    gridManager.EndObject = gridManager.StartObject;
                }

                gridManager.CheckObjects(true);
                gridManager.LastFoundObject.GetComponent<MeshRenderer>().material.color = Color.green;
                gridManager.isSelectingObjects = false;
                gridManager.StartObject = null;
                gridManager.EndObject = null;
            }
            else
            {
                gridManager.DeselectAll();
                gridManager.LastFoundObject = null;
            }
        }
        else
        {
            if (gridManager.canMakeSelections)
            {
                if (gridManager.StartObject == null)
                {
                    gridManager.StartObject = gridManager.LastFoundObject;
                }
                else
                {
                    gridManager.isSelectingObjects = true;
                }
            }
            else
            {
                gridManager.isSelectingObjects = false;
                gridManager.DeselectAll();
            }
        }
    }

    public void OnLeftShift(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            gridManager.isAdditionalSelection = true;
        }
        else
        {
            gridManager.isAdditionalSelection = false;
        }
    }
}