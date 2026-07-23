using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace Assets.Scripts
{
	public class TilemapManager : MonoBehaviour
	{
		Tilemap[] _Tilemaps = new Tilemap[2];

		List<Vector2> WallTiles;
		List<Vector2> MashineTiles;
		List<Vector2> OutputTiles;

		// Use this for initialization
		void Start()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				_Tilemaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
			}

			for (int i = -_Tilemaps[0].size.x; i < _Tilemaps[0].size.x; i++)
			{
				for (int j = -_Tilemaps[0].size.y; j < _Tilemaps[0].size.y; j++)
				{
					if (_Tilemaps[0].GetTile(new Vector3Int(i, j, 0)) != null)
						FilterTile(_Tilemaps[0].GetTile(new Vector3Int(i, j, 0)).name, new Vector2(i, j));
				}
			}
			for (int i = -_Tilemaps[1].size.x; i < _Tilemaps[1].size.x; i++)
			{
				for (int j = -_Tilemaps[1].size.y; j < _Tilemaps[1].size.y; j++)
				{
					if (_Tilemaps[1].GetTile(new Vector3Int(i, j, 0)) != null)
						FilterTile(_Tilemaps[1].GetTile(new Vector3Int(i, j, 0)).name, new Vector2(i, j));
				}
			}
		}

		void FilterTile(string name, Vector2 pos) 
		{
			if (name == "WallSprite") 
			{
				WallTiles.Add(pos);
			}
			else if (name == "MashineSprite") 
			{
				MashineTiles.Add(pos);
			}
			else if (name == "DeliveryConveyorSprite") 
			{
				OutputTiles.Add(pos);
			}
		}

		// Update is called once per frame
		void Update()
		{
			
		}
	}
}