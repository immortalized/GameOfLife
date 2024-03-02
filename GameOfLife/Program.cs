class GameOfLife
{
    private Random random = new Random();
    private int[,] Lives;
    private int width, height;
    private int generationCount;
    private string printBuffer;

    public GameOfLife(int width, int height, string path = null)
    {
        this.width = width;
        this.height = height;
        Lives = new int[height, width];

        if (path == null)
        {
            InitializeRandomState();
        }
        else
        {
            using (StreamReader sr = new StreamReader(path))
            {
                int lineIndex = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    for (int i = line.IndexOf('1'); i > -1; i = line.IndexOf('1', i + 1))
                    {
                        Lives[lineIndex, i] = 1;
                    }
                    lineIndex++;
                }
            }
        }
    }

    private void InitializeRandomState()
    {
        for (int i = 0; i < Lives.GetLength(0); i++)
        {
            for (int j = 0; j < Lives.GetLength(1); j++)
            {
                Lives[i, j] = random.Next(100) < 15 ? 1 : 0;
            }
        }
    }

    private void Clear()
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

                if (ni >= 0 && ni < height && nj >= 0 && nj < width && Lives[ni, nj] == 1)
                {
                    neighbours++;
                }
            }
        }

        return neighbours;
    }

    private void Visualize()
    {
        printBuffer = $"Conway's Game of Life (C#) | Generation: {generationCount}\n\n";

        for (int i = 0; i < Lives.GetLength(0); i++)
        {
            for (int j = 0; j < Lives.GetLength(1); j++)
            {
                printBuffer += (Lives[i, j] == 1) ? "█" : " ";
            }
            printBuffer += "\n";
        }

        Clear();
        Console.WriteLine(printBuffer);
    }

    private void Update()
    {
        generationCount++;

        int[,] newLives = new int[height, width];

        for (int i = 0; i < Lives.GetLength(0); i++)
        {
            for (int j = 0; j < Lives.GetLength(1); j++)
            {
                int neighbours = GetAliveNeighbors(i, j);

                if (Lives[i, j] == 1)
                {
                    newLives[i, j] = (neighbours == 2 || neighbours == 3) ? 1 : 0;
                }
                else
                {
                    newLives[i, j] = (neighbours == 3) ? 1 : 0;
                }
            }
        }

        Lives = newLives;
    }

    public void RunSimulation(int delay, bool manualStep)
    {
        while (true)
        {
            Visualize();
            Update();
            Thread.Sleep(delay);

            if (manualStep)
            {
                Console.ReadKey(true);
            }
        }
    }
}
