using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGrid
{
	public enum State : int
	{
		EMPTY = 0,
		WHITE = 1,
		BLACK = 2
	}
	public int _xIndex = 0;
	public int _zIndex = 0;
	public State _occupied = State.EMPTY;
	/// <summary>
	/// The _position : pivot is left down
	/// </summary>
	public Vector3 _position = Vector3.zero;
}

/// <summary>
/// Grid generator.
/// </summary>
public class Grid : MonoBehaviour 
{
	public 	float 		_unitGridLength 		= 1f;
	public 	int 		_numberOfGrids 			= 10;
	public  List<UIMesh>	_uiMesh			= new List<UIMesh>();
	public 	GameObject 		_prefab;
	
	private static Grid 		_instance;
	private UnitGrid[,]			_gridArray		= null;
	public static Grid Instance
	{
		get
		{
			return _instance;
		}
	}
	
	public 	UnitGrid[,]	GridArray
	{
		get
		{
			return _gridArray;
		}
	}
	
	private void Awake()
	{
		_instance = this;
		Transform myTransform = transform;
		_gridArray = new UnitGrid[_numberOfGrids, _numberOfGrids];
		for(int i = 0; i < _numberOfGrids; ++i)
		{
			for(int j = 0; j < _numberOfGrids; ++j)
			{
				_gridArray[i,j] = new UnitGrid();
				_gridArray[i,j]._occupied = UnitGrid.State.EMPTY;
				_gridArray[i,j]._xIndex = i;
				_gridArray[i,j]._zIndex = j;
				_gridArray[i,j]._position.x = myTransform.position.x + i * _unitGridLength;
				_gridArray[i,j]._position.z = myTransform.position.z + j * _unitGridLength;
			}
		}
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Transform myTransform = transform;
		Vector3 orginPos = myTransform.position;
		orginPos.y += 0.01f;
		
		for(int i = 0; i <= _numberOfGrids ; ++i )
		{
			// draw horizontal first
			Vector3 startPos = orginPos + myTransform.forward * _unitGridLength * i;
			Vector3 endPos   = orginPos + myTransform.forward * _unitGridLength * i + myTransform.right * _unitGridLength * _numberOfGrids;
			Gizmos.DrawLine(startPos, endPos);
			
			// then draw vertical
			startPos = orginPos + myTransform.right * _unitGridLength * i;
			endPos   = orginPos + myTransform.right * _unitGridLength * i + myTransform.forward * _unitGridLength * _numberOfGrids;
			
			Gizmos.DrawLine(startPos, endPos);
		}
		
		Gizmos.color = Color.white;
		if(_gridArray != null)
		{
			for(int i = 0 ;i < _numberOfGrids ; ++i)
			{
				for(int j = 0; j < _numberOfGrids ; ++j)
				{
					if(_gridArray[i,j]._occupied != UnitGrid.State.EMPTY)
					{
						Gizmos.DrawWireCube(_gridArray[i,j]._position + Vector3.one * 0.5f * _unitGridLength, Vector3.one * _unitGridLength);
					}
				}
			}
		}
	}
	
	public void Snap( int widthX, int lengthZ, Placeable placeable)
	{
		// get the left down pivot
		Vector3 leftDownPosition = placeable.transform.position - (new Vector3(widthX,0f, lengthZ) ) * 0.5f * _unitGridLength;
		
		int xIndex = 0;
		int zIndex = 0;
		Vector3 myPosition = transform.position;
		
		float min = float.MaxValue;
		
		// snap X first 
		for(int i = 0; i < _numberOfGrids; ++i)
		{
			float minDistanceX = Mathf.Abs(myPosition.x + _unitGridLength * i - leftDownPosition.x);
			
			if(minDistanceX <= min)
			{
				min = minDistanceX;
				xIndex = i;
			}
		}
		
		min = float.MaxValue;
		// then snap z
		for(int i = 0; i < _numberOfGrids; ++i)
		{
			float minDistanceZ = Mathf.Abs(myPosition.z + _unitGridLength * i - leftDownPosition.z);
			
			if(minDistanceZ <= min)
			{
				min = minDistanceZ;
				zIndex = i;
			}
		}
		
		// handling collision
		// this is left down snap pos, which is not exactlly we want until we restore it back to center pivot
		Vector3 snapPostion = myPosition;
		
		// make sure they are in the boundry
		int overSizeX = xIndex + widthX - _numberOfGrids;
		int overSizeZ = zIndex+ lengthZ - _numberOfGrids;
		
		if(overSizeX > 0)
		{
			xIndex  -= overSizeX;
		}
		if(overSizeZ > 0)
		{
			zIndex  -= overSizeZ;
		}
		
		snapPostion.x += xIndex * _unitGridLength;
		snapPostion.z += zIndex * _unitGridLength;
		
		// this is restore pivot back to center
		placeable.transform.position = snapPostion + (new Vector3(widthX,0f, lengthZ) ) * 0.5f * _unitGridLength + Vector3.up * 0.05f;
		
		placeable._xIndex = xIndex;
		placeable._zIndex = zIndex;
	}
	
	public void RecalculateGrids()
	{		
		if(_gridArray != null)
		{
			for(int i = 0 ;i < _numberOfGrids ; ++i)
			{
				for(int j = 0; j < _numberOfGrids ; ++j)
				{
					_gridArray[i,j]._occupied = UnitGrid.State.EMPTY;
				}
			}
		}
		
		foreach(var item in PlaceableManager.Instance._placeables)
		{
			for(int i = 0; i < item._xSize; ++i)
			{ 
				
				for(int j = 0; j < item._zSize; ++j)
				{
					int influenceXIndex = item._xIndex + i;
					int influenceZIndex = item._zIndex + j;
					influenceXIndex = Mathf.Clamp(influenceXIndex, 0, _numberOfGrids-1);
					influenceZIndex = Mathf.Clamp(influenceZIndex, 0, _numberOfGrids-1);
					_gridArray[influenceXIndex, influenceZIndex]._occupied = item._color;
				}
			}
		}
	}
	
	public static bool CollideAll(Placeable placable, System.Action onCollidWithSameColor = null, System.Action<Placeable> onCollideWithDifferentColor = null)
	{
		if(PlaceableManager.Instance._placeables.Count > 0)
		{
			foreach(var item in PlaceableManager.Instance._placeables)
			{
				if(item == placable)
				{
					continue;
				}
				// do horizontal first
				if ( 
					placable._xIndex == item._xIndex
					|| ((item._xIndex < placable._xIndex) && (item._xIndex + item._xSize > placable._xIndex))
					|| ((placable._xIndex < item._xIndex) && (placable._xIndex + placable._xSize > item._xIndex)	)
					
					)
				{
					// then do vertical
					if ( 
						placable._zIndex == item._zIndex
						|| ((item._zIndex < placable._zIndex) && (item._zIndex + item._zSize > placable._zIndex)) 
						|| ((placable._zIndex < item._zIndex) && (placable._zIndex + placable._zSize > item._zIndex))
						)
					{
						if(item._color != placable._color)
						{
							Debug.Log(" not same color");
							if(onCollideWithDifferentColor != null)
							{
								onCollideWithDifferentColor(item);
							}
						}
						else 
						{
							Debug.Log("same color");
							if(onCollidWithSameColor != null)
							{
								onCollidWithSameColor();
							}
						}
						return true;
					}
				}
			}
		}
		return false;
	}
	
	public void CreateUIMesh(int xIndex, int zIndex)
	{
		// todo : need to pool these object
		if (xIndex < _numberOfGrids && zIndex < _numberOfGrids && xIndex >= 0 && zIndex >= 0)
		{
			Vector3 pos = new Vector3( transform.position.x+ xIndex * _unitGridLength + 0.5f * _unitGridLength, transform.position.y + 0.2f , transform.position.z + zIndex * _unitGridLength+ 0.5f * _unitGridLength);
			Instantiate(_prefab, pos, Quaternion.identity);
		}
	}
	
	public void DeleteAllUIMesh()
	{
		// todo : need to pool these object
		for(int i = 0; i < _uiMesh.Count; ++i)
		{
			Destroy( _uiMesh[i].gameObject);
		}
		_uiMesh.Clear();
	}
	
}
