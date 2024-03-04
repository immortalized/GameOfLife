using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

class GameOfLife
{
    private Stopwatch genTimer = new Stopwatch();

    private Random random = new Random();

    private StringBuilder printBuffer = new StringBuilder();

    private int[,] grid;
    private int[,] originalGrid;

    private short cellSize;

    private int width, height;

    private int generationCount;

    private int aliveCellCount = 0;

    public GameOfLife(int width, int height, short cellSize, string path = null)
    {
        this.width = width;
        this.height = height;
        grid = new int[height, width];
        originalGrid = new int[height, width];
        this.cellSize = cellSize;

        // Initialize the grid either randomly or from a file
        if (path == null)
        {
            InitializeRandomState();
        }
        else
        {
            using (StreamReader sr = new StreamReader(path))
            {
                InitializeFromFile(sr);
            }
        }
    }

    // Initialize the grid with random live (1) and dead (0) cells
    private void InitializeRandomState()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = random.Next(100) < 15 ? 1 : 0; // Approximately 15% chance of a cell being alive
            }
        }
    }

    // Initialize the grid from a file specified by the StreamReader
    private void InitializeFromFile(StreamReader sr)
    {
        int lineIndex = 0;
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            for (int i = line.IndexOf('1'); i > -1; i = line.IndexOf('1', i + 1))
            {
                grid[lineIndex, i] = 1;
            }
            lineIndex++;
        }
    }

    // Count the number of alive neighbors around a specific cell in the grid
    private int GetAliveNeighbors(int[,] grid, int i, int j)
    {
        int neighbours = 0;
        int ni, nj;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                ni = i + x;
                nj = j + y;

                if (ni >= 0 && ni < height && nj >= 0 && nj < width && grid[ni, nj] == 1)
                {
                    neighbours++;
                }
            }
        }

        return neighbours;
    }

    // Visualize the current generation's grid on the console
    private void VisualizeGeneration()
    {
        printBuffer.Clear();
        printBuffer.AppendLine($"Conway's Game of Life (C#, Console) | Generation: {generationCount} | Grid cells: {grid.Length} - Live cells: {aliveCellCount} | Time took to generate: {genTimer.ElapsedMilliseconds} ms          \n");

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                printBuffer.Append((grid[i, j] == 1) ? "█" : " ");
            }
            printBuffer.AppendLine();
        }

        Console.SetCursorPosition(0, 0);
        FastConsole.Write(printBuffer.ToString());
        FastConsole.Flush();
    }

    // Update the current generation based on the rules of Conway's Game of Life
    private void UpdateGeneration()
    {

        Console.CursorVisible = false; // To prevent annoying cursor flickering

        genTimer.Reset();
        genTimer.Start();

        generationCount++;
        aliveCellCount = 0;

        originalGrid = (int[,])grid.Clone(); // Create a copy of the current grid for calculating the next generation

        Parallel.For(0, grid.GetLength(0), i =>
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                int neighbours = GetAliveNeighbors(originalGrid, i, j);

                if (originalGrid[i, j] == 1)
                {
                    grid[i, j] = (neighbours == 2 || neighbours == 3) ? 1 : 0;
                }
                else
                {
                    grid[i, j] = (neighbours == 3) ? 1 : 0;
                }

                Interlocked.Add(ref aliveCellCount, grid[i, j]); // Use Interlocked to safely update aliveCellCount
            }
        });

        genTimer.Stop();
    }

    // Simulate the Game of Life with a specified delay between generations and optional manual step
    public void Simulate(int delay, bool manualStep)
    {
        ConsoleHelper.SetCurrentFont("Consolas", cellSize);
        while (true)
        {
            VisualizeGeneration(); // Wait for the asynchronous operation to complete
            UpdateGeneration();
            Thread.Sleep(delay);

            if (manualStep)
            {
                Console.ReadKey(true);
            }
        }
    }
}
