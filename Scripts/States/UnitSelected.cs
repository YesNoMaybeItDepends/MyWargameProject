using Godot;
using System;
using System.Collections.Generic;

public class UnitSelected : State
{
    public Unit unitSelected;
    public List<Hex> highlightedHexes = new List<Hex>();

    public UnitSelected(StateManager Owner, Unit unit) : base(Owner)
    {
        Name = "State UnitSelected";

        unitSelected = unit;
    }

    public override void stateEnter()
    {
        highlightMovementRange();
    }

    public override void handleInput(Godot.Collections.Array ColliderDicts)
    {
        // Right Click
        if (Input.IsActionJustPressed("mouse_click_right"))
        {
            foreach (Godot.Collections.Dictionary collider in ColliderDicts)
            {
                if (collider["collider"] is Hex hex)
                {
                    // Move
                    if (hex.unit is null && unitSelected.movementPoints > 0)
                    {
                        unitSelected.tile.unit = null;
                        hex.unit = unitSelected;
                        unitSelected.movementPoints--;
                        foreach (Hex h in highlightedHexes)
                        {
                            h.terrain.sprite.Modulate = new Color(1,1,1);
                        }
                        highlightMovementRange();
                    }
                    // Attack
                    else if (hex.unit != null && unitSelected.movementPoints > 0)
                    {
                        if (hex.unit != unitSelected)
                        {
                            // Get or spawn block dice
                            BlockDice dice = unitSelected.GetNode("Block Dice") as BlockDice;
                            if (dice is null)
                            {
                                dice = new BlockDice();
                                unitSelected.AddChild(dice);
                                dice.GlobalPosition = new Vector2(-96,-96);
                            }
                            
                            // Roll dice
                            dice.rollDie();
                            
                            // Handle resultt
                            switch (dice.face)
                            {
                                case BlockDice.Faces.AttackerDown:
                                    GD.Print("AttackerDown");
                                    break;
                                case BlockDice.Faces.DefenderPushed_1:
                                    GD.Print("DefenderPushed_1");
                                    break;
                                case BlockDice.Faces.DefenderDown:
                                    GD.Print("DefenderDown");
                                    hex.unit.QueueFree();
                                    hex.unit = null;
                                    break;
                                case BlockDice.Faces.DefenderStumbles:
                                    GD.Print("DefenderStumbles");
                                    break;
                                case BlockDice.Faces.DefenderPushed_2:
                                    GD.Print("DefenderPushed_2");
                                    break;
                                case BlockDice.Faces.BothDown:
                                    GD.Print("BothDown");
                                    break;
                            }

                            // Exhaust all AP on attack
                            unitSelected.movementPoints = 0;
                            
                            // unitSelected.RemoveChild(dice);
                            // dice.QueueFree();
                        }
                    }
                }
            }
        }
        // Left Click
        else if (Input.IsActionJustPressed("mouse_click_left"))
        {
            foreach (Godot.Collections.Dictionary collider in ColliderDicts)
            {
                if (collider["collider"] is Hex hex)
                {
                    // Select a different Unit
                    if (hex.unit != null)
                    {
                        unitSelected.sprite.Modulate = new Color(1,1,1);
                        unitSelected = hex.unit;
                        unitSelected.sprite.Modulate = Colors.GreenYellow;
                        foreach (Hex h in highlightedHexes)
                        {
                            h.terrain.sprite.Modulate = new Color(1,1,1);
                        }
                        highlightMovementRange();
                    }
                    // Undo selection, return back to DefaultState
                    else
                    {
                        unitSelected.sprite.Modulate = new Color(1,1,1);
                        foreach (Hex h in highlightedHexes)
                        {
                            h.terrain.sprite.Modulate = new Color(1,1,1);
                        }
                        owner.state = new DefaultState(owner);
                    }
                }
                // Click on a block die
                else if (collider["collider"] is BlockDice dice)
                {

                }
            }
        }
    } 

    public void highlightMovementRange()
    {        
        List<OffsetCoordinates> coordsInRange = unitSelected.tile.offsetPos.GetCoordinatesInRange(unitSelected.movementPoints);
        
        foreach (OffsetCoordinates coords in coordsInRange)
        {
            Hex hex = owner.board.getHexAt(coords);
            if (hex != null)
            {
                highlightedHexes.Add(hex);
                hex.terrain.sprite.Modulate = Colors.Gray;
            }
        }
    }
}