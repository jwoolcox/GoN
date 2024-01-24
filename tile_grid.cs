using Godot;
using System;
using System.Collections.Generic;

public partial class tile_grid : Node2D
{
	[Signal]
	public delegate void PlayingStateChangedEventHandler(bool IsRunning);

	const int BLOCK_COLS = 36;
	const int BLOCK_ROWS = 24;
	private NeighbourTile[][] _NeighbourTiles = new NeighbourTile[BLOCK_ROWS][];
	private bool _IsPlaying = false;

	public enum EBlockDir
	{
		TL,         //TOP LEFT
		TC,         //TOP CENTER
		TR,         //TOP RIGHT
		CL,         //CENTER LEFT
		CR,         //CENTER RIGHT
		BL,         //BOTOM LEFT
		BC,         //BOTTOM CENTER
		BR          //BOTTOM RIGHT
	}

	public void SetPlayingState(double PlayingSpeed)
	{
		Timer timer = GetNode<Timer>("Timer");

		if (_IsPlaying)
		{
			timer.Stop();
			_IsPlaying = false;
		}
		else
		{
			_IsPlaying = true;

			//start timer, and run
			timer.WaitTime = PlayingSpeed;
			timer.Start();
		}

		EmitSignal(SignalName.PlayingStateChanged, new Variant[] { _IsPlaying });
	}

	public void ResetAllTiles()
	{
		if (_IsPlaying)
		{
			SetPlayingState(0.0);
		}

		foreach (NeighbourTile[] Row in _NeighbourTiles)
		{
			foreach (NeighbourTile Tile in Row)
			{
				Tile.SetAlive(false);
			}
		}
	}

	public void OnTimerTimeout()
	{
		_InternalTick();

		if (_IsPlaying)
		{
			//start timer, and run
			GetNode<Timer>("Timer").Start();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//load the tile scene
		PackedScene scene = GD.Load<PackedScene>("res://neighbour_tile.tscn");

		//splat the tiles
		for (int row = 0; row < _NeighbourTiles.Length; row++)
		{
			NeighbourTile[] tilesRow = new NeighbourTile[BLOCK_COLS];

			for (int col = 0; col < tilesRow.Length; col++)
			{
				NeighbourTile instance = scene.Instantiate<NeighbourTile>();
				instance.MoveLocalX(28 * col);
				instance.MoveLocalY(28 * row);
				AddChild(instance);
				tilesRow[col] = instance;
			}

			_NeighbourTiles[row] = tilesRow;
		}
	}

	private int ResolveOffset(int Src, int Offset, bool IsRow)
	{
		int result = 0;
		int testValue = 0;

		if (IsRow)
			testValue = BLOCK_ROWS - 1;
		else
			testValue = BLOCK_COLS - 1;

		if (Src == 0 && Offset < 0)
		{
			result = testValue + (Offset + 1);
		}
		else if (Src == testValue && Offset > 0)
		{
			result = (Offset - 1);
		}
		else
		{
			result = Src + Offset;
		}



		return result;
	}

	private int ResolveOffsetCol(int SrcCol, int Offset)
	{
		return ResolveOffset(SrcCol, Offset, false);
	}

	private int ResolveOffsetRow(int SrcRow, int Offset)
	{
		return ResolveOffset(SrcRow, Offset, true);
	}

	private int IsNeighbourAlive(int SrcRow, int SrcCol, EBlockDir BlockDir)
	{
		int ColOffset = 0;
		int RowOffset = 0;

		switch (BlockDir)
		{
			case EBlockDir.TL:
				ColOffset = -1;
				RowOffset = -1;
				break;
			case EBlockDir.TC:
				RowOffset = -1;
				break;
			case EBlockDir.TR:
				ColOffset = 1;
				RowOffset = -1;
				break;
			case EBlockDir.CL:
				ColOffset = -1;
				break;
			case EBlockDir.CR:
				ColOffset = 1;
				break;
			case EBlockDir.BL:
				RowOffset = 1;
				ColOffset = -1;
				break;
			case EBlockDir.BC:
				RowOffset = 1;
				break;
			case EBlockDir.BR:
				RowOffset = 1;
				ColOffset = 1;
				break;
		}

		int LookUpCol = ResolveOffsetCol(SrcCol, ColOffset);
		int LookUpRow = ResolveOffsetRow(SrcRow, RowOffset);
		bool IsTileAlive = _NeighbourTiles[LookUpRow][LookUpCol].IsAlive();

		//GD.Print($"Tile [{SrcRow}, {SrcCol}] Neighbour [{LookUpRow}, {LookUpCol}] : {IsTileAlive}");

		return IsTileAlive ? 1 : 0;
	}

	private int CountLivingNeighbours(int SrcRow, int SrcCol)
	{
		int result = 0;

		foreach (EBlockDir dir in Enum.GetValues(typeof(EBlockDir)))
		{
			result += IsNeighbourAlive(SrcRow, SrcCol, dir);
		}

		return result;
	}

	/*
	 * If the cell is alive, then it stays alive if it has either 2 or 3 live 
	 * neighbors. 
	 * If the cell is dead, then it springs to life only in the case that it 
	 * has 3 live neighbors
	 */
	private void _InternalTick()
	{
		LinkedList<NeighbourTile> wakeTiles = new LinkedList<NeighbourTile>();
		LinkedList<NeighbourTile> killTiles = new LinkedList<NeighbourTile>();

		//run the blocks
		for (int row = 0; row < BLOCK_ROWS; row++)
		{
			for (int col = 0; col < BLOCK_COLS; col++)
			{
				NeighbourTile tile = _NeighbourTiles[row][col];

				int livingNeighbourCount = CountLivingNeighbours(row, col);

				if (tile.IsAlive() && (livingNeighbourCount < 2 || livingNeighbourCount > 3))
				{
					killTiles.AddLast(tile);
				}
				else if (!tile.IsAlive() && (livingNeighbourCount == 3))
				{
					//spring to life
					wakeTiles.AddLast(tile);
				}
			}
		}

		//apply live, dead states
		foreach (var tile in wakeTiles)
		{
			tile.SetAlive(true);
		}

		foreach (var tile in killTiles)
		{
			tile.SetAlive(false);
		}
	}
}
