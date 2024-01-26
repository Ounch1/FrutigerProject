using UnityEngine;

public class Clickable : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Clicked");
    }

    private void OnMouseEnter()
    {
        CursorManager.SetCursorState(CursorManager.CursorState.Hover);
    }

    private void OnMouseExit()
    {
        CursorManager.SetCursorState(CursorManager.CursorState.Default);
    }
}
