using Godot;
using System;

public class MapGeneratorGui : Panel
{
    public MapGenerator mapgen;

    HSlider OctavesSlider;
    HSlider PeriodSlider;
    HSlider PersistenceSlider;
    HSlider LacunaritySlider;
    Button SeedButton;

    Label OctavesLabel;
    Label PeriodLabel;
    Label PersistenceLabel;
    Label LacunarityLabel;
    Label SeedLabel;

    Sprite NoiseMap;
    Texture NoiseMapTexture;

    CheckButton GrayscaleToggle;
    bool grayscaleToggled = false;

    ScrollContainer ScrollableTerrainList;
    VBoxContainer Terrains;

    public MapGeneratorGui()
    {
        Name = "Map Generator Gui";
    }

    public void Initialize(MapGenerator Mapgen)
    {
        GD.Print("init");
        mapgen = Mapgen;
    }

    public override void _EnterTree()
    {
        GD.Print("entered tree");
        OctavesSlider = GetNode("Octaves Slider") as HSlider;
        PeriodSlider = GetNode("Period Slider") as HSlider;
        PersistenceSlider = GetNode("Persistence Slider") as HSlider;
        LacunaritySlider = GetNode("Lacunarity Slider") as HSlider;
        SeedButton = GetNode("Seed Button") as Button;

        OctavesLabel = GetNode("Octaves Value") as Label;
        PeriodLabel = GetNode("Period Value") as Label;
        PersistenceLabel = GetNode("Persistence Value") as Label;
        LacunarityLabel = GetNode("Lacunarity Value") as Label;
        SeedLabel = GetNode("Seed Value") as Label;

        NoiseMap = GetNode("Noise Map") as Sprite;
        NoiseMapTexture = NoiseMap.Texture;

        GrayscaleToggle = GetNode("Grayscale Toggle") as CheckButton;

        OctavesSlider.Connect("value_changed", this, "onOctavesChanged");
        PeriodSlider.Connect("value_changed", this, "onPeriodChanged");
        PersistenceSlider.Connect("value_changed", this, "onPersistenceChanged");
        LacunaritySlider.Connect("value_changed", this, "onLacunarityChanged");
        SeedButton.Connect("pressed", this, "onSeedPressed");

        GrayscaleToggle.Connect("toggled", this, "onGrayscaleToggled");

        PackedScene s = (PackedScene)GD.Load("res://Scenes/Scrollable Terrain List.tscn");
        ScrollableTerrainList = s.Instance() as ScrollContainer;
        AddChild(ScrollableTerrainList);
        ScrollableTerrainList.RectPosition += new Vector2(0,RectSize.y+20);
        
        Terrains = ScrollableTerrainList.GetNode("Terrains") as VBoxContainer;

        s = (PackedScene)GD.Load("res://Scenes/Terrain Container.tscn");
        for (int i = 0; i != mapgen.regions.Length; i++)
        {
            TerrainContainer tc = s.Instance() as TerrainContainer;
            tc.Initialize(this, mapgen.regions[i]);
            Terrains.AddChild(tc);
        }
    }

    public override void _ExitTree()
    {
        OctavesSlider.Disconnect("value_changed", this, "onOctavesChanged");
        PeriodSlider.Disconnect("value_changed", this, "onPeriodChanged");
        PersistenceSlider.Disconnect("value_changed", this, "onPersistenceChanged");
        LacunaritySlider.Disconnect("value_changed", this, "onLacunarityChanged");
    }

    public void changeNoiseMapTexture()
    {
        ImageTexture img = new ImageTexture();
        img.CreateFromImage(mapgen.osxn.GetImage(30, 30));
        NoiseMap.Texture = img; 
    }

    public void onOctavesChanged(float octaves)
    {
        OctavesLabel.Text = octaves.ToString();
        mapgen.osxn.Octaves = (int)octaves;
        mapgen.GenerateRandomMap();
        changeNoiseMapTexture();
        onGrayscaleToggled(grayscaleToggled);
    }

    public void onPeriodChanged(float period)
    {
        PeriodLabel.Text = period.ToString();
        mapgen.osxn.Period = period;
        mapgen.GenerateRandomMap();
        changeNoiseMapTexture();
        onGrayscaleToggled(grayscaleToggled);
    }

    public void onPersistenceChanged(float persistence)
    {
        PersistenceLabel.Text = persistence.ToString();
        mapgen.osxn.Persistence = persistence;
        mapgen.GenerateRandomMap();
        changeNoiseMapTexture();
        onGrayscaleToggled(grayscaleToggled);
    }

    public void onLacunarityChanged(float lacunarity)
    {
        LacunarityLabel.Text = lacunarity.ToString();
        mapgen.osxn.Lacunarity = lacunarity;
        mapgen.GenerateRandomMap();
        changeNoiseMapTexture();
        onGrayscaleToggled(grayscaleToggled);
    }

    public void onSeedPressed()
    {
        mapgen.osxn.Seed = (int)mapgen.rng.Randi();
        SeedLabel.Text = mapgen.osxn.Seed.ToString();
        mapgen.GenerateRandomMap();
        changeNoiseMapTexture();
        onGrayscaleToggled(grayscaleToggled);
    }

    public void onGrayscaleToggled(bool value)
    {
        if (value == true)
        {
            grayscaleToggled = true;
            mapToGrayscale();
        }
        else
        {
            grayscaleToggled = false;
            foreach(Hex hex in mapgen.map.Hexes)
            {
                hex.terrain.sprite.Modulate = new Color(1,1,1);
            }
            mapgen.SetMapTerrain();
        }
    }

    public void mapToGrayscale()
    {
        foreach (Hex hex in mapgen.map.Hexes)
        {
            float thisfloat = mapgen.osxn.GetNoise2d(hex.offsetPos.x, hex.offsetPos.y);
            //thisfloat = Mathf.Abs(thisfloat);
            Color c = Colors.Black;
            c = c.LinearInterpolate(Colors.White, thisfloat);
            hex.terrain.sprite.Modulate = c;
        }
    }
}