using Godot;
using System;

public class BlockDice : Area2D
{
    public AnimatedSprite sprite;
    //public Texture texture;
    
    public enum Faces 
    {
        AttackerDown,
        DefenderPushed_1,
        DefenderDown,
        DefenderStumbles,
        DefenderPushed_2,
        BothDown,
    }

    public const int sides = 6;

    public RandomNumberGenerator rng;

    public Faces face;

    public BlockDice()
    {
        Name = "Block Dice";

        rng = new RandomNumberGenerator(); 
        rng.Randomize();

        sprite = new AnimatedSprite();

        SpriteFrames frames = GD.Load("res://Assets/BlockDice_SpriteFrames.tres") as SpriteFrames;
        sprite.Frames = frames;
        sprite.Frame = 0;
        face = (Faces)0;

        // TODO Add Collision Polygon
        // HexCollisionPolygon = new CollisionPolygon2D();
        // HexCollisionPolygon.Polygon = HexPolygonVector;
        // HexCollisionPolygon.Position = new Vector2(-36, -36); // TODO these should be 
        // AddChild(HexCollisionPolygon);

        AddChild(sprite);
    }

    public Faces rollDie()
    {
        Faces result = (Faces)(rng.RandiRange(0,5));
        sprite.Frame = (int)result;
        face = result;
        return result;
    }
}