using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class ProButtonAttribute : PropertyAttribute
{
    public string ButtonName;

    public ProButtonAttribute(string name = null)
    {
        ButtonName = name;
    }
}
