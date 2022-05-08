using Godot;
using System;

public class GuiManager : Node, IService
{

    public Control UI;
    public Control selectedPlaceholder;    
    public SelectedPanel selectedPanel;
    public Control targetPlaceholder;
    public TargetPanel targetPanel;

    public GuiManager()
    {

    }

    public override void _EnterTree()
    {
        UI = GetNode("/root/Node2D/CanvasLayer/UI") as Control;
        selectedPanel = UI.GetNode("Bottom/Unit Panel") as SelectedPanel;
        targetPanel = UI.GetNode("Bottom/Target Panel") as TargetPanel;
        selectedPlaceholder = UI.GetNode("Bottom/Selected Placeholder") as Control;
        targetPlaceholder = UI.GetNode("Bottom/Target Placeholder") as Control;
    }

    public void OnVisibilityChange(Godot.Object o)
    {
        if (o is TargetPanel t)
        {
            if (t.Visible) targetPlaceholder.Visible = false;
            else targetPlaceholder.Visible = true;
        }
        else if (o is SelectedPanel s)
        {
            if (s.Visible) selectedPlaceholder.Visible = false;
            else selectedPlaceholder.Visible = true;
        }
    }
}