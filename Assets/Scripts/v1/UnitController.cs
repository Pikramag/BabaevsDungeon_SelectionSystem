using UnityEngine;

public class UnitController : MonoBehaviour
{
    public UnitAndGridManager unitAndGridManager;
    void Start()
    {
        unitAndGridManager.Units.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        unitAndGridManager.Units.Remove(this.gameObject);
    }
}