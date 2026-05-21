using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SnakeGame : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 20;
    public int gridHeight = 20;
    public float cellSize = 20f;

    [Header("UI References")]
    public RectTransform gameArea;      // Panel where the game is drawn
    public GameObject cellPrefab;       // Simple UI Image prefab for snake/food
    public TMP_Text scoreText;
    public TMP_Text statusText;         // "Press any key to start", "You Win!", "Game Over"
    public int scoreToWin = 10;

    [Header("Speed")]
    public float moveInterval = 0.2f;   // Seconds between each move

    private List<Vector2Int> snake = new List<Vector2Int>();
    private Vector2Int food;
    private Vector2Int direction = Vector2Int.right;
    private Vector2Int nextDirection = Vector2Int.right;
    private int score = 0;
    private bool isRunning = false;
    private bool taskCompleted = false;
    private int taskCompletedOnDay = -1; //-1 means not completed yet

    private Dictionary<Vector2Int, GameObject> cellObjects = new Dictionary<Vector2Int, GameObject>();

    public bool IsTaskCompleted()
    {
        return taskCompletedOnDay == DayManager.Instance.currentDay;
    }
    void OnEnable()
    {
        if (DayManager.Instance != null && taskCompletedOnDay == DayManager.Instance.currentDay)
        {
            // Already completed today, show message instead
            ClearBoard();
            scoreText.text = "Task Complete!";
            statusText.text = "You already completed today's task. Come back tomorrow!";
            return;
        }
        // Reset game when computer screen opens
        ResetGame();
    }

    void Update()
    {
        // Block input if already completed today
        if (DayManager.Instance != null && taskCompletedOnDay == DayManager.Instance.currentDay)
            return;

        if (!isRunning)
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                ResetGame();
                StartCoroutine(RunGame());
            }
            return;
        }

        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (direction != Vector2Int.down)
                nextDirection = Vector2Int.up;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            if (direction != Vector2Int.up)
                nextDirection = Vector2Int.down;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            if (direction != Vector2Int.right)
                nextDirection = Vector2Int.left;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            if (direction != Vector2Int.left)
                nextDirection = Vector2Int.right;
        }
    }

    IEnumerator RunGame()
    {
        isRunning = true;
        statusText.text = "";

        while (isRunning)
        {
            yield return new WaitForSecondsRealtime(moveInterval);
            MoveSnake();
        }
    }

    void MoveSnake()
    {
        direction = nextDirection;

        Vector2Int newHead = snake[0] + direction;

        // Wall collision
        if (newHead.x < 0 || newHead.x >= gridWidth || newHead.y < 0 || newHead.y >= gridHeight)
        {
            GameOver();
            return;
        }

        // Self collision
        if (snake.Contains(newHead))
        {
            GameOver();
            return;
        }

        snake.Insert(0, newHead);

        // Check food
        if (newHead == food)
        {
            score++;
            scoreText.text = "Score: " + score + " / " + scoreToWin;

            if (score >= scoreToWin)
            {
                Win();
                return;
            }

            SpawnFood();
        }
        else
        {
            // Remove tail
            Vector2Int tail = snake[snake.Count - 1];
            snake.RemoveAt(snake.Count - 1);
            RemoveCell(tail);
        }

        DrawSnake();
    }

    void SpawnFood()
    {
        // Find a free cell
        List<Vector2Int> freeCells = new List<Vector2Int>();
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!snake.Contains(pos))
                    freeCells.Add(pos);
            }

        if (freeCells.Count == 0) return;

        // Remove old food cell
        RemoveCell(food);

        food = freeCells[Random.Range(0, freeCells.Count)];
        DrawCell(food, Color.red);
    }

    void DrawSnake()
    {
        for (int i = 0; i < snake.Count; i++)
        {
            Color c = i == 0 ? Color.green : new Color(0.2f, 0.8f, 0.2f);
            DrawCell(snake[i], c);
        }
    }

    void DrawCell(Vector2Int pos, Color color)
    {
        if (cellObjects.ContainsKey(pos))
        {
            cellObjects[pos].GetComponent<Image>().color = color;
            return;
        }

        GameObject cell = Instantiate(cellPrefab, gameArea);
        RectTransform rt = cell.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(cellSize, cellSize);

        // Offset so grid is centered in GameArea
        float offsetX = -(gridWidth * cellSize) / 2f;
        float offsetY = -(gridHeight * cellSize) / 2f;
        rt.anchoredPosition = new Vector2(pos.x * cellSize + offsetX, pos.y * cellSize + offsetY);

        cell.GetComponent<Image>().color = color;
        cellObjects[pos] = cell;
    }

    void DrawBorder()
    {
        // Bottom and Top
        for (int x = -1; x <= gridWidth; x++)
        {
            DrawCell(new Vector2Int(x, -1), Color.black);
            DrawCell(new Vector2Int(x, gridHeight), Color.black);
        }
        // Left and Right
        for (int y = 0; y <= gridHeight; y++)
        {
            DrawCell(new Vector2Int(-1, y), Color.black);
            DrawCell(new Vector2Int(gridWidth, y), Color.black);
        }
    }

    void RemoveCell(Vector2Int pos)
    {
        if (cellObjects.ContainsKey(pos))
        {
            Destroy(cellObjects[pos]);
            cellObjects.Remove(pos);
        }
    }

    void GameOver()
    {
        isRunning = false;
        statusText.text = "Game Over! Press any key to retry.";
        ClearBoard();
        StopAllCoroutines(); //Stops here instead
    }

    void Win()
    {
        isRunning = false;
        taskCompleted = true;
        taskCompletedOnDay = DayManager.Instance != null ? DayManager.Instance.currentDay : -1;
        statusText.text = "Task Complete! Well done!";
        ClearBoard();
        Debug.Log("Daily task completed!");
        StopAllCoroutines();
    }

    void ClearBoard()
    {
        foreach (var cell in cellObjects.Values)
            Destroy(cell);
        cellObjects.Clear();
    }

    void ResetGame()
    {
        // Don't reset if already completed today
        if (DayManager.Instance != null && taskCompletedOnDay == DayManager.Instance.currentDay)
            return;
        isRunning = false;
        taskCompleted = false;
        score = 0;
        direction = Vector2Int.right;
        nextDirection = Vector2Int.right;

        ClearBoard();

        if (DayManager.Instance != null)
        {
            scoreToWin = DayManager.Instance.currentDay * 5;
            moveInterval = Mathf.Max(0.05f, 0.2f - (DayManager.Instance.currentDay * 0.02f));
        }
        snake.Clear();
        snake.Add(new Vector2Int(gridWidth / 2, gridHeight / 2));

        scoreText.text = "Score: 0 / " + scoreToWin;
        statusText.text = "Press any key to start";

        SpawnFood();
        DrawSnake();
        DrawBorder();
    }

    public void ResetTaskCompletion()
    {
        taskCompletedOnDay = -1;
        taskCompleted = false;
    }
}