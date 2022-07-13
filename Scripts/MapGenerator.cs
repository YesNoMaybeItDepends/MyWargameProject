using Godot;
using System;

public class MapGenerator : Node2D
{
    public OpenSimplexNoise osxn = new OpenSimplexNoise();
    public RandomNumberGenerator rng = new RandomNumberGenerator();
    public Map map;

    public terrainType[] regions;

    public MapGenerator(Map @Map)
    {
        Name = "Map Generator";
        osxn = new OpenSimplexNoise();
        rng = new RandomNumberGenerator();
        rng.Randomize();

        map = @Map;

        // TODO this shouldn't be like this
        // Load resources
        // Load resources from text file
        ResourceManager m = ServiceProvider.GetService<ResourceManager>();
        File file = new File();
        file.Open("res://Assets/Resources.txt", File.ModeFlags.Read);
        string fileContents = file.GetAsText();

        while (!file.EofReached())
        {
            var l = file.GetLine();
            m.SetResource<Texture>(l);
        }
        file.Close();

        // Get textures for the level
        Texture DeadPlainsGray_04 = m.GetResource<Texture>("PH2_DeadPlainsGray_04.png");
        Texture DesertTropic_00 = m.GetResource<Texture>("PH2_DesertTropic_00.png");
        Texture FjordSummer_08 = m.GetResource<Texture>("PH2_FjordSummer_08.png");
        Texture IslandsOceans_24 = m.GetResource<Texture>("PH2_IslandsOceans_24.png");
        Texture IslandsOceans_27 = m.GetResource<Texture>("PH2_IslandsOceans_27.png");
        Texture SmoothMtnBrn_00 = m.GetResource<Texture>("PH2_SmoothMtnBrn_00.png");
        Texture SmoothMtnBrn_12 = m.GetResource<Texture>("PH2_SmoothMtnBrn_12.png");
        Texture SmoothMtnGrn_00 = m.GetResource<Texture>("PH2_SmoothMtnGrn_00.png");
        Texture SmoothMtnGrn_12 = m.GetResource<Texture>("PH2_SmoothMtnGrn_12.png");
        Texture SmoothMtnGrn_62 = m.GetResource<Texture>("PH2_SmoothMtnGrn_62.png");

        // Generate terraintypes
        terrainType[] Regions = {
            new terrainType("Ocean", 0.3f, IslandsOceans_27),
            new terrainType("Coast", 0.4f, IslandsOceans_24),
            new terrainType("Beach", 0.5f, DesertTropic_00),
            new terrainType("Grasslands", 0.6f, SmoothMtnGrn_62),
            new terrainType("Hills", 0.7f, SmoothMtnGrn_00),
            new terrainType("Mountains", 0.8f, SmoothMtnGrn_12)
        };

        regions = Regions;
    }

    public override void _EnterTree()
    {
        PackedScene scene = (PackedScene)ResourceLoader.Load("res://Scenes/MapGenerator Panel.tscn");
        //MapGeneratorGui gui = new MapGeneratorGui();
        MapGeneratorGui gui = scene.Instance() as MapGeneratorGui;
        gui.Initialize(this);
        Control UI = GetNode("/root/Game Manager/CanvasLayer/UI") as Control;
        UI.AddChild(gui);
    }

    public void GenerateRandomMap()
    {
        GD.Print("generating random map");
        osxn.Seed = (int)rng.Randi();

        foreach (Hex hex in map.Hexes)
        {
            float thisfloat = osxn.GetNoise2d(hex.offsetPos.x, hex.offsetPos.y);
            //thisfloat = Mathf.Abs(thisfloat);
            
            // if (thisfloat < 0.1) {thisfloat = thisfloat * 10;}
            // if (thisfloat < 0.1) {GD.Print("nigga");}
            //GD.Print(thisfloat);

            for (int i = 0; i < regions.Length; i++)
            {
                if (thisfloat <= regions[i].height)
                {
                    hex.terrain.sprite.Texture = regions[i].texture;
                    break;
                }
            }

            // Color thiscolor = new Color(thisfloat,thisfloat,thisfloat);
            // Color thecolor = hex.terrain.sprite.Modulate;
            // hex.terrain.sprite.Modulate = new Color(255f*thisfloat,255f*thisfloat,255f*thisfloat);
            // GD.Print(thisfloat);
        }
        //float perlinValue = open
    }

    public void SetMapTerrain()
    {
        foreach (Hex hex in map.Hexes)
        {
            float thisfloat = osxn.GetNoise2d(hex.offsetPos.x, hex.offsetPos.y);
            //thisfloat = Mathf.Abs(thisfloat);

            for (int i = 0; i < regions.Length; i++)
            {
                if (thisfloat <= regions[i].height)
                {
                    hex.terrain.sprite.Texture = regions[i].texture;
                    break;
                }
            }
        }
    }

    public class terrainType 
    {
        public terrainType(string Name, float Height, Texture Texture)
        {
            name = Name;
            height = Height;
            texture = Texture;
        }

        public string name;
        public float height;
        public Texture texture;
    }

    public void TogglePanel()
    {
        
        Connect("", this, "");
    }
}