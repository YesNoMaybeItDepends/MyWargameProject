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
        switch (die.face)
            {
                case BlockDie.Faces.AttackerDown:
                    GD.Print("AttackerDown");
                    break;
                case BlockDie.Faces.DefenderPushed_1:
                    GD.Print("DefenderPushed_1");
                    break;
                case BlockDie.Faces.DefenderDown:
                    GD.Print("DefenderDown");
                    target.QueueFree();
                    target.tile.unit = null;
                    break;
                case BlockDie.Faces.DefenderStumbles:
                    GD.Print("DefenderStumbles");
                    break;
                case BlockDie.Faces.DefenderPushed_2:
                    GD.Print("DefenderPushed_2");
                    break;
                case BlockDie.Faces.BothDown:
                    GD.Print("BothDown");
                    break;
            }
    }
}