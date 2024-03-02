class GameOfLife
{
    private Random random = new Random();
    private int[,] lives;
    private int[,] newLives;
    private int width, height;
    private int generationCount;
    private string printBuffer;
    private int aliveCellCount = 0;

    public GameOfLife(int width, int height, string path = null)
    {
        this.width = width;
        this.height = height;
        lives = new int[height, width];
        newLives = new int[height, width];

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
                lives[i, j] = random.Next(100) < 15 ? 1 : 0;
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
                lives[lineIndex, i] = 1;
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

    private int GetAliveNeighbors(int i, int j)
    {
        int neighbours = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int ni = i + x;
                int nj = j + y;

                if (ni >= 0 && ni < height && nj >= 0 && nj < width && lives[ni, nj] == 1)
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

        for (int i = 0; i < lives.GetLength(0); i++)
        {
            for (int j = 0; j < lives.GetLength(1); j++)
            {
                printBuffer += (lives[i, j] == 1) ? "█" : " ";
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

        newLives = (int[,])lives.Clone();

        for (int i = 0; i < lives.GetLength(0); i++)
        {
            for (int j = 0; j < lives.GetLength(1); j++)
            {
                int neighbours = GetAliveNeighbors(i, j);

                if (lives[i, j] == 1)
                {
                    newLives[i, j] = (neighbours == 2 || neighbours == 3) ? 1 : 0;
                }
                else
                {
                    newLives[i, j] = (neighbours == 3) ? 1 : 0;
                }

                aliveCellCount += newLives[i, j];
            }
        }

        lives = newLives;
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
