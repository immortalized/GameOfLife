class GameOfLife
{
    private Random random = new Random();
    private int[,] grid;
    private int[,] originalGrid;
    private int width, height;
    private int generationCount;
    private string printBuffer;
    private int aliveCellCount = 0;

    public GameOfLife(int width, int height, string path = null)
    {
        this.width = width;
        this.height = height;
        grid = new int[height, width];
        originalGrid = new int[height, width];

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

    private void InitializeRandomState()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = random.Next(100) < 15 ? 1 : 0;
            }
        }
    }

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

    private void ClearGrid()
    {
        for (int heightC = height; heightC > 0; heightC--)
        {
            Console.SetCursorPosition(0, 0 + heightC);
            Console.Write(new string(' ', width));
        }
        Console.SetCursorPosition(0, 0);
    }

    private int GetAliveNeighbors(int[,] grid, int i, int j)
    {
        int neighbours = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int ni = i + x;
                int nj = j + y;

                if (ni >= 0 && ni < height && nj >= 0 && nj < width && grid[ni, nj] == 1)
                {
                    neighbours++;
                }
            }
        }

        return neighbours;
    }

    private void VisualizeGeneration()
    {
        printBuffer = $"Conway's Game of Life (C#) | Generation: {generationCount} | Cells alive: {aliveCellCount}\n\n";

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                printBuffer += (grid[i, j] == 1) ? "█" : " ";
            }
            printBuffer += "\n";
        }

        ClearGrid();
        Console.WriteLine(printBuffer);
    }

    private void UpdateGeneration()
    {
        generationCount++;
        aliveCellCount = 0;

        originalGrid = (int[,])grid.Clone();

        for (int i = 0; i < grid.GetLength(0); i++)
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

                aliveCellCount += grid[i, j];
            }
        }
    }

    public void Simulate(int delay, bool manualStep)
    {
        while (true)
        {
            VisualizeGeneration();
            UpdateGeneration();
            Thread.Sleep(delay);

            if (manualStep)
            {
                Console.ReadKey(true);
            }
        }
    }
}
