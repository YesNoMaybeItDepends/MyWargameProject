using Godot;
using System;

public class StateManager : Node2D, IService
{
    private State _state;
    public State state
    {
        get
        {
            return _state;
        }
        set
        {
            if (state != null)
            {
                // Same state
                if (value.GetType() == _state.GetType())
                {
                    // TODO update this so that if it's the same then it updates itself and doesn't actually instance a new state
                    _state = value;
                    _state.stateEnter();
                    AddChild(_state);
                    return;
                }

                // Cleanup old state
                _state.stateExit();
                _state.QueueFree();
            }

            // Assign new state
            _state = value;
            _state.stateEnter();
            AddChild(_state);
        }
    }

    public StateManager()
    {
        Name = "State Manager";
    }

    public void Initialize()
    {
        //Connect("OnInput", this, "handleInput");
    }

    public void handleEvent(object o)
    {
        state.handleEvent(o);
    }
    public void handleMouseEnter(object o)
    {
        state.handleMouseEnter(o);
    }

    public void handleMouseExit(object o)
    {
        state.handleMouseExit(o);
    }
}