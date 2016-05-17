using UnityEngine;
using System.Collections;

/// <summary>
/// Placeable.
/// </summary>
public abstract class Placeable : MonoBehaviour 
{
	protected 	Grid 			_grid 				= null;
	protected 	Color 			_gizmoColor			= Color.green;
	protected 	Vector3 		_originalPos		= Vector3.zero;
	protected 	int 			_originalXIndex		= 0;
	protected	int				_originalZIndex 	= 0;
	
	public		UnitGrid.State	_color				= UnitGrid.State.WHITE;
	public		int   			_xSize				= 1;
	public		int   			_zSize				= 1;
	
	[HideInInspector] public 	int 			_xIndex	= 0;
	[HideInInspector] public 	int 			_zIndex = 0;
	
	protected void Start()
	{
		_grid = Grid.Instance;
		if(PlaceableManager.Instance._placeables.Contains(this) == false)
		{
			PlaceableManager.Instance._placeables.Add(this);
		}
	}
	
	protected void OnDestroy()
	{
		AudioManager.Instance.play("Delete");
		if(PlaceableManager.Instance._placeables.Contains(this) == true)
		{
			PlaceableManager.Instance._placeables.Remove(this);
		}
		
		if(_grid != null)
		{
			_grid.RecalculateGrids();
		}
	}
	
	protected void OnMouseDrag() 
	{
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),  out hit, Camera.main.far, 1 << LayerMask.NameToLayer("Plane")) == true)
		{
			Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction);
			Dragging(hit.point);
		}
    }
	
	protected void OnMouseDown()
	{
		_originalPos = transform.position;
		_originalXIndex = _xIndex;
		_originalZIndex = _zIndex;
		
		_grid.DeleteAllUIMesh();
	}
	
	protected void OnMouseEnter()
	{
		if(Input.GetMouseButton(0) == false)
		{
			ShowAllPossibleMoves();
		}
	}
	
	protected void OnMouseUpAsButton()
	{
		_grid.DeleteAllUIMesh();
	}
	
	protected void OnMouseUp()
	{
		Debug.Log("release");
		OnPlaced();
	}
	
	protected void OnDrawGizmosSelecteds()
	{
		Gizmos.color = _gizmoColor;
		Vector3 size = Vector3.one;
		size.x = _xSize;
		size.z = _zSize;
		Gizmos.DrawCube(transform.position,  size);
	}
	
	protected void Dragging(Vector3 position)
	{
		transform.position = position;
		Snapping();
	}
	
	protected void Snapping()
	{
		if(_grid == null)
		{
			return;
		}
		_grid.Snap(_xSize, _zSize, this);
	}

	protected void OnPlaced()
	{
		AudioManager.Instance.play("Release");
		// todo : check if my move is valid
		bool isMyMoveValid = CheckValidMove();
		
		if(isMyMoveValid == true)
		{
			// to do : check if i should take out my opponent
			bool collided = Grid.CollideAll( this, () => {OnRestore();}, (opponent)=>{ Destroy(opponent.gameObject); _grid.RecalculateGrids();} );
			if(collided == false)
			{
				_grid.RecalculateGrids();
			}
		}
		else 
		{
			OnRestore();
		}
	}
	
	protected void OnRestore()
	{
		_xIndex = _originalXIndex;
		_zIndex = _originalZIndex;
		transform.position = _originalPos;
		Snapping();
	}
	
	protected virtual bool CheckValidMove()
	{
		return false;
	}
	
	protected virtual void ShowAllPossibleMoves()
	{
		_grid.DeleteAllUIMesh();
	}
	
	public void AssignPositionWithIndex(int xIndex, int zIndex)
	{
		if (xIndex <  _grid._numberOfGrids && zIndex < _grid._numberOfGrids && xIndex >= 0 && zIndex >= 0)
		{
			Vector3 pos = new Vector3( _grid.transform.position.x+ xIndex *_grid._unitGridLength + 0.5f * _grid._unitGridLength, _grid.transform.position.y + 0.2f , _grid.transform.position.z + zIndex * _grid._unitGridLength + 0.5f * _grid._unitGridLength );
			transform.position = pos;
			Snapping();
		}
	}
}
