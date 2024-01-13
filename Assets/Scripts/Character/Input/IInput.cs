using System;

public interface IInput
{
    public void SubscribeOnInput(Action action);
    public void UnsubscribeOnInput(Action action);
}