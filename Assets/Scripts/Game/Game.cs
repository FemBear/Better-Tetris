using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    private GameObject priviewTetromino;
    private GameObject nextTetromino;

    private bool gameStarted = false;

    private Vector2 prevPosition = new Vector2(-6.5f, 16.0f);

    void Start()
    {
        SpawnNextTetromino();
    }

    public bool CheckIsAboveGrid(Tetromino tetromino)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            foreach (Transform mino in tetromino.transform)
            {
                Vector2 pos = Round(mino.position);
                if (pos.y > gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool isFullRowAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        FindObjectOfType<GameManager>().NumberOfRowsThisTurn++;
        return true;
    }

    public void DeleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            MoveRowDown(i);
        }
    }


    public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (isFullRowAt(y))
            {
                DeleteMinoAt(y);
                MoveAllRowsDown(y + 1);
                --y;
            }
        }
    }

    public void UpdateGrid(Tetromino tetromino)
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }
    public Transform GetTransformFromGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public void SpawnNextTetromino()
    {
        if (!gameStarted)
        {
            gameStarted = true;

            nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
            priviewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), prevPosition, Quaternion.identity);
            priviewTetromino.GetComponent<Tetromino>().enabled = false;
        }
        else
        {
            priviewTetromino.transform.localPosition = new Vector2(5.0f, 20.0f);
            priviewTetromino.GetComponent<Tetromino>().enabled = true;
            priviewTetromino = nextTetromino;
            priviewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), prevPosition, Quaternion.identity);
            priviewTetromino.GetComponent<Tetromino>().enabled = false;
        }

    }

    string GetRandomTetromino()
    {
        int RandomTetromino = Random.Range(1, 8);
        string RandomTetrominoName = "Prefabs/Tetromino_T";
        switch (RandomTetromino)
        {
            case 1:
                RandomTetrominoName = "Prefabs/Tetromino_T";
                break;
            case 2:
                RandomTetrominoName = "Prefabs/Tetromino_Long";
                break;
            case 3:
                RandomTetrominoName = "Prefabs/Tetromino_Square";
                break;
            case 4:
                RandomTetrominoName = "Prefabs/Tetromino_J";
                break;
            case 5:
                RandomTetrominoName = "Prefabs/Tetromino_L";
                break;
            case 6:
                RandomTetrominoName = "Prefabs/Tetromino_S";
                break;
            case 7:
                RandomTetrominoName = "Prefabs/Tetromino_Z";
                break;

        }
        return RandomTetrominoName;
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("lastScore", FindObjectOfType<GameManager>().currentScore);
        SceneManager.LoadScene("GameOver");
    }
    public bool CheckIsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }
}
