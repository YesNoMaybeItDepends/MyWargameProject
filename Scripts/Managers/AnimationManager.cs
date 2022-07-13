using System;
using Godot;

public class AnimationManager : Node
{
    public Tween tween;

    public AnimationManager()
    {
        Name = "Animation Manager";
        
        tween = new Tween();
        AddChild(tween);
    }
}