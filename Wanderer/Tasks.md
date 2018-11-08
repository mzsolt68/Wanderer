# Full Week Project: Wanderer - The RPG game

Build a hero based walking on tiles and killing monsters type of game. The hero
is controlled in a maze using the keyboard. Heroes and monsters have levels and
stats depending on their levels. The goal is reach the highest level by killing
the monsters holding the keys to the next level.

## Workshop: Plan your work

### 0. Fork this repository (under your user)

### 1. Clone the repository to your computer

### 2. Go through the technical details

#### How to launch the program

- We will use our [FoxDraw.cs](FoxDraw.cs) class in the following examples
  - But you can write your own until the end of the week! :)
- When reading through the specification and the stories again keep this in mind.
- Here's an example, it contains
  - a big drawable canvas
  - and handling pressing keys, for moving your hero around
  - be aware that these are just all the needed concepts put in one place
  - you can separate anything anyhow  
  - `RPGGame` is the namespace in the following example

  ```xml
  <Window x:Class="RPGGame.MainWindow"
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
          xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
          xmlns:local="clr-namespace:RPGGame"
          mc:Ignorable="d"
          Title="MainWindow" Height="551" Width="1008" KeyDown="WindowKeyDown">
      <Grid>
          <Canvas Name="canvas"/>
      </Grid>
  </Window>
  ```

  ```csharp
  public partial class MainWindow : Window
  {
      public MainWindow()
      {
          InitializeComponent();
          var foxDraw = new FoxDraw(canvas);
      }

      private void WindowKeyDown(object sender, KeyEventArgs e)
      {
          if (e.Key == Key.Left)
          {
              Console.WriteLine("To the left!");
          }

          if (e.Key == Key.Right)
          {
              Console.WriteLine("To the right!");
          }
      }
  }
  ```

  - You can add images with FoxDraw like this:
    - `foxDraw.AddImage(imagesource, x, y);`
  - Steps of adding an Image to your project:
    - Add an `Assets` folder to your project
    - Insert image files in it
    - Set the `Copy to OutPut Directory` property of the image to `Copy Always`

  ```csharp
  public partial class MainWindow : Window
  {
      public MainWindow()
      {
          InitializeComponent();
          var foxDraw = new FoxDraw(canvas);
          foxDraw.AddImage("./assets/boss.png", 0, 0);
      }
  }
  ```

### 3. Create a GitHub project under your repository for your work and add the [project stories](https://github.com/greenfox-academy/teaching-materials/blob/master/project/wanderer/stories.md).

### 4. Form groups and plan your application together.

Plan your architecture. In your architecture you should consider the following components:

- Models
    - GameObject
        - Character
            - Monster
            - Hero
            - types
        - Area
        - Tile
            - EmptyTile
            - NotEmptyTile
- GameLogic
    - current hero
    - current area
- Main
    - handling events
    - current game

#### 5. Think about task breakdown in Kanban together

Now that you see the big picture, **go through the stories together** and think about how to break them down into tasks:

- To classes
- To methods
- To data and actions
- Extend the story cards with some of these points as a reminder

#### 6. Start working on your first task!
