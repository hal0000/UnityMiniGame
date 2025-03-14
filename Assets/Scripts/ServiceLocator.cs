using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    // Dictionary to hold registered services
    private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

    // Register a service instance for a given type
    public static void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    // Retrieve a service instance by type
    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        throw new Exception($"Service of type {typeof(T)} not registered.");
    }

    public static void Unregister<T>()
    {
        _services.Remove(typeof(T));
    }
}