using Godot;
using System;

public class SelectedPanel : Panel
{
    public Unit unit = null;
    TextureRect Sprite;
    VBoxContainer Stats;
    Label Stats_Name;
    Label Stats_HP;
    Label Stats_AP;
    Label Stats_STR;

    public SelectedPanel()
    {
        // GD.Print("HELLO REDDIT HELLO HELLO GOOD MORNING REDDIT");
        // if (IsInsideTree())
        // {
        //     Sprite = GetNode("HBoxContainer/Sprite") as TextureRect;
        //     Stats = GetNode("HBoxContainer/Stats") as VBoxContainer;
        //     if (Stats != null)
        //     {
        //         Stats_Name = Stats.GetNode("Name") as Label;
        //         Stats_HP = Stats.GetNode("HP") as Label;
        //         Stats_AP = Stats.GetNode("AP") as Label;
        //         Stats_STR = Stats.GetNode("STR") as Label;
        //     }
        // }
        Connect("visibility_changed", this, "OnVisibilityChange");
    }

    public void OnVisibilityChange()
    {
        ServiceProvider.GetService<GuiManager>().OnVisibilityChange(this);
    }

    public void Init()
    {
        
    }

    public override void _EnterTree()
    {
        Sprite = GetNode("HBoxContainer/Sprite") as TextureRect;
        Stats = GetNode("HBoxContainer/Stats") as VBoxContainer;
        if (Stats != null)
        {
            Stats_Name = Stats.GetNode("Name") as Label;
            Stats_HP = Stats.GetNode("HP") as Label;
            Stats_AP = Stats.GetNode("AP") as Label;
            Stats_STR = Stats.GetNode("STR") as Label;
        }
    }

    public void Set(Unit Unit)
    {
        Visible = true;

        unit = Unit;

        Sprite.Texture = unit.sprite.Texture;
        Stats_Name.Text = unit.Name;
        Stats_HP.Text = "lol";
        Stats_AP.Text = unit.movementPoints.ToString();
        Stats_STR.Text = "3";
    }
}