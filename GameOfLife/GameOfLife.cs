class GameOfLife
{
    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();
    private static IntPtr ConsoleWindow = GetConsoleWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    private const int MAXIMIZE = 3;

    private Stopwatch genTimer = new Stopwatch();
    private Random random = new Random();
    private StringBuilder printBuffer = new StringBuilder();

    private int[,] grid;
    private int[,] originalGrid;

    private int width, height;

    private int generationCount;

    private int aliveCellCount = 0;

    public GameOfLife(int width, int height, string path = null)
    {
        this.width = width;
        this.height = height;
        grid = new int[height, width];
        originalGrid = new int[height, width];

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

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 'O')
                {
                    grid[lineIndex, i] = 1;
                }
                else if (line[i] == '.')
                {
                    grid[lineIndex, i] = 0;
                }
            }

            lineIndex++;
        }
    }

    // Count the number of alive neighbors around a specific cell in the grid
    private int GetAliveNeighbors(int[,] grid, int i, int j)
    {
        int aliveNeighbors = 0;

        for (int ni = i - 1; ni <= i + 1; ni++)
        {
            for (int nj = j - 1; nj <= j + 1; nj++)
            {
                if (ni >= 0 && ni < height && nj >= 0 && nj < width && (ni != i || nj != j) && grid[ni, nj] == 1)
                {
                    aliveNeighbors++;
                }
            }
        }

        return aliveNeighbors;
    }

    // Visualize the current generation's grid on the console
    private void VisualizeGeneration()
    {
        printBuffer.Clear();
        printBuffer.AppendLine($"Conway's Game of Life (C#, Console) | Generation: {generationCount} | Grid cells: {grid.Length} - Live cells: {aliveCellCount} | Time took to generate: {genTimer.ElapsedMilliseconds} ms          \n");

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
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

        Parallel.For(0, height, i =>
        {
            for (int j = 0; j < width; j++)
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
        ConsoleHelper.SetCurrentFont("Consolas", 2);
        ShowWindow(ConsoleWindow, MAXIMIZE);
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
