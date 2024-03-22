/// <summary>
/// Represents Conway's Game of Life simulation.
/// </summary>
public class GameOfLife
{
    // Declaration of external methods from DLLs
    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();
    private static IntPtr ConsoleWindow = GetConsoleWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    // Constants
    private const int MAXIMIZE = 3;

    // Private fields
    private Stopwatch genTimer = new Stopwatch(); // Timer for measuring generation time
    private Random random = new Random(); // Random number generator
    private StringBuilder printBuffer = new StringBuilder(); // StringBuilder for building console output

    private int[] grid; // Represents the current state of the grid
    private int[] originalGrid; // Represents the original state of the grid

    private int width, height; // Width and height of the grid
    private int generationCount; // Tracks the number of generations processed
    private int aliveCellCount = 0; // Tracks the count of alive cells in the grid

    // Constructor
    /// <summary>
    /// Initializes a new instance of the GameOfLife class with the specified width, height, and optional path to a file for initial grid state.
    /// </summary>
    /// <param name="width">The width of the grid.</param>
    /// <param name="height">The height of the grid.</param>
    /// <param name="path">Optional. The path to a file containing the initial state of the grid.</param>
    public GameOfLife(int width, int height, string path = null)
    {
        this.width = width + 2; // Adding boundary cells
        this.height = height + 2; // Adding boundary cells
        grid = new int[this.width * this.height]; // Initialize grid with boundary cells
        originalGrid = new int[width * height]; // Initialize original grid without boundary cells

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

    // Private methods

    /// <summary>
    /// Initializes the grid with a random state.
    /// </summary>
    private void InitializeRandomState()
    {
        for (int i = 0; i < width * height; i++)
        {
            // Generate random state for grid cells with constraints to exclude boundary cells
            if (!(i < width || i > width * height - width || i % width == 0 || i % width == width - 1))
            {
                grid[i] = random.Next(100) < 15 ? 1 : 0; // Approximately 15% chance of a cell being alive
            }
            else
            {
                grid[i] = 0; // Set boundary cells as dead
            }
        }
    }

    /// <summary>
    /// Initializes the grid from a file.
    /// </summary>
    /// <param name="sr">The StreamReader object used to read from the file.</param>
    private void InitializeFromFile(StreamReader sr)
    {
        int lineIndex = 1;

        // Read lines from the file to initialize the grid state
        while (!sr.EndOfStream && lineIndex < height)
        {
            string line = sr.ReadLine();

            // Parse each line to identify alive cells and update the grid accordingly
            for (int i = line.IndexOf('O'); i > -1 && i < width * height; i = line.IndexOf('O', i + 1))
            {
                grid[lineIndex * width + i + 1] = 1; // Set cell as alive
            }
            lineIndex++;
        }
    }

    /// <summary>
    /// Retrieves the count of alive neighbors for a given cell in the grid.
    /// </summary>
    /// <param name="grid">The grid representing the current state.</param>
    /// <param name="index">The index of the cell for which to count the alive neighbors.</param>
    /// <returns>The count of alive neighbors for the specified cell.</returns>
    private int GetAliveNeighbors(int[] grid, int index)
    {
        int aliveNeighbors = 0;
        int i = index / width;
        int j = index % width;

        // Iterate over neighboring cells to count alive neighbors
        for (int ni = -1; ni <= 1; ni++)
        {
            for (int nj = -1; nj <= 1; nj++)
            {
                int neighborIndex = (i + ni) * width + (j + nj);

                // Check if the neighbor index is valid and not the current cell
                if (neighborIndex != index && neighborIndex >= 0 && neighborIndex < width * height && grid[neighborIndex] == 1)
                {
                    aliveNeighbors++;
                }
            }
        }

        return aliveNeighbors;
    }

    /// <summary>
    /// Visualizes the current generation of the grid in the console.
    /// </summary>
    private void VisualizeGeneration()
    {
        printBuffer.Clear();
        printBuffer.AppendLine($"Conway's Game of Life (C#, Console) | Generation: {generationCount} | Grid cells: {grid.Length} - Live cells: {aliveCellCount} | Time took to generate: {genTimer.ElapsedMilliseconds} ms          \n");

        // Generate visual representation of the grid
        for (int i = 1; i < height - 1; i++)
        {
            for (int j = 1; j < width - 1; j++)
            {
                printBuffer.Append((grid[i * width + j] == 1) ? "█" : " "); // Use '█' character for alive cells and space for dead cells
            }
            printBuffer.AppendLine();
        }

        // Display the grid in the console
        Console.SetCursorPosition(0, 0);
        FastConsole.Write(printBuffer.ToString()); // Output the generated buffer
        FastConsole.Flush(); // Flush the output
    }

    /// <summary>
    /// Updates the grid to the next generation based on Conway's Game of Life rules.
    /// </summary>
    private void UpdateGeneration()
    {
        Console.CursorVisible = false;
        genTimer.Reset();
        genTimer.Start();

        generationCount++; // Increment generation count
        aliveCellCount = 0; // Reset alive cell count

        originalGrid = (int[])grid.Clone(); // Clone the current grid state for updating

        // Parallelize the generation update process for better performance
        Parallel.For(0, width * height, index =>
        {
            // Apply Game of Life rules to each non-boundary cell
            if (!(index < width || index > width * height - width || index % width == 0 || index % width == width - 1))
            {
                int neighbours = GetAliveNeighbors(originalGrid, index); // Count alive neighbors

                // Apply rules to determine the
                // state of the cell in the next generation
                if (originalGrid[index] == 1)
                {
                    grid[index] = (neighbours == 2 || neighbours == 3) ? 1 : 0; // Cell survives if it has 2 or 3 neighbors, otherwise dies
                }
                else
                {
                    grid[index] = (neighbours == 3) ? 1 : 0; // Dead cell becomes alive if it has exactly 3 neighbors
                }

                Interlocked.Add(ref aliveCellCount, grid[index]); // Update alive cell count using atomic operation
            }
        });

        genTimer.Stop(); // Stop generation timer
    }

    // Public methods

    /// <summary>
    /// Simulates the Game of Life with the specified delay between generations and optional manual step mode.
    /// </summary>
    /// <param name="delay">The delay (in milliseconds) between generations.</param>
    /// <param name="manualStep">Optional. If true, simulation proceeds one step at a time with user input.</param>
    public void Simulate(int delay, bool manualStep)
    {
        ConsoleHelper.SetCurrentFont("Consolas", 2); // Set console font for better visualization
        ShowWindow(ConsoleWindow, MAXIMIZE); // Maximize the console window

        while (true)
        {
            VisualizeGeneration(); // Visualize the current generation
            UpdateGeneration(); // Update to the next generation
            Thread.Sleep(delay); // Delay between generations

            if (manualStep)
            {
                Console.ReadKey(true); // Wait for user input before proceeding to the next generation
            }
        }
    }
}
