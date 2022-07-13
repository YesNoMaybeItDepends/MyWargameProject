using Godot;
using System;
using System.Collections.Generic;

public class resourceTest : Resource
{
    [Export]
    public string Name;

    [Export]
    public int Sneed;

    [Export]
    public List<lol> lool;
}

public enum lol
{
    Sneeds,
    Chucks
}