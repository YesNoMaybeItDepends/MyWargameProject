using Godot;
using System;

public class StateManager : Node2D
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
            if (_state != null)
            {
                _state.stateExit();
                _state.QueueFree();
            }
            _state = value;
            _state.stateEnter();
            AddChild(_state);
        }
    }

    public Board board;

    public StateManager()
    {
        Name = "State Manager";
        Helpers.SetStateManager(this);
        DefaultState defaultState = new DefaultState(this);
        state = defaultState;
    }

    public void Initialize(Board Board)
    {
        board = Board;
        //Connect("OnInput", this, "handleInput");
    }

    public void handleInput(Godot.Collections.Array ColliderDicts)
    {
        state.handleInput(ColliderDicts);
    }

}