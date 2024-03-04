# GameOfLife
Basic C# console implementation of Conway's Game Of Life.
## Usage
### With randomized initializtion
```C#
GameOfLife gol = new GameOfLife(950, 250, 4);
gol.Simulate(0, false);
```
### With a prewritten pattern
```C#
GameOfLife gol = new GameOfLife(200, 100, 4, "gosper_glider_gun.txt");
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
### Breeder 1
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/breeder1.gif)
### Gosper glider gun
![](https://github.com/immortalized/GameOfLife/blob/main/Showcase/gosperglidergun.gif)
