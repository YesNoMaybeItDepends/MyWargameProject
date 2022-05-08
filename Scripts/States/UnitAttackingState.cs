using Godot;
using System;

public class UnitAttackingState : State
{
    public Unit target;
    public Unit attacker;
    public BlockDie die;

    public UnitAttackingState(StateManager Owner, Unit _attacker, Unit _target) : base(Owner)
    {
        Name = "UnitAttackingState";
        target = _target;
        attacker = _attacker;
    }

    public override void stateEnter()
    {
        die = new BlockDie();
        AddChild(die);

        float x = target.GlobalPosition[0] - die.RectSize.x/2;
        float y = target.GlobalPosition[1] - die.RectSize.y*1.5f;
        die.RectGlobalPosition = new Vector2(x,y);

        die.asprite.ZIndex = 1;

        die.rollDie();

        die.Connect("pickedBlockDie", this, "handleBlockDieResult");
    }

    public void handleBlockDieResult(BlockDie.Faces result)
    {
        if (target != null)
        {
            State nextState = new DefaultState(owner);
            switch (die.face)
            {
                case BlockDie.Faces.AttackerDown:
                    GD.Print("AttackerDown");
                    kill(attacker);
                    nextState = new DefaultState(owner);
                    break;
                case BlockDie.Faces.DefenderPushed_1:
                    nextState = new PushState(owner, result, attacker,target);
                    break;
                case BlockDie.Faces.DefenderDown:
                    GD.Print("DefenderDown");
                    nextState = new PushState(owner, result, attacker,target);
                    break;
                case BlockDie.Faces.DefenderStumbles:
                    GD.Print("DefenderStumbles");
                    nextState = new PushState(owner, result, attacker,target);
                    break;
                case BlockDie.Faces.DefenderPushed_2:
                    nextState = new PushState(owner, result, attacker,target);
                    break;
                case BlockDie.Faces.BothDown:
                    GD.Print("BothDown");
                    kill(target);
                    kill(attacker);
                    nextState = new DefaultState(owner);
                    break;
            }

            owner.state = nextState;
        }
    }

    private void kill(Unit unit)
    {
        unit.tile.unit = null;
        unit.QueueFree();
    }

    public override void stateExit()
    {
        die.QueueFree();
    }
}