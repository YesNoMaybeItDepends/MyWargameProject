using Godot;
using System;

public class Inputcontroller : Node2D
{
    public sneed Level;
    public RayCast2D raycast;
    public StateManager gameController;

    public Inputcontroller()
    {
        GD.Print("InputController constructor");
        raycast = new RayCast2D();
        raycast.Enabled = true;
        raycast.CastTo = Vector2.One;
        raycast.CollideWithAreas = true;

        AddChild(raycast);
    }

    public void Initialize(StateManager GameController)
    {
        
        gameController = GameController;
    }

    // public override void _PhysicsProcess(float delta)
    // {
    //     //var rayLength = 100;
    //     //var mousePos = GetViewport().GetMousePosition();
    //     var mousePos = GetGlobalMousePosition();
    //     // raycast.Position = mousePos;
    //     // //raycast.Transform.origin = Level.camera.
    //     // raycast.ForceRaycastUpdate();
    //     // if (raycast.IsColliding())
    //     // {
            
    //     //     var collider = raycast.GetCollider();
    //     //     if (collider is Tile tile)
    //     //     {
    //     //         //var lol = Input.GetMouseMode();
    //     //         GD.Print("WE FREAKIN GOTTEM REDDIT HELL YEAAAAAA");
    //     //         GD.Print(tile.Name);
    //     //         tile.handleInput();
    //     //     }
    //     // }


    //     var spaceState = GetWorld2d().DirectSpaceState;
    //     //var result = spaceState.IntersectRay(mousePos, mousePos, collideWithAreas: true);
    //     Godot.Collections.Array result = spaceState.IntersectPoint(mousePos, collideWithAreas: true);
        
    //     if (result.Count > 0)
    //     {
            
    //         foreach (Godot.Collections.Dictionary collider in result)
    //         {
    //             if (collider["collider"] is Iinput col)
    //             {
    //                 //col.handleInput();
    //                 gameController.handleInput(result);
    //                 break;
    //             }
    //         }
    //     }   
    // }
}