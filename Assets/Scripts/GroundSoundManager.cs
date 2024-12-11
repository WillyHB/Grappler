using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundSoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct TileSounds
    {
        public TileBase tile;
        [Space(10)]
        public Audio[] Footsteps;
        public Audio Land;
    }

    public PlayerStateMachine PFSM;

    public TileSounds[] Sounds;

    public TileSounds? GetCurrentTileSounds()
    {
        var tile = GetGroundTileType();

        if (tile.Item1 == HitData.Hit) 
        {
            if (tile.Item2 != null) 
            {
                foreach (var tileSound in Sounds)
                {
                    if (tileSound.tile == tile.Item2)
                    {
                        return tileSound;
                    }
                }
            }

            else 
            {
                return Sounds[0];
            }

        }


        return null;
    }

    public enum HitData {

        Hit,
        None,
    }

    public Tuple<HitData,TileBase> GetGroundTileType()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
        new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().size.y / 2 + GetComponent<BoxCollider2D>().offset.y),
        PFSM.GroundedCheckRadius, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag(PFSM.GroundTag))
            {      
                if (!hit.collider.TryGetComponent(out Tilemap t)) 
                    return new Tuple<HitData, TileBase>(HitData.Hit, null);
                var map = t;
                var grid = map.layoutGrid;

                Vector3 gridPosition = grid.transform.InverseTransformPoint(hit.point);
                Vector3Int cell = grid.LocalToCell(gridPosition);

                var tile = map.GetTile(cell);

                return new Tuple<HitData, TileBase>(HitData.Hit,tile);
            }
        }

        return new Tuple<HitData, TileBase>(HitData.None, null);
    }
}
