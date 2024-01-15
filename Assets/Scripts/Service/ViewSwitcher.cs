using UnityEngine;
using UnityEngine.Events;

public class ViewSwitcher : MonoBehaviour, I_Service
{
    [SerializeField]
    private bool _isOTS;

    public bool isOTS
    {   
        get
        {
            return _isOTS;
        }
        set
        {
            _isOTS = value;
            SwitchViewMode(_isOTS);
        }
    }

    public UnityEvent onOTS;
    public UnityEvent onFPV;

    private void OnValidate()
    {
        SwitchViewMode(_isOTS);
    }

    private void OnEnable()
    {
        ServiceLocator.RegisterService(this);
        SwitchViewMode(_isOTS);
    }

    private void OnDisable()
    {
        ServiceLocator.UnregisterService<ViewSwitcher>();
    }

    private void SwitchViewMode(bool tpc)
    {
        var action = tpc? onOTS : onFPV;
        action?.Invoke();
    }
}