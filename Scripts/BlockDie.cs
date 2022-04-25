using Godot;
using System;

public class BlockDie : TextureButton
{
    public AnimatedSprite asprite;
    public ShaderMaterial shadermat;

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

    private float _sdefault = 0;
    private float _shover = 0.1f;
    private float _spressed = -0.1f;

    #region SIGNALS
    
    [Signal]
    delegate void pickedBlockDie(Faces faceSelected);
    
    #endregion 
    
    public BlockDie()
    {
        Name = "Block Die";

        rng = new RandomNumberGenerator(); 
        rng.Randomize();

        // Add sprite
        asprite = new AnimatedSprite();
        AddChild(asprite);

        // Set die sprite 
        SpriteFrames frames = GD.Load("res://Assets/BlockDice_SpriteFrames.tres") as SpriteFrames;
        asprite.Frames = frames;
        asprite.Frame = 0;
        face = (Faces)0;

        // The size of the button equals the size of the die sprite
        Vector2 frameSize = frames.GetFrame("default", 1).GetSize();
        RectSize = frameSize;

        // Buttons are not centered, so the die isn't either
        asprite.Centered = false;

        // Connect input events
        Connect("mouse_entered", this, "enter");
        Connect("button_down", this, "down");
        Connect("button_up", this, "up");
        Connect("mouse_exited", this, "exit");

        // Add shader to the die sprite
        shadermat = new ShaderMaterial();
        asprite.Material = shadermat;

        // Load shader
        shadermat.Shader = GD.Load("res://Shaders/brighten.shader") as Shader;
    }

    public Faces rollDie()
    {
        Faces result = (Faces)(rng.RandiRange(0,5));
        asprite.Frame = (int)result;
        face = result;
        return result;
    }

    public void enter()
    {
        shadermat.SetShaderParam("bright_amount", _shover);
    }

    public void down()
    {
        shadermat.SetShaderParam("bright_amount", _spressed);

        EmitSignal("pickedBlockDie", (Faces)asprite.Frame);

        if (asprite.Frame == 5)
        {
            asprite.Frame = 0;
        }
        asprite.Frame++;


    }
    public void up()
    {   
        shadermat.SetShaderParam("bright_amount", _shover);
    }

    public void exit()
    {
        shadermat.SetShaderParam("bright_amount", _sdefault);
    }

}