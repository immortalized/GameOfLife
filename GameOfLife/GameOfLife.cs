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

    private int[] grid;
    private int[] originalGrid;

    private int width, height;

    private int generationCount;

    private int aliveCellCount = 0;

    public GameOfLife(int width, int height, string path = null)
    {
        this.width = width;
        this.height = height;
        grid = new int[width * height];
        originalGrid = new int[width * height];

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

    private void InitializeRandomState()
    {
        for (int i = 0; i < width * height; i++)
        {
            grid[i] = random.Next(100) < 15 ? 1 : 0; // Approximately 15% chance of a cell being alive
        }
    }

    private void InitializeFromFile(StreamReader sr)
    {
        int index = 0;
        int lineIndex = 0;

        while (!sr.EndOfStream && index < width * height)
        {
            string line = sr.ReadLine();

            for (int i = line.IndexOf('O'); i > -1 && index < width * height; i = line.IndexOf('O', i + 1))
            {
                grid[lineIndex * width + i] = 1;
                index++;
            }

            lineIndex++;
        }
    }

    private int GetAliveNeighbors(int[] grid, int index)
    {
        int aliveNeighbors = 0;
        int i = index / width;
        int j = index % width;

        int[] neighborOffsets = { -width - 1, -width, -width + 1, -1, 1, width - 1, width, width + 1 };

        foreach (int offset in neighborOffsets)
        {
            int neighborIndex = index + offset;

            if (neighborIndex >= 0 && neighborIndex < width * height && grid[neighborIndex] == 1)
            {
                aliveNeighbors++;
            }
        }

        return aliveNeighbors;
    }

    private void VisualizeGeneration()
    {
        printBuffer.Clear();
        printBuffer.AppendLine($"Conway's Game of Life (C#, Console) | Generation: {generationCount} | Grid cells: {grid.Length} - Live cells: {aliveCellCount} | Time took to generate: {genTimer.ElapsedMilliseconds} ms          \n");

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                printBuffer.Append((grid[i * width + j] == 1) ? "█" : " ");
            }
            printBuffer.AppendLine();
        }

        Console.SetCursorPosition(0, 0);
        FastConsole.Write(printBuffer.ToString());
        FastConsole.Flush();
    }

    private void UpdateGeneration()
    {
        Console.CursorVisible = false;
        genTimer.Reset();
        genTimer.Start();

        generationCount++;
        aliveCellCount = 0;

        originalGrid = (int[])grid.Clone();

        Parallel.For(0, width * height, index =>
        {
            int i = index / width;
            int j = index % width;

            int neighbours = GetAliveNeighbors(originalGrid, index);

            if (originalGrid[index] == 1)
            {
                grid[index] = (neighbours == 2 || neighbours == 3) ? 1 : 0;
            }
            else
            {
                grid[index] = (neighbours == 3) ? 1 : 0;
            }

            Interlocked.Add(ref aliveCellCount, grid[index]);
        });

        genTimer.Stop();
    }

    public void Simulate(int delay, bool manualStep)
    {
        ConsoleHelper.SetCurrentFont("Consolas", 2);
        ShowWindow(ConsoleWindow, MAXIMIZE);
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
