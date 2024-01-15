using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<string, I_Service> _services = new Dictionary<string, I_Service>();

    public static void RegisterService<T>(T service) where T : I_Service
    {
        var key = typeof(T).Name;
        if(_services.ContainsKey(key))
        {
            Debug.LogError("Attempting to register service that already exists: " + key);
            throw new InvalidOperationException();
        }

        _services.Add(key, service);
    }

    public static T GetService<T>() where T : I_Service
    {
        var key = typeof(T).Name;
        if(!_services.ContainsKey(key))
        {
            Debug.LogError("Attempting to get service that does not exists: " + key);
            throw new InvalidOperationException();
        }

        return (T)_services[key];
    }

    public static void UnregisterService<T>() where T : I_Service
    {
        var key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError("Attempting to unregister service that does not exists: " + key);
            throw new InvalidOperationException();
        }

        _services.Remove(key);
    }
}