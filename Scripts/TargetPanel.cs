using Godot;
using System;

public class TargetPanel : Panel
{
    public Unit unit;
    TextureRect Sprite;
    VBoxContainer Stats;
    Label Stats_Name;
    Label Stats_HP;
    Label Stats_AP;
    Label Stats_STR;
    public Control placeholder;

    public override void _EnterTree()
    {
        placeholder = GetNode("../Target Placeholder") as Control;
        Sprite = GetNode("HBoxContainer/Sprite") as TextureRect;
        Stats = GetNode("HBoxContainer/Stats") as VBoxContainer;
        if (Stats != null)
        {
            Stats_Name = Stats.GetNode("Name") as Label;
            Stats_HP = Stats.GetNode("HP") as Label;
            Stats_AP = Stats.GetNode("AP") as Label;
            Stats_STR = Stats.GetNode("STR") as Label;
        }

        Connect("visibility_changed", this, "OnVisibilityChange");
    }

    public void OnVisibilityChange()
    {
        ServiceProvider.GetService<GuiManager>().OnVisibilityChange(this);
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