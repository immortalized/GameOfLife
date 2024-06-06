# GameOfLife

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://www.microsoft.com/en-us/windows)
[![Console](https://img.shields.io/badge/Console-000000?style=for-the-badge&logo=windows-terminal&logoColor=white)](https://docs.microsoft.com/en-us/windows/console/)

Basic C# console implementation of Conway's Game Of Life.
## Usage
### With randomized initializtion
```C#
GameOfLife gol = new GameOfLife(1900, 500);
gol.Simulate(0, false);
```
### With a prewritten pattern
```C#
GameOfLife gol = new GameOfLife(950, 250, "pattern.txt");
gol.Simulate(0, false);
```
#### Example pattern (Gosper glider gun)
```
.....................................
.........................O...........
.......................O.O...........
.............OO......OO............OO
............O...O....OO............OO
.OO........O.....O...OO..............
.OO........O...O.OO....O.O...........
...........O.....O.......O...........
............O...O....................
.............OO......................
```
## Showcase
### Randomized initialization
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/randomized_Init.gif)
### Breeder 1
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/breeder_1.gif)
### Spacefiller collapse
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/spacefiller_collapse.gif)
### Gosper glider gun
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/gosperglidergun.gif)
