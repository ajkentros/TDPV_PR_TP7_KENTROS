using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ControladorObstaculos : MonoBehaviour
{
    [SerializeField] private Tilemap obstaculosTilemap;   // Tilemap de los obstáculos
    [SerializeField] private Tile obstaculo1;             // Tile de tipo Obstaculo_1
    [SerializeField] private Tile obstaculo2;             // Tile de tipo Obstaculo_2
    [SerializeField] private Tile obstaculo3;             // Tile de tipo Obstaculo_3

    private readonly int cantidadGenerarObstaculo1 = 1;     // Referencia a la variable que establece la cantidad de Obstáculos1 que se generarán al colisionar
    private readonly int cantidadGenerarObstaculo2 = 1;     // Referencia a la variable que establece la cantidad de Obstáculos2 que se generarán al colisionar
    private readonly int cantidadGenerarObstaculo3 = 1;     // Referencia a la variable que establece la cantidad de Obstáculos3 que se generarán al colisionar

    private int tileAGenerar;       // Referencia a la variable que guarda la cantidad de tiles a generar


    private readonly float reboteFactorObstaculo1 = 1.5f;       // Referencia a la variable de establece el factor de rebote que aporta el Obstáculo1
    private readonly float reboteFactorObstaculo2 = 3.0f;       // Referencia a la variable de establece el factor de rebote que aporta el Obstáculo2
    private readonly float reboteFactorObstaculo3 = 4.0f;       // Referencia a la variable de establece el factor de rebote que aporta el Obstáculo3
    private float reboteFactor;

    // gestiona la colisión del tipo de obstáculo para generar nuevos
    public void HandleTileCollision(Collision2D collision)
    {
        // Verifica si la colisión es con el player
        if (!collision.gameObject.CompareTag("Obstaculo"))
        {
            return;
        }

        // Obtiene la posición del tile en el Tilemap
        Vector3 hitPosition = Vector3.zero;

        // Por cada punto de contacto del obstaculoq que colisionó => obtiene la posición del obstáculo
        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
            Vector3Int tilePosition = obstaculosTilemap.WorldToCell(hitPosition);

            // Obtiene el tipo de tile
            TileBase hitTile = obstaculosTilemap.GetTile(tilePosition);

            // Si el tile colisionado no es nulo => genera nuevos tiles según el tipo
            if (hitTile != null)
            {
                if (hitTile == obstaculo1)
                {
                    tileAGenerar = cantidadGenerarObstaculo1;
                    reboteFactor = reboteFactorObstaculo1;
                    // Reproduce efecto de sonido al colisionar con el Obstaculo_01
                    AudioManager.Instance.PlayEfectoSonoro(0);
                }
                else if (hitTile == obstaculo2)
                {
                    tileAGenerar = cantidadGenerarObstaculo2;
                    reboteFactor = reboteFactorObstaculo2;
                    // Reproduce efecto de sonido al colisionar con el Obstaculo_01
                    AudioManager.Instance.PlayEfectoSonoro(1);
                }
                else if (hitTile == obstaculo3)
                {
                    tileAGenerar = cantidadGenerarObstaculo3;
                    reboteFactor = reboteFactorObstaculo3;
                    // Reproduce efecto de sonido al colisionar con el Obstaculo_01
                    AudioManager.Instance.PlayEfectoSonoro(2);
                }

                // Llama al método que genera nuevos tiles
                GenerateNewTiles(hitTile, tileAGenerar);

                break; 
            }
        }
    }

    // Gestiona la generación de nuevos tiles
    void GenerateNewTiles(TileBase hitTile, int numberOfTiles)
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            Vector3Int newTilePosition = FindRandomEmptyTilePosition();

            if (newTilePosition != new Vector3Int(0, 0, 0)) 
            {
                if (hitTile == obstaculo1)
                {
                    obstaculosTilemap.SetTile(newTilePosition, obstaculo1);
                }
                else if (hitTile == obstaculo2)
                {
                    obstaculosTilemap.SetTile(newTilePosition, obstaculo2);
                }
                else if (hitTile == obstaculo3)
                {
                    obstaculosTilemap.SetTile(newTilePosition, obstaculo3);
                }
            }
        }
    }

    // Gestiona una posición aleatoria del nuevo tile a generar que no se superponga con el colisionado
    Vector3Int FindRandomEmptyTilePosition()
    {
        // Encuentra todas las posiciones no ocupadas en el Tilemap
        BoundsInt bounds = obstaculosTilemap.cellBounds;

        List<Vector3Int> emptyPositions = new();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new(x, y, 0);

                if (obstaculosTilemap.GetTile(tilePosition) == null)
                {
                    emptyPositions.Add(tilePosition);
                }
            }
        }

        // Si se encuentran posiciones vacías, devolver una posición aleatoria
        if (emptyPositions.Count > 0)
        {
            return emptyPositions[Random.Range(0, emptyPositions.Count)];
        }

        // Si no se encuentra ninguna posición vacía, devolver una posición predeterminada
        return new Vector3Int(0, 0, 0);
    }

    public float ObtieneRebotFactor()
    {
        return reboteFactor;
    }
}
