using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSup/Mosh")]
public class WaveEntityGroupDescription : ScriptableObject
{
	[SerializeField]
	private List<Wave> _waves = new List<Wave>();
	
	private Path _path;

	public List<Wave> GetWaves => _waves;

	public Path Path => _path;
}
