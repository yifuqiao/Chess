using UnityEngine;
using System.Collections;

/// <summary>
/// Audio manager.
/// </summary>
public class AudioManager : MonoBehaviour 
{
	public AudioSource[] _audioPool;
	
	public AudioClip[] clips;
	
	private static AudioManager _instance;
	public static AudioManager Instance
	{
		get
		{
			return _instance;
		}
	}
	
	private void Awake()
	{
		_instance = this;
	}
	
	public void play(string name)
	{
		foreach(AudioClip clip in clips)
		{
			if(clip.name == name)
			{
				foreach(AudioSource item in _audioPool)
				{
					if(item != null)
					{
						if(item.isPlaying == false)
						{
							item.clip = clip;
							item.Play();
							return;
						}
					}
				}
			}
		}
	}
}
