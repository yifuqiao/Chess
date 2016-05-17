using UnityEngine;
using System.Collections;

/// <summary>
/// Bishop.
/// </summary>
public class Bishop : Placeable 
{
	protected override bool CheckValidMove ()
	{	
		UnitGrid[,] gridArray = _grid.GridArray;
		if(Mathf.Abs(_xIndex - _originalXIndex) == Mathf.Abs(_zIndex - _originalZIndex))
		{
			int size = Mathf.Abs(_xIndex - _originalXIndex)-1;
			Vector2[] checkingSet = new Vector2[size];
			if(_zIndex - _originalZIndex == _xIndex - _originalXIndex)
			{
				int currentXIndex = Mathf.Max(_xIndex, _originalXIndex);
				int currentZIndex = Mathf.Max(_zIndex, _originalZIndex);
				
				
				for(int i = 0; i < size; ++i)
				{
					currentXIndex -= 1;
					currentZIndex -= 1;
					checkingSet[i].x = currentXIndex;
					checkingSet[i].y = currentZIndex;
				}
			}
			
			if(_zIndex - _originalZIndex == -(_xIndex - _originalXIndex) )
			{
				int currentXIndex = Mathf.Max(_xIndex, _originalXIndex);
				int currentZIndex = Mathf.Min(_zIndex, _originalZIndex);
				
				for(int i = 0; i < size; ++i)
				{
					currentXIndex -= 1;
					currentZIndex += 1;
					checkingSet[i].x = currentXIndex;
					checkingSet[i].y = currentZIndex;
				}
			}
			
			foreach(Vector2 vec2 in checkingSet)
			{
				if(gridArray[(int)vec2.x, (int)vec2.y]._occupied != UnitGrid.State.EMPTY)
				{
					return false;
				}
			}
			return true;
		}
		//invalid move pattern
		return false;
	}
	
	protected override void ShowAllPossibleMoves ()
	{
		base.ShowAllPossibleMoves ();
	
		bool goingleftUpStopped = false;
		bool goingrightUpStopped = false;
		bool goingLeftDownStopped = false;
		bool goingRightDownStopped = false;
		
		for(int i = 1; i < 8; ++i)
		{
			if(goingleftUpStopped == false)
			{
				int x = _xIndex - i;
				int z = _zIndex + i;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex - i, _zIndex + i);
						goingleftUpStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingleftUpStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex - i, _zIndex + i);
					}
				}
			}
			
			if(goingrightUpStopped == false)
			{
				int x = _xIndex + i;
				int z = _zIndex + i;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex + i, _zIndex + i);
						goingrightUpStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingrightUpStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex + i, _zIndex + i);
					}
				}
			}
			
			if(goingLeftDownStopped == false)
			{
				int x = _xIndex - i;
				int z = _zIndex - i;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex - i, _zIndex - i);
						goingrightUpStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingLeftDownStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex - i, _zIndex - i);
					}
				}
			}
			
			if(goingRightDownStopped == false)
			{
				int x = _xIndex + i;
				int z = _zIndex - i;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex + i, _zIndex - i);
						goingRightDownStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingRightDownStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex + i, _zIndex - i);
					}
				}
			}
		}
	}
}
