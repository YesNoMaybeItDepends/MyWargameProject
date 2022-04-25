using Godot;
using System;

public abstract class State : Node2D
{
    protected StateManager owner;
    public State (StateManager Owner)
    {
        Name = "State";

        owner = Owner;
    }

    // TODO remove this
    [Obsolete]
    public virtual void handleInput(Godot.Collections.Array ColliderDicts)
    {

    }

    public virtual void handleEvent(object o)
    {

    }

    public virtual void handleMouseEnter(object o)
    {

    }

    public virtual void handleMouseExit(object o)
    {

    }

    public virtual void stateEnter()
    {

    }

    public virtual void stateExit()
    {

    }
}




