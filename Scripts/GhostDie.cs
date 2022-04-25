using Godot;
using System;

public class GhostDie : AnimatedSprite
{
    public const int sides = 6;
    Timer timer;

    public GhostDie()
    {
        Name = "Ghost Die";

        // Set die sprite 
        SpriteFrames frames = GD.Load("res://Assets/BlockDice_SpriteFrames.tres") as SpriteFrames;
        Frames = frames;
        Frame = 0;

        ZIndex = 1;

        Modulate = new Color(1,1,1,0.5f);


    }

    public override void _EnterTree()
    {
        timer = new Timer();
        AddChild(timer);
        timer.WaitTime = 0.30f;
        timer.Paused = false;
        timer.Connect("timeout", this, "onTimeout");
        //timer.Start();
        timer.Start();
    }

    void onTimeout()
    {
        if (Frame == 5)
        {
            Frame = 0;
        }

        Frame++;
    }

    public override void _Process(float delta)
    {
        float degreesPerSecond = 360.0f * 0.50f;

        Rotate(delta * Mathf.Deg2Rad(degreesPerSecond));
    }
}