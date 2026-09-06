#if VSEH_MONO
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VSItemTooltips;

internal enum MonoJsonValueKind
{
    Undefined,
    Object,
    Array,
    String,
    Number,
    True,
    False,
    Null,
}

internal readonly struct MonoJsonProperty
{
    internal MonoJsonProperty(JProperty property)
    {
        Name = property.Name;
        Value = new MonoJsonElement(property.Value);
    }

    internal string Name { get; }
    internal MonoJsonElement Value { get; }
}

internal readonly struct MonoJsonElement
{
    private readonly JToken _token;

    internal MonoJsonElement(JToken token)
    {
        _token = token;
    }

    internal MonoJsonValueKind ValueKind => _token?.Type switch
    {
        JTokenType.Object => MonoJsonValueKind.Object,
        JTokenType.Array => MonoJsonValueKind.Array,
        JTokenType.String => MonoJsonValueKind.String,
        JTokenType.Integer or JTokenType.Float => MonoJsonValueKind.Number,
        JTokenType.Boolean when _token.Value<bool>() => MonoJsonValueKind.True,
        JTokenType.Boolean => MonoJsonValueKind.False,
        JTokenType.Null => MonoJsonValueKind.Null,
        _ => MonoJsonValueKind.Undefined,
    };

    internal IEnumerable<MonoJsonProperty> EnumerateObject()
    {
        if (_token is not JObject obj) yield break;
        foreach (JProperty property in obj.Properties())
            yield return new MonoJsonProperty(property);
    }

    internal IEnumerable<MonoJsonElement> EnumerateArray()
    {
        if (_token is not JArray array) yield break;
        foreach (JToken element in array)
            yield return new MonoJsonElement(element);
    }

    internal bool TryGetProperty(string name, out MonoJsonElement value)
    {
        if (_token is JObject obj)
        {
            JProperty property = obj.Property(name, StringComparison.Ordinal);
            if (property != null)
            {
                value = new MonoJsonElement(property.Value);
                return true;
            }
        }

        value = default;
        return false;
    }

    internal string GetString() => _token?.Value<string>();

    internal bool TryGetInt32(out int value)
    {
        try
        {
            value = _token.Value<int>();
            return true;
        }
        catch
        {
            value = 0;
            return false;
        }
    }

    internal bool TryGetDouble(out double value)
    {
        try
        {
            value = _token.Value<double>();
            return true;
        }
        catch
        {
            value = 0;
            return false;
        }
    }
}

internal sealed class MonoJsonDocument : IDisposable
{
    private MonoJsonDocument(JToken root)
    {
        RootElement = new MonoJsonElement(root);
    }

    internal MonoJsonElement RootElement { get; }

    internal static MonoJsonDocument Parse(string json) => new MonoJsonDocument(JToken.Parse(json));

    public void Dispose()
    {
    }
}
#endif
