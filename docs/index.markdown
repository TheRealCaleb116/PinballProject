---
layout: default
---

![Image Here](/assets/img/PinballGame.png)

# Pinball Game - Caleb Wiebolt

Below is the writeup for my pinball game. It was created in Unity for my Animation and Planning in Games class. All the graded features that I attempeted can be found below



## Features Attempted
### Showcase Video


{% include video.html %}


### Feature List

| Feature                           | Description       | TimeCode |
|:-------------                     |:------------------|:------|
| Basic Pinball Dynamics            | Simple, physcially acurate simulation of the ball as it moves and bounces | The whole video  |
| Multiple Balls Interacting        | Multiple balls on screen at the same time interacting with eachother | 1:37-2:04   |
| Circular Obstacles                | I implemented circular objects for the balls to boucne off of. | 0:43-0:51  |
| Line Segment Obstacles            | The use of line segemnts/polygons, used on the upper boarder and the border by the flippers. | 0:53-0:59  |
| Plunger/Launcher to shoot balls   | I implemented a plunger that is controlled by the space bar to launch balls into the scene | 0:02-0:10  |
| Textured Background               | I textured the background of the pinball table with a wood texture | The whole video  |
| Textured Obstacles                | I textured various obsticles, especially the circular bumpers with textures depending on their point value. | The whole video  |
| Reactive Obstacles                | When a pinball hits one of the point giving bumpers the bumper will flash. | 1:12-1:17  |
| Score Display                     | The current score is displayed graphically on screen and updates as scoring bumpers are hit | The whole video  |
| Pinball Game                      | Both flippers work independantly with the left and right mouse buttons and balls collide off of the flippers naturally. To start the game you press the start button. Your goal is to hit as many of the scoring bumpers as possible. You have a limited number of balls you can launch onto the screen and once they are gone it is game over. | 0:55-1:36  |


## Tools and Libaries Used
*   Unity 2022.3.9f1 and Visual Studio


## Assets Used
*   Pinball art assets by <a href="https://www.freepik.com/free-vector/pinball-machine-parts-realistic-collection_13804973.htm"> macrovector</a> on Freepik
*   Wood background texture by <a href="https://www.freepik.com/free-photo/damaged-parquet-texture_969026.htm">fwstudio</a> on Freepik

## Difficulties Encountered
Building this game was a fun challenege. No matter how many times i seem to write collision code it is always finicky and messy thing to get working cleanly. One of the main challenges i ran into while coding this projects was my tendency to scope creep. This was especially true when it came to things like OOP design practices. For the collision system especually i started with a very generalized, nicley designed OOP version that just didn't work. It could have worked but i realized as i was writting it how much time i was spending writing code that i would never need for this project. Code that, whole well designed, was just making it harder for me to troubleshoot what was happening in this small game. I ended up with a more tailored aproach in design for this game. Is it wonderfully designed with long term scalability in mind, no. Does it fit this application well without being unweildy or restrctive, yes. My main take away is to start by solving the problem in front of you, make it work before you make it perfect.

