# GameOfLife
Basic C# console implementation of Conway's Game Of Life.
## Usage
### With randomized initializtion
```C#
GameOfLife gol = new GameOfLife(1900, 500);
gol.Simulate(0, false);
```
### With a prewritten pattern
```C#
GameOfLife gol = new GameOfLife(950, 250, "gosper_glider_gun.txt");
gol.Simulate(30, false);
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
### Randomized init
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/randomizedinit2.gif)
### Breeder 1
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/breeder1.gif)
