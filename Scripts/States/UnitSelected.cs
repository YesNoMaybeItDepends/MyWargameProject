using Godot;
using System;
using System.Collections.Generic;

public class UnitSelectedState : State
{
    public Unit unitSelected;
    public List<Hex> highlightedHexes = new List<Hex>();

    Unit hoveredUnit;
    GhostDie ghostDie;

    public UnitSelectedState(StateManager Owner, Unit unit) : base(Owner)
    {
        Name = "State UnitSelected";

        unitSelected = unit;
    }

    public override void stateEnter()
    {
        // TODO somehow mark what unit is selected
        //unitSelected.sprite.Modulate = Colors.GreenYellow;
        //unitSelected.sprite.Modulate = Colors.Green;
        // READ THIS https://godotengine.org/qa/1616/z-value-for-custom-draw-calls
        
        //VisualServer.CanvasItemAddCircle()
        
        outlineUnit(unitSelected, Colors.White);

        highlightMovementRange();

        ghostDie = new GhostDie();
        AddChild(ghostDie);
        Vector2 frameSize = ghostDie.Frames.GetFrame("default", 1).GetSize();
        ghostDie.Visible = false;
    }

    void outlineUnit(Unit unit, Color color)
    {
        ShaderMaterial shaderMat = new ShaderMaterial();
        Shader shader = GD.Load("res://Shaders/Outline.tres") as Shader;
        shaderMat.Shader = shader;
        shaderMat.SetShaderParam("stroke",2.5f);
        shaderMat.SetShaderParam("outline_color", color);
        unit.sprite.Material = shaderMat;
    }

    public override void stateExit()
    {
        unitSelected.sprite.Material = null;
    }

    public override void _EnterTree()
    {
        
    }

    public override void handleEvent(object o)
    {
        // On Right Click
        if (Input.IsActionJustPressed("mouse_click_right"))
        {
            if (o is Hex hex)
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
                        // get ghost die
                        // activate it
                        // switch to attack state
                        UnitAttackingState attackState = new UnitAttackingState(owner, unitSelected, hex.unit);
                        owner.state = attackState;

                        // Exhaust all AP on attack
                        unitSelected.movementPoints = 0;
                    }
                }
            }
        }
        // On Left Click
        else if (Input.IsActionJustPressed("mouse_click_left"))
        {
            if (o is Hex hex)
            {
                // Select a different Unit
                if (hex.unit != null)
                {
                    unitSelected.sprite.Material = null;
                    unitSelected = hex.unit;
                    outlineUnit(unitSelected, Colors.White);
                    ghostDie.Visible = false;
                    hoveredUnit = null;
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
        }
    }

    public override void handleMouseEnter(object o)
    {
        if (o is Unit unit)
        {
            if (unit != hoveredUnit && unit != unitSelected)
            {
                    hoveredUnit = unit;

                    outlineUnit(hoveredUnit,Colors.Red);

                    Vector2 frameSize = ghostDie.Frames.GetFrame("default", 1).GetSize();
                    
                    float x = unit.GlobalPosition[0];
                    float y = unit.GlobalPosition[1] - frameSize.y*1f;
                    ghostDie.GlobalPosition = new Vector2(x,y);
                    
                    ghostDie.Visible = true;
            }
        }
    }

    public override void handleMouseExit(object o)
    {
        if (o is Unit unit)
        {
            if (unit == hoveredUnit)
            {
                if (unit != unitSelected)
                {
                    hoveredUnit.sprite.Material = null;
                }
                ghostDie.Visible = false;
                hoveredUnit = null;
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