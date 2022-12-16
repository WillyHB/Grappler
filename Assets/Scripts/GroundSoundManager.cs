using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine;

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
        TileBase tile = GetGroundTileType();

        foreach (var tileSound in Sounds)
        {
            if (tileSound.tile == tile)
            {
                return tileSound;
            }
        }

        return null;
    }

    public TileBase GetGroundTileType()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
        new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().size.y / 2 + GetComponent<BoxCollider2D>().offset.y),
        PFSM.GroundedCheckRadius, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag(PFSM.GroundTag))
            {      
                var map = hit.collider.GetComponent<Tilemap>();
                var grid = map.layoutGrid;

                Vector3 gridPosition = grid.transform.InverseTransformPoint(hit.point);
                Vector3Int cell = grid.LocalToCell(gridPosition);

                var tile = map.GetTile(cell);

                return tile;
            }
        }

        return null;
    }
}
