using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public LineRenderer gridRenderer;
    public Material gridMaterial;
    public float gridSize = 32.0f;
    void Start()
    {
        DrawGrid();
    }


    void DrawGrid()
    {
        gridRenderer.positionCount = (gridWidth + gridHeight) * 2;

        // Draw horizontal lines
        for (int y = 0; y <= gridHeight; y++)
        {
            gridRenderer.SetPosition(y * 2, new Vector3(0, y * gridSize, 0));
            gridRenderer.SetPosition(y * 2 + 1, new Vector3(gridWidth * gridSize, y * gridSize, 0));
        }

        // Draw vertical lines
        for (int x = 0; x <= gridWidth; x++)
        {
            gridRenderer.SetPosition((gridHeight + x) * 2, new Vector3(x * gridSize, 0, 0));
            gridRenderer.SetPosition((gridHeight + x) * 2 + 1, new Vector3(x * gridSize, gridHeight * gridSize, 0));
        }

        gridRenderer.material = gridMaterial;
        gridRenderer.startWidth = gridRenderer.endWidth = 0.1f; // Adjust the thickness of the lines

        // Adjust material properties for a Tetris-like appearance
        gridMaterial.mainTextureScale = new Vector2(gridWidth, gridHeight);
        gridMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }
}