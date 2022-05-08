using System;
using Godot;
using System.Collections.Generic;

public static class ServiceProvider
{
    // private static Dictionary<object, object> services;

    // public static void SetService<T>(object service)
    // {
    //     if (services == null)
    //     {
    //         services = new Dictionary<object, object>();
    //     }

    //     if (service != null)
    //     {
    //         services.Add(typeof(T), service);
    //     }
    // }

    // public static T GetService<T>()
    // {
    //     if (services != null && services.ContainsKey(typeof(T)))
    //     {
    //         return (T)services[typeof(T)];
    //     }
    //     return default(T);
    // }

    private static Dictionary<Type, object> services = new Dictionary<Type, object>();

    static ServiceProvider()
    {

    }

    public static void SetService<T>(T service)
    {
        services.Add(typeof(T), service);
    }

    public static T GetService<T>()
    {
        return (T)services[typeof(T)];
    }
}