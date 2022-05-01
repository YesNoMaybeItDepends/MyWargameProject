using Godot;
using System;

public class GameManager : Node
{
	const int GRID_COLUMNS = 15;
	const int GRID_ROWS = 15;

	Vector2 screenCenter;

	string icon = "res://Assets/icon.png";
	string green = "res://Assets/green.png";
	string lol = "res://Assets/lol.png";
	string demo = "res://Assets/demo.png";
	string polybro = "res://Assets/Polybro.png";
	
	public Camera2D camera;

	Board board;
	public Unit selectedUnit = null;
	
	bool isLeftClicking = false;
	public Inputcontroller inputController;
	public StateManager gameController;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		Helpers.SetGameManager(this);

		camera = new Camera2D();
		camera.Current = true;
		AddChild(camera);

		//DefaultState defaultState = new DefaultState();
		gameController = new StateManager();
		AddChild(gameController);

		inputController = new Inputcontroller(); 
		inputController.Initialize(gameController);
		AddChild(inputController);

		board = new Board(GRID_COLUMNS, GRID_ROWS);
		Terrain terrain = new Terrain("Plains", demo);
		board.Initialize(terrain);
		AddChild(board);
		gameController.board = board;

		Unit sneed = new Unit("sneed", icon);
		board.Grid[5,5].unit = sneed;

		Unit chuck = new Unit("sneed", icon);
		board.Grid[6,6].unit = chuck;

		//board.debugToggleTileNumbers(true, Board.coordinateTypes.offset);
	}

	// Camera Code
	// TODO Where should it go? In InputController, or CameraManager?
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
		{
			// Start left clicking
			if (mouseButton.Pressed && mouseButton.ButtonIndex == 1)
			{
				isLeftClicking = true;
			}
			// Stop left clicking
			else if (!mouseButton.Pressed && mouseButton.ButtonIndex == 1)
			{
				isLeftClicking = false;
			}
			// If scrolling wheel up
			if (mouseButton.ButtonIndex == (int)ButtonList.WheelUp)
			{
				camera.Zoom -= new Vector2(0.1f,0.1f);
			}
			else if (mouseButton.ButtonIndex == (int)ButtonList.WheelDown)
			{
				camera.Zoom += new Vector2(0.1f,0.1f);
			}
		}
		if (@event is InputEventMouseMotion mouseMotion)
		{
			if (isLeftClicking)
			{
				camera.Position -= mouseMotion.Relative;
			}
		}
    }
}
