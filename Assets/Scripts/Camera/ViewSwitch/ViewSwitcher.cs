using UnityEngine;
using UnityEngine.Events;

public class ViewSwitcher : MonoBehaviour
{
    private static ViewSwitcher _instance;

    [SerializeField] private bool _isTPC;

    public bool isTPC
    {   
        get
        {
            return _isTPC;
        }
        set
        {
            _isTPC = value;
            SwitchViewMode(_isTPC);
        }
    }

    public UnityEvent onTPC;
    public UnityEvent onFPS;

    public static ViewSwitcher instance => _instance;

    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(this.gameObject);

        SwitchViewMode(_isTPC);
    }

    private void SwitchViewMode(bool tpc)
    {
        var action = tpc? onTPC : onFPS;
        action?.Invoke();
    }
}