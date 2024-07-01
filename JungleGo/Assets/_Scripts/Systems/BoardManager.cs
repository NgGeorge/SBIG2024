using UnityEngine;

/// <summary>
/// Manages the game board and provides methods for manipulating and interacting with it.
/// </summary>
public class BoardManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of BoardManager, allowing it to be accessed globally.
    /// </summary>
    public static BoardManager Instance { get; private set; }

    /// <summary>
    /// 2D array to hold the game board tiles.
    /// </summary>
    public Tile[,] Board;

    /// <summary>
    /// Number of rows in the game board.
    /// </summary>
    public readonly int Rows = Constants.BoardRows;

    /// <summary>
    /// Number of columns in the game board.
    /// </summary>
    public readonly int Cols = Constants.BoardCols;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Check if an instance of BoardManager already exists
        if (Instance == null)
        {
            // If not, set this instance as the singleton instance
            Instance = this;

            // Ensure this instance persists across scenes
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // If another instance exists and it's not this one, destroy this instance
            Destroy(gameObject);
        }

        // Initialize the game board with the specified size
        Board = new Tile[Rows, Cols];
        InitializeCleanBoard();
    }

    /// <summary>
    /// Creates and returns an empty tile.
    /// </summary>
    /// <returns>An empty tile object.</returns>
    public static Tile CreateEmptyTile() => new Tile(TileType.Empty, Constants.EmptyTileCode);

    /// <summary>
    /// Initializes the game board with default empty tiles.
    /// </summary>
    private void InitializeCleanBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Board[i, j] = CreateEmptyTile();
            }
        }
    }

    /// <summary>
    /// Checks if the given coordinates (x, y) are within the bounds of the game board.
    /// </summary>
    /// <param name="x">The x-coordinate to check.</param>
    /// <param name="y">The y-coordinate to check.</param>
    /// <returns>
    /// <c>true</c> if the coordinates (x, y) are within the board bounds; otherwise, <c>false</c>.
    /// </returns>
    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Rows && y >= 0 && y < Cols;
    }

    /// <summary>
    /// Places a product tile at the specified coordinates (x, y) on the game board.
    /// </summary>
    /// <param name="x">The x-coordinate to place the product.</param>
    /// <param name="y">The y-coordinate to place the product.</param>
    /// <param name="id">The ID of the product to place.</param>
    /// <returns><c>true</c> if the product was successfully placed; otherwise, <c>false</c>.</returns>
    public bool PlaceProduct(int x, int y, int id)
    {
        // Check if the coordinates are within bounds
        if (IsInBounds(x, y))
        {
            // Place the product tile at the specified coordinates
            Board[x, y] = new Tile(TileType.Product, id);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Places an obstacle tile at the specified coordinates (x, y) on the game board.
    /// </summary>
    /// <param name="x">The x-coordinate to place the obstacle.</param>
    /// <param name="y">The y-coordinate to place the obstacle.</param>
    /// <returns><c>true</c> if the obstacle was successfully placed; otherwise, <c>false</c>.</returns>
    public bool PlaceObstacle(int x, int y)
    {
        // Check if the coordinates are within bounds
        if (IsInBounds(x, y))
        {
            // Place the obstacle tile at the specified coordinates
            Board[x, y] = new Tile(TileType.Obstacle, Constants.ObstacleTileCode);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Places a customer tile at the specified coordinates (x, y) on the game board.
    /// </summary>
    /// <param name="x">The x-coordinate to place the customer.</param>
    /// <param name="y">The y-coordinate to place the customer.</param>
    /// <param name="id">The ID of the customer to place.</param>
    /// <returns><c>true</c> if the customer was successfully placed; otherwise, <c>false</c>.</returns>
    public bool PlaceCustomer(int x, int y, int id)
    {
        // Check if the coordinates are within bounds
        if (IsInBounds(x, y))
        {
            // Place the customer tile at the specified coordinates
            Board[x, y] = new Tile(TileType.Customer, id);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Prints the current state of the game board to the Unity console.
    /// </summary>
    public void PrintBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                if (Board[i, j].Type == TileType.Empty)
                {
                    Debug.Log("[   ]");
                }
                else if (Board[i, j].Type == TileType.Obstacle)
                {
                    Debug.Log("[XXX]");
                }
                else if (Board[i, j].Type == TileType.Product)
                {
                    Debug.Log($"[P-{Board[i, j].Id}]");
                }
                else if (Board[i, j].Type == TileType.Customer)
                {
                    Debug.Log($"[C-{Board[i, j].Id}]");
                }
            }
            Debug.Log("\n");
        }
    }

    /// <summary>
    /// Randomly places obstacles and products on the game board for testing purposes.
    /// </summary>
    /// <param name="numberOfObstacles">The number of obstacles to randomly place.</param>
    /// <param name="numberOfProducts">The number of products to randomly place.</param>
    /// <remarks>
    /// This method randomly places obstacles and products on the game board. It also moves
    /// customers to random positions on the board as part of the testing process.
    /// </remarks>
    public void RandomizeBoard(int numberOfObstacles, int numberOfProducts)
    {
        System.Random random = new System.Random();
        GameManager.Instance.Customers.ForEach(customer => {
            int x = random.Next(Rows);
            int y = random.Next(Cols);
            customer.Move(x, y);
        });
        
        int placedObstacles = 0;
        while (placedObstacles < numberOfObstacles)
        {
            int x = random.Next(Rows);
            int y = random.Next(Cols);
            if (Board[x, y].Type == TileType.Empty)
            {
                PlaceObstacle(x, y);
                placedObstacles++;
            }
        }

        int placedProducts = 0;
        while (placedProducts < numberOfProducts)
        {
            int x = random.Next(Rows);
            int y = random.Next(Cols);
            if (Board[x, y].Type == TileType.Empty)
            {
                PlaceProduct(x, y, ProductDatabase.Instance.GetRandomProduct().Id);
                placedProducts++;
            }
        }
    }

    /// <summary>
    /// Moves a customer tile from the old coordinates (oldX, oldY) to the new coordinates (newX, newY) on the game board.
    /// </summary>
    /// <param name="oldX">The current x-coordinate of the customer.</param>
    /// <param name="oldY">The current y-coordinate of the customer.</param>
    /// <param name="newX">The new x-coordinate to move the customer to.</param>
    /// <param name="newY">The new y-coordinate to move the customer to.</param>
    /// <param name="id">The ID of the customer to move.</param>
    public void MoveCustomerInBoard(int oldX, int oldY, int newX, int newY, int id)
    {
        Board[oldX, oldY] = CreateEmptyTile();
        PlaceCustomer(newX, newY, id);
    }
}
