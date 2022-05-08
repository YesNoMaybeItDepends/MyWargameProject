using Godot;
using System;

public class GameManager : Node, IService
{
	const int GRID_COLUMNS = 15;
	const int GRID_ROWS = 15;

	Vector2 screenCenter;

	string icon = "res://Assets/icon.png";
	string green = "res://Assets/green.png";
	string lol = "res://Assets/lol.png";
	string demo = "res://Assets/demo.png";
	string polybro = "res://Assets/Polybro.png";
	string sneed_sprite = "res://Assets/sneed.png";
	string chuck_sprite = "res://Assets/chuck.png";

	public Camera2D camera;

	Board board;
	public Unit selectedUnit = null;
	
	bool isLeftClicking = false;
	public Inputcontroller inputController;
	public StateManager stateManager;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		ServiceProvider.SetService<GameManager>(this);

		camera = new Camera2D();
		camera.Current = true;
		AddChild(camera);

		//DefaultState defaultState = new DefaultState();
		stateManager = new StateManager();
		AddChild(stateManager);

		inputController = new Inputcontroller(); 
		AddChild(inputController);
		inputController.Initialize(stateManager);

		GuiManager guiManager = new GuiManager();
		AddChild(guiManager);
		ServiceProvider.SetService<GuiManager>(guiManager);

		board = new Board(GRID_COLUMNS, GRID_ROWS);
		Terrain terrain = new Terrain("Plains", demo);
		board.Initialize(terrain);
		AddChild(board);
		stateManager.board = board;

		Unit sneed = new Unit("Sneed", sneed_sprite);
		board.Grid[5,5].unit = sneed;

		Unit chuck = new Unit("Chuck", chuck_sprite);
		board.Grid[6,6].unit = chuck;

		Unit poly = new Unit("Polybro", polybro);
		board.Grid[7,7].unit = poly;

		Unit godo = new Unit("Godot", icon);
		board.Grid[8,8].unit = godo;
		
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
