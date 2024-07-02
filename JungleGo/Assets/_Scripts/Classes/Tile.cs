using UnityEngine;

/// <summary>
/// Represents a tile on the game board.
/// </summary>
public struct Tile
{
    /// <summary>
    /// The type of the tile.
    /// </summary>
    public TileType Type;

    /// <summary>
    /// The unique identifier of the tile.
    /// </summary>
    public int Id;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tile"/> struct with the specified type and identifier.
    /// </summary>
    /// <param name="type">The type of the tile.</param>
    /// <param name="id">The unique identifier of the tile.</param>
    public Tile(TileType type, int id)
    {
        Type = type;
        Id = id;
    }
}
