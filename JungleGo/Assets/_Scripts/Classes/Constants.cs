/// <summary>
/// Contains constant definitions used throughout the game.
/// </summary>
public static class Constants
{
    // === Board Settings ===
    /// <summary>
    /// Defines the number of rows in the game board.
    /// </summary>
    public const int BoardRows = 10; // Row size

    /// <summary>
    /// Defines the number of columns in the game board.
    /// </summary>
    public const int BoardCols = 10; // Column size

    /// <summary>
    /// Represents the tile code used for empty tiles when initializing the board.
    /// </summary>
    public const int EmptyTileCode = 0; // Empty tile code for initializing the board

    /// <summary>
    /// Represents the tile code used for obstacle tiles when initializing the board.
    /// </summary>
    public const int ObstacleTileCode = 0; // Obstacle code for initializing the board

    // === Level Settings ===
    public const int MaxUniqueProducts = 12;
    public const int MaxCustomerDelaySec = 10; 
    public const int MinCustomerDelaySec = 1;
    public const int MinCustomers = 7;
    /// <summary>
    /// Number of customers added per difficulty level
    /// </summary>
    public const int CustomerCountDiffMod = 3;
    /// <summary>
    /// Maximum difficulty level, semi-arbitrary but used for customer
    /// list size and modifying delay between customers.
    /// </summary>
    public const int DifficultyCap = 5;

    // === General Purpose ===
    public const string[] CustomerNames = {
        "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hannah", "Isaac", "Jane",
        "Kate", "Liam", "Mia", "Noah", "Olivia", "Paul", "Quinn", "Rose", "Samuel", "Tara",
        "Uma", "Victor", "Wendy", "Xavier", "Yvonne", "Zachary", "Abigail", "Benjamin", "Catherine"
    };
}

/// <summary>
/// Represents the type of a tile on the game board.
/// </summary>
public enum TileType 
{
    /// <summary>
    /// Indicates that the tile is empty.
    /// </summary>
    Empty,

    /// <summary>
    /// Indicates that the tile contains an obstacle.
    /// </summary>
    Obstacle, 

    /// <summary>
    /// Indicates that the tile contains a customer.
    /// </summary>
    Customer,

    /// <summary>
    /// Indicates that the tile contains a product.
    /// </summary>
    Product
}

public enum Names
