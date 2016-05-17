using UnityEngine;
using System.Collections;

/// <summary>
/// User interface mesh.
/// </summary>
public class UIMesh : MonoBehaviour 
{
	private void Awake()
	{
		Grid.Instance._uiMesh.Add(this);
	}
}
