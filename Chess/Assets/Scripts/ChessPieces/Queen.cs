using UnityEngine;
using System.Collections;

/// <summary>
/// Queen.
/// </summary>
public class Queen : Placeable
{
	protected override bool CheckValidMove ()
	{	
		UnitGrid[,] gridArray = _grid.GridArray;
		//verticle move
		if( _zIndex == _originalZIndex)
		{
			int start = Mathf.Min(_xIndex, _originalXIndex) + 1;
	    	int loopTimes = Mathf.Max(_xIndex, _originalXIndex) ;
			for(int i = start; i < loopTimes; ++i)
			{
				if(gridArray[i, _zIndex]._occupied != UnitGrid.State.EMPTY)
				{
					return false;
				}
			}
			return true;
		}
		
		//horizontal move
		if( _xIndex == _originalXIndex)
		{
			int start = Mathf.Min(_zIndex, _originalZIndex) + 1;
	    	int loopTimes = Mathf.Max(_zIndex, _originalZIndex) ;
			for(int i = start; i < loopTimes; ++i)
			{
				if(gridArray[_xIndex,i]._occupied != UnitGrid.State.EMPTY)
				{
					return false;
				}
			}
			return true;
		}
		
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
				Debug.Log(vec2);
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
		bool goingLeftStopped = false;
		bool goingRightStopped = false;
		bool goingUpStopped = false;
		bool goingDownStopped = false;
		bool goingleftUpStopped = false;
		bool goingrightUpStopped = false;
		bool goingLeftDownStopped = false;
		bool goingRightDownStopped = false;
		
		for(int i = 1; i < 8; ++i)
		{
			if(goingLeftStopped == false)
			{
				int x = _xIndex - i;
				int z = _zIndex;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex - i, _zIndex);
						goingLeftStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingLeftStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex - i, _zIndex);
					}
				}
			}
			
			if(goingRightStopped == false)
			{
				int x = _xIndex + i;
				int z = _zIndex;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex + i, _zIndex);
						goingRightStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingRightStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex + i, _zIndex);
					}
				}
			}
			
			
			if(goingUpStopped == false)
			{
				int x = _xIndex;
				int z = _zIndex + i;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex , _zIndex + i);
						goingUpStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingUpStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex , _zIndex + i);
					}
				}
			}
			
			if(goingDownStopped == false)
			{
				int x = _xIndex;
				int z = _zIndex - i;
				if(x >= 0 && x < 8 && z >= 0 && z < 8)
				{
					if(_grid.GridArray[x, z]._occupied != _color && _grid.GridArray[x, z]._occupied != UnitGrid.State.EMPTY)
					{
						_grid.CreateUIMesh(_xIndex , _zIndex - i);
						goingDownStopped = true;
					}
					else if (_grid.GridArray[x, z]._occupied == _color)
					{
						goingDownStopped = true;
					}
					else 
					{
						_grid.CreateUIMesh(_xIndex , _zIndex - i);
					}
				}
			}
			
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
