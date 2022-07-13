using Godot;
using System;

public class TerrainContainer : Panel
{
    TextureRect TerrainTexture;
    Label TerrainName;
    Label TerrainHeight;
    Button IncreaseButton;
    Button DecreaseButton;
    VSlider Slider;

    MapGeneratorGui parent;
    MapGenerator.terrainType terrain;

    public override void _EnterTree()
    {
        TerrainTexture = GetNode("Terrain Container/Terrain Texture") as TextureRect;
        TerrainName = GetNode("Terrain Container/VBoxContainer/Terrain Name") as Label;
        TerrainHeight = GetNode("Terrain Container/VBoxContainer/HBoxContainer/Terrain Height") as Label;
        IncreaseButton = GetNode("Terrain Container/VBoxContainer/HBoxContainer/VBoxContainer/Increase Button") as Button;
        DecreaseButton = GetNode("Terrain Container/VBoxContainer/HBoxContainer/VBoxContainer/Decrease Button") as Button;
        Slider = GetNode("Terrain Container/VSlider") as VSlider;

        if (parent != null)
        {
            TerrainName.Text = terrain.name;
            TerrainHeight.Text = terrain.height.ToString();
            TerrainTexture.Texture = terrain.texture;

            IncreaseButton.Connect("pressed", this, "onIncreaseButtonPressed");
            DecreaseButton.Connect("pressed", this, "onDecreaseButtonPressed");
            Slider.Connect("value_changed", this, "onSliderChanged");
        }
    }

    void onIncreaseButtonPressed()
    {
        terrain.height = terrain.height + 0.05f;
        TerrainHeight.Text = terrain.height.ToString();
        // if (TerrainHeight.Text.Length > 3)
        // {
        //     TerrainHeight.Text = TerrainHeight.Text.Substring(0, 4);
        // }
        parent.mapgen.SetMapTerrain();
    }

    void onSliderChanged(float value)
    {
        terrain.height = value;
        TerrainHeight.Text = terrain.height.ToString();
        parent.mapgen.SetMapTerrain();
    }

    void onDecreaseButtonPressed()
    {
        terrain.height = terrain.height - 0.05f;
        TerrainHeight.Text = terrain.height.ToString();
        // if (TerrainHeight.Text.Length > 3)
        // {
        //     TerrainHeight.Text = TerrainHeight.Text.Substring(0, 4);
        // }
        parent.mapgen.SetMapTerrain();
    }

    public void Initialize(MapGeneratorGui Parent, MapGenerator.terrainType Terrain)
    {
        parent = Parent;
        terrain = Terrain;

        if (IsInsideTree())
        {

        }
        else
        {
            GD.PushError("TerrainContainer not in tree, can't initialize");
        }
    }
}