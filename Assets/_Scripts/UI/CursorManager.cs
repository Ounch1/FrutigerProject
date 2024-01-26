using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public enum CursorState
    {
        Default,
        Hover
    }

    private static CursorManager instance;

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D hoverCursor;

    [SerializeField] private float YOffset = 0f;
    [SerializeField] private float XOffset = 0f;

    private CursorState currentCursorState = CursorState.Default;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateCursor();
    }

    public static void SetCursorState(CursorState newState)
    {
        if (instance != null)
        {
            instance.currentCursorState = newState;
            instance.UpdateCursor();
        }
    }

    private void UpdateCursor()
    {
        switch (currentCursorState)
        {
            case CursorState.Default:
                Cursor.SetCursor(defaultCursor, new Vector2(XOffset, YOffset), CursorMode.Auto);
                break;
            case CursorState.Hover:
                Cursor.SetCursor(hoverCursor, new Vector2(XOffset, YOffset), CursorMode.Auto);
                break;
            default:
                break;
        }
    }
}
