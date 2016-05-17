using UnityEngine;
using System.Collections;

public class Knight : Placeable
{
	protected override bool CheckValidMove ()
	{
		if(Mathf.Abs( _xIndex - _originalXIndex) == 1)
		{
			if(Mathf.Abs(_zIndex - _originalZIndex) == 2)
			{
				return true;
			}
		}
		else if(Mathf.Abs(_zIndex - _originalZIndex) == 1)
		{
			if(Mathf.Abs( _xIndex - _originalXIndex) == 2)
			{
				return true;
			}
		}
		return false;
	}
	
	protected override void ShowAllPossibleMoves ()
	{
		base.ShowAllPossibleMoves();
		
		int x = _xIndex - 1;
		int z = _zIndex - 2;
		CreateUIMesh( x,  z);
		
		x = _xIndex + 1;
		z = _zIndex + 2;
		CreateUIMesh( x,  z);
		
		x = _xIndex - 1;
		z = _zIndex + 2;
		CreateUIMesh( x,  z);
		
		x = _xIndex + 1;
		z = _zIndex - 2;
		CreateUIMesh( x,  z);
		
		x = _xIndex - 2;
		z = _zIndex - 1;
		CreateUIMesh( x,  z);
		
		x = _xIndex + 2;
		z = _zIndex + 1;
		CreateUIMesh( x,  z);
		
		x = _xIndex + 2;
		z = _zIndex - 1;
		CreateUIMesh( x,  z);
		
		x = _xIndex - 2;
		z = _zIndex + 1;
		CreateUIMesh( x,  z);
	}
	
	private void CreateUIMesh(int x, int z)
	{
		if(x >= 0 && x < 8 && z >= 0 && z < 8)
		{
			if (_grid.GridArray[x, z]._occupied != _color)
			{
				_grid.CreateUIMesh(x, z);
			}
		}
	}
}
