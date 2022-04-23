using Godot;
using System;
using System.Collections.Generic;

public class Board : Node2D
{
    public Hex[,] Grid;

    int GridRows;
    int GridColumns;

    public int SpriteWidth = 96;
    public int SpriteHeight = 96;
    public float outerRadius = 96/2;
    public float innerRadius = (72/2)*0.866025404f;

    private bool debugTileNumbers = false;

    // public int SpriteWidth = 64;
    // public int SpriteHeight = 64;

    Vector2 OriginPosition = Vector2.Zero;

    public Board(int gridHeight, int gridWidth)
    {
        Name = "Board";

        Position = OriginPosition;

        GridRows = gridWidth;
        GridColumns = gridHeight;
        Grid = new Hex[gridWidth,gridHeight];
    }

    public void Initialize(Terrain terrain)
    {

        double hexY = outerRadius * Math.Sqrt(3);
        double hexX = outerRadius * 2;
        double hexHorizontalSpacing = hexX * 0.75;

        for (int col = 0; col < GridColumns; col++)
        {
            for (int row = 0; row < GridRows; row++)
            {              
                // Make Tile
                Hex tile = new Hex(new Vector2(col,row));
                
                // Set Tile Terrain
                Terrain nuTerrain = new Terrain(terrain.name, terrain.sprite.Texture.ResourcePath);
                tile.terrain = nuTerrain;
                
                // Set Tile Position
                Vector2 position = new Vector2();
                position.x = col * (float)hexHorizontalSpacing;
                position.y = (((float)hexY/2) * (col%2)) + (row*(float)hexY);
                tile.Position = position;
				
                // Add Tile to Grid
                Grid[col,row] = tile;
                
                // Set Tile Name
                tile.Name = $"{col},{row}";

                // Append Tile to Board
                AddChild(tile);
			}
		}
    }

    public Hex getHexAt(OffsetCoordinates offsetCoords)
    {
        Hex hex = null;
        
        int x = offsetCoords.x;
        int y = offsetCoords.y;

        if ((x >= 0 && x < GridColumns) && (y >= 0 && y < GridRows))
        {
            if (Grid[x, y] != null)
            {
                hex = Grid[x, y];
            }
        }

        return hex;
    }

    public enum coordinateTypes
    {
        offset,
        axial,
        cube
    }

    public void debugToggleTileNumbers(bool value, coordinateTypes coordinateType)
    {
        bool changeRequired = false;
        if ((value == true && debugTileNumbers == false) || (value == false && debugTileNumbers == true))
        {
            changeRequired = true;
        }
        if (changeRequired)
        {
            for (int row = 0; row < GridRows; row++)
            {
                for (int col = 0; col < GridColumns; col++)
                {
                    if (value == true)
                    {
                        Label label = new Label();
                        
                        DynamicFont font = new DynamicFont();
                        font.FontData = GD.Load("res://Assets/OpenSans-Regular.ttf") as DynamicFontData;
                        //font.Size = 24;
                        font.Size = 12;
                        font.OutlineColor = Colors.Black;
                        font.OutlineSize = 3;
                        Theme theme = new Theme();
                        theme.DefaultFont = font;
                        
                        label.Theme = theme;
                        label.AddColorOverride("font_color", Colors.White);
                        
                        switch (coordinateType)
                        {
                            case coordinateTypes.offset:
                                label.Text = col+","+row;
                                break;
                            case coordinateTypes.axial:
                                label.Text = Grid[col,row].axialPos.ToStringOnSeparateLines();
                                break;
                            case coordinateTypes.cube:
                                label.Text = "ICE CUBE MUTHAFUGGA";
                                break;
                        }
                        
                        label.RectPosition -= (new Vector2(SpriteHeight, SpriteWidth)*0.25f);

                        Grid[col,row].AddChild(label);
                        label.Name = "TileNumber";

                        debugTileNumbers = true;
                    }
                    else
                    {
                        Label label = Grid[col,row].GetNodeOrNull("TileNumber") as Label;
                        if (label != null)
                        {
                            label.QueueFree();
                            debugTileNumbers = false;
                        }
                    }
                }
            }
        }
    }

}