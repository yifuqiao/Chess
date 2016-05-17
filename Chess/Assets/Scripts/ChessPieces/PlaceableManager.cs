using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Placeable manager.
/// </summary>
public class PlaceableManager : MonoBehaviour 
{
	[HideInInspector] public 		List<Placeable> 			_placeables = new List<Placeable>();
	
	private 	static 	PlaceableManager 	_instance = null;
	public 		static PlaceableManager 	Instance 
	{
		get
		{
			return _instance;
		}
	}
	
	private void Awake()
	{
		_instance = this;
		StartCoroutine(RandomAssignMarbles());
	}
	
	// we know we only have 6 marbles for thi task
	private IEnumerator RandomAssignMarbles()
	{
		while (_placeables.Count < 6)
		{
			yield return null;	
		}
		
		// shuffle,
		int totalPlaceables = _placeables.Count;
		List<Placeable> newList = new List<Placeable>();
		for(int i = 0; i < totalPlaceables; ++i)
		{
			int rand = Random.Range(0,_placeables.Count);
			newList.Add(_placeables[rand]);
			_placeables.RemoveAt(rand);
		}
		_placeables = newList;
		
		// assign them diagnatlly
		int x = Random.Range(0,3);
		for(int i = 0; i < totalPlaceables; ++i)
		{
			int index = x + i;
			_placeables[i]._xIndex = index;
			_placeables[i]._zIndex = Random.Range(0, 8);
			_placeables[i].AssignPositionWithIndex(_placeables[i]._xIndex, _placeables[i]._zIndex);
		}
		Grid.Instance.RecalculateGrids();
	}
	
	private void OnGUI()
	{
		if(GUI.Button(new Rect(0f,0f, 100f, 100f), "Ramdom\nPosition"))
		{
			StopAllCoroutines();
			StartCoroutine(RandomAssignMarbles());
		}
		
		if(GUI.Button(new Rect(0f,100f, 100f, 100f), "Art\nSource"))
		{
			Application.OpenURL("http://www.turbosquid.com/3d-models/chess-board-3d-max/300732");
		}
	}
}
