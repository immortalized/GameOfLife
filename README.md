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
### Note that you can't really go higher than a couple hundred thousand total cells, because even StringBuilder is slow to render more than that.
#### Example pattern (Gosper glider gun)
(1 represents a living cell, anything else is a dead one)
```

                         1
                       1 1
             11      11            11
            1   1    11            11
 11        1     1   11
 11        1   1 11    1 1
           1     1       1
            1   1
             11
```
## Showcase
### Randomized initialization
![](https://github.com/immortalized/GameOfLife/blob/main/Other/randomizedinit.gif)
### Gosper glider gun (prewritten pattern)
![](https://github.com/immortalized/GameOfLife/blob/main/Other/gosperglidergun.gif)
