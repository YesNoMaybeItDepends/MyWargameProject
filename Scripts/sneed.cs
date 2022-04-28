using Godot;
using System;

public class sneed : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	const int GRID_HEIGHT = 4;
	const int GRID_WIDTH = 4;

	int[,] grid = new int[GRID_WIDTH, GRID_HEIGHT];

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

	public BlockDie dice;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		//camera = new Camera2D();
		//AddChild(camera);
		//DefaultState defaultState = new DefaultState();
		gameController = new StateManager();
		AddChild(gameController);

		inputController = new Inputcontroller(); 
		inputController.Initialize(gameController);
		AddChild(inputController);
		
		camera = GetChild(0) as Camera2D;
		camera.Current = true;

		Terrain terrain = new Terrain("Plains", demo);
		board = new Board(16,16);
		board.Initialize(terrain);
		AddChild(board);
		gameController.board = board;

		Unit sneed = new Unit("sneed", icon);
		board.Grid[5,5].unit = sneed;

		Unit chuck = new Unit("sneed", icon);
		board.Grid[6,6].unit = chuck;

		board.debugToggleTileNumbers(true, Board.coordinateTypes.offset);
	}

	void ligma(BlockDie.Faces face)
	{
		GD.Print(face);
	}

	void onUnitSelected(Unit unit)
	{
		selectedUnit = unit;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		int velocity = 50;

		if (Input.IsMouseButtonPressed(1))
		{

		}
	}



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
		if (@event is InputEventKey key)
		{
			if (key.Scancode == (int)KeyList.Space && key.Pressed)
			{
				dice.rollDie();
			}
		}
    }
}
