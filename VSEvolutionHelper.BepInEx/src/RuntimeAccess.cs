using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

internal static class RuntimeAccess
{
    internal static T Get<T>(object instance, string name)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));

        Type type = instance.GetType();
        FieldInfo field = AccessTools.Field(type, name);
        if (field != null) return (T)field.GetValue(instance);

        PropertyInfo property = AccessTools.Property(type, name);
        if (property != null) return (T)property.GetValue(instance);

        throw new MissingMemberException(type.FullName, name);
    }
}

internal static class UnityObjectQuery
{
    internal static T[] FindAll<T>(bool includeInactive = false) where T : Object
    {
        var found = Object.FindObjectsByType<T>(
            includeInactive ? UnityEngine.FindObjectsInactive.Include : UnityEngine.FindObjectsInactive.Exclude,
            UnityEngine.FindObjectsSortMode.None);
#if VSEH_MONO
        return found;
#else
        var result = new T[found.Length];
        for (int i = 0; i < found.Length; i++) result[i] = found[i];
        return result;
#endif
    }

    internal static T FindAny<T>(bool includeInactive = false) where T : Object
    {
        return Object.FindAnyObjectByType<T>(
            includeInactive ? UnityEngine.FindObjectsInactive.Include : UnityEngine.FindObjectsInactive.Exclude);
    }
}

internal static class RuntimeEvents
{
    internal static UnityAction<BaseEventData> Wrap(Action<BaseEventData> action)
    {
#if VSEH_MONO
        return data => action(data);
#else
        return (UnityAction<BaseEventData>)action;
#endif
    }
}
