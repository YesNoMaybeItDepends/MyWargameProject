using Godot;
using System;
using System.Collections.Generic;

public class UnitSelectedState : State
{
    Map board;
    private SelectedPanel panel;
    private Unit _unitSelected;
    public Unit unitSelected
    {
        get
        {
            return _unitSelected;
        }
        set
        {
            // if it's the same unit
            if (value == _unitSelected)
            {
                return;
            }
            // if it's a different unit 
            else if (value != _unitSelected && _unitSelected != null)
            {
                // Deselect current unit
                // TODO we're not disposing the material, we should queuefree it or pool it or something
                _unitSelected.selected = false;
                _unitSelected.sprite.Material = null;
                RemoveMovementRangeHighlight();

                // The new unit is currently outlined red, we must undo it
                if (hoveredUnit != null)
                {
                    hoveredUnit.sprite.Material = null;
                }

                ghostDie.Visible = false;
                hoveredUnit = null;

                if (panel != null)
                {
                    panel.unit = null;
                    panel.Visible = false;
                }
            }
            
            // set new unit
            _unitSelected = value;
            
            // if unit was not null
            if (value != null)
            {
                value.selected = true;
                outlineUnit(_unitSelected, Colors.White);
                MovementRangeHighlight();
                // Update Unit Panel
                if (panel != null)
                {
                    panel.Set(value);
                }
            }
        }
    }

    public List<Hex> highlightedHexes = new List<Hex>();
    private bool _highlightMovement = false;
    public bool highlightMovement 
    {
        get {return _highlightMovement;}
        set
        {
            if (value == true)
            {
                MovementRangeHighlight();
            }
            else
            {
                RemoveMovementRangeHighlight();
            }
            
            _highlightMovement = value;
        }
    }

    Unit hoveredUnit;
    GhostDie ghostDie;

    public UnitSelectedState(StateManager Owner, Unit unit) : base(Owner)
    {
        board = ServiceProvider.GetService<GameManager>().board;

        Name = "State UnitSelected";
        
        panel = ServiceProvider.GetService<GuiManager>().selectedPanel;
        
        unitSelected = unit;

        // Generate ghost die
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
        
        // Disable active unit highlight
        if (unitSelected != null)
        {
            unitSelected.sprite.Material = null;
        }

        if (hoveredUnit != null)
        {
            // Disable hovered unit highlight
            hoveredUnit.sprite.Material = null;
        }

        // disable highlighted tiles
        highlightMovement = false;

        panel.unit = null;
        panel.Visible = false;
        
        //unitSelected = null;
    }

    public override void handleEvent(object o)
    {
        // On Right Click
        if (Input.IsActionJustPressed("mouse_click_right"))
        {
            if (o is Hex hex)
            {
                // Move
                if (unitSelected.Move(hex))// && !unitSelected.unitStateManager.state is UnitMovingState)
                {
                    // highlightMovement = false;
                    // highlightMovement = true;
                }
                // Attack
                else if (hex.unit != null && unitSelected.movementPoints > 0)
                {
                    // TODO this shouldn't be like this, the problem is we can't do == on offset coordinates yet. This should also be a function called isInRange() or something like that
                    bool inRange = false;
                    var hexNeighbours = unitSelected.tile.offsetPos.GetNeighbours();
                    foreach (OffsetCoordinates c in hexNeighbours)
                    {
                        // this is ToString() because we can't compare OffsetCoordinates yet. read above.
                        if (c.ToString() == hex.offsetPos.ToString())
                        {
                            inRange = true;
                        }
                    }
                    if (inRange)
                    {
                        if (hex.unit != unitSelected)
                        {
                            // Exhaust all AP on attack
                            unitSelected.movementPoints = 0;
                            highlightMovement = false;

                            UnitAttackingState attackState = new UnitAttackingState(owner, unitSelected, hex.unit);
                            owner.state = attackState;
                        }
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
                    unitSelected = hex.unit;
                }
                // Undo selection, return back to DefaultState
                else
                {
                    unitSelected = null;
                    owner.state = new DefaultState(owner);
                }
            }
        }
        // On Middle Click
        // DEBUG
        // TODO move to common controls
        else if (Input.IsActionJustPressed("mouse_click_middle"))
        {
            if (o is Hex hex)
            {
                hex.QueueFree();
                hex.board.Hexes[hex.offsetPos.x, hex.offsetPos.y] = null;
            }
        }
    }

    public override void handleMouseEnter(object o)
    {
        if (o is Unit unit)
        {
            if (unit != hoveredUnit && unit != unitSelected && unitSelected.movementPoints > 0)
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

    public void MovementRangeHighlight()
    {   
        if (unitSelected != null)
        {
            List<OffsetCoordinates> coordsInRange = unitSelected.tile.offsetPos.GetCoordinatesInRange(unitSelected.movementPoints);
            
            foreach (OffsetCoordinates coords in coordsInRange)
            {
                Hex hex = board.getHexAt(coords);
                if (hex != null)
                {
                    highlightedHexes.Add(hex);
                    hex.terrain.sprite.Modulate = Colors.Gray;
                }
            }
        }     
    }

    public void RemoveMovementRangeHighlight()
    {
        if (highlightedHexes.Count != 0)
        {
            foreach (Hex hex in highlightedHexes)
            {
                hex.terrain.sprite.Modulate = new Color(1,1,1);
            }

            highlightedHexes.Clear();
        }
    }
}