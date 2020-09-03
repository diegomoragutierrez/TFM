using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class PrefabAttribute : Attribute
{

    // Properties.
    public string Name { get; }

    public bool Persistent { get; }

    // Constructors.
    public PrefabAttribute(string name)
    {
        this.Name = name;
        this.Persistent = false;
    }

    public PrefabAttribute(string name, bool persistent)
    {
        this.Name = name;
        this.Persistent = persistent;
    }

}