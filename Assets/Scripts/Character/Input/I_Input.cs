using System;

public interface I_Input
{
    public void SubscribeOnInput(Action action);
    public void UnsubscribeOnInput(Action action);
}