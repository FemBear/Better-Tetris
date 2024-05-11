using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    public int totalScore = 0;
    public int scorePerLine = 100;

    AudioSource audioSource;

    public AudioClip rowDeleteSound;

    public TextMeshProUGUI hud_score;

    private GameObject previewTetromino;
    private TetrominoData previewPiece;
    private TetrominoData nextTetrominoData;

    private Vector2 prevPosition;

    private bool hasBeenHeld = false;
    private TetrominoData heldPiece;
    private GameObject heldPieceGameObject;


    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        UpdateUI();
        GenerateNextPiece();
        SpawnPiece();
    }

    #region Piece/Board Interaction
    public void SpawnPiece()
    {
        SetNextPiece(); // Set next piece for preview
        PreviewPiece();
        activePiece.Initialize(this, spawnPosition, nextTetrominoData);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece); // Place the active piece on the board
        }
        else
        {
            GameOver(); // Game over if the active piece cannot be placed
        }
    }

    public void GenerateNextPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        previewPiece = tetrominoes[random];
    }

    private void SetNextPiece()
    {
        nextTetrominoData = previewPiece;
        GenerateNextPiece();
    }


    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        if (tilemap == null)
        {
            return;
        }

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void AddScore(int linesCleared)
    {
        totalScore += linesCleared * scorePerLine;
        UpdateUI();
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;
        int linesCleared = 0;

        // Clear all tiles in the row and count the cleared lines
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        linesCleared++;

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }

        // Add score based on the number of lines cleared
        if (linesCleared > 0)
        {
            AddScore(linesCleared);
            playLineClearSound();
        }
    }

    public void PreviewPiece()
    {
        if (previewTetromino != null)
        {
            //Clear the preview piece
            Clear(previewTetromino.GetComponent<Piece>());
            Destroy(previewTetromino);
        }
        
        //Create a new preview piece
        previewTetromino = new GameObject("Preview");
        previewTetromino.transform.position = prevPosition;
        Vector3Int previewSpawnPosition = new Vector3Int(10, 4, 0);

        //Add a piece component to the preview piece
        Piece previewPiece = previewTetromino.AddComponent<Piece>();
        previewPiece.Initialize(this, previewSpawnPosition, this.previewPiece);

        //Set the preview piece on the board
        Set(previewPiece);
        previewPiece.enabled = false;
    }
    //disable the held piece
    public void HoldPiece()
    {
        Debug.Log("Hold Piece");
    }


    #endregion

    #region GameState
    public void GameOver()
    {
        PlayerPrefs.SetInt("lastScore", totalScore);
        SceneManager.LoadScene("GameOver");
    }
    #endregion

    #region UI
    public void UpdateUI()
    {
        hud_score.text = totalScore.ToString();
    }

    #endregion

    #region Audio

    void playLineClearSound()
    {
        audioSource.PlayOneShot(rowDeleteSound);
    }

    #endregion
}
