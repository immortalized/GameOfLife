# GameOfLife
Basic console implementation of Conway's Game Of Life.
## Usage
### With randomized initializtion
```C#
GameOfLife gol = new GameOfLife(200, 50);
gol.RunSimulation(70, false);
```
### With a pre-written pattern
```C#
GameOfLife gol = new GameOfLife(200, 50, "gosper_glider_gun.txt");
gol.RunSimulation(70, false);
```
## Showcase
### Randomized initialization
![](https://github.com/immortalized/GameOfLife/blob/main/Other/randomized.gif)
### Gosper glider gun
![](https://github.com/immortalized/GameOfLife/blob/main/Other/glidergun.gif)
