using System;
using System.Collections.Generic;
using System.Diagnostics;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SymulatorJazdy.LevelsManager;

namespace SymulatorJazdy
{
    class Program
    {
        private static RenderWindow MainWindow;
        private static Stopwatch Timer;
        private static bool GameStopped;
        private static List<LevelDelegate> Levels;
        private static int CurrentLevelIndex;
        private static Level CurrentLevel;
        private static long TimeStep = 60;
        private static View CurrentView;

        static void Main(string[] args)
        {
            MainWindow = new RenderWindow(VideoMode.DesktopMode, "Hello world!");

            //levels
            Levels = LevelsManager.AllLevels();
            CurrentLevelIndex = 0;
            if(Levels.Count <= 0)
            {
                return;
            }
            CurrentLevel = Levels[CurrentLevelIndex]();

            MainWindow.Closed += RenderWindow_Closed;
            MainWindow.KeyPressed += RenderWindow_KeyPressed;

            Timer = new Stopwatch();
            Timer.Start();
            while (MainWindow.IsOpen)
            {
                if(CurrentLevel.PlayerObject.CarState == Player.State.Crashed)
                {
                    GameStopped = true;
                }
                else if(CurrentLevel.PlayerObject.CarState == Player.State.Finished)
                {
                    if (CurrentLevelIndex + 1 < Levels.Count)
                    {
                        CurrentLevelIndex++;
                        CurrentLevel = Levels[CurrentLevelIndex]();
                    }
                    else
                    {
                        CurrentLevel.PlayerObject.MessageToShow = "To był ostatni poziom";
                    }
                }

                MainWindow.DispatchEvents();

                if(!GameStopped)
                {
                    long deltaTime = Timer.ElapsedMilliseconds;
                    if (deltaTime >= TimeStep)
                    {
                        CurrentLevel.VisualObjects.ForEach(x => x.DeltaTime = TimeStep);
                        Timer.Elapsed.Add(TimeSpan.FromMilliseconds(-TimeStep));

                        CurrentLevel.VisualObjects.ForEach(x => x.DoActions());

                        CurrentLevel.Triggers.ForEach(x => x.TryToTrigger());
                    }
                }


                MainWindow.Clear();
                //move view...
                Vector2f playerPosition = CurrentLevel.PlayerObject.ObjectSprite.Position;
                CurrentView = new View(new FloatRect(playerPosition.X - MainWindow.Size.X/2, playerPosition.Y - MainWindow.Size.Y / 2, MainWindow.Size.X, MainWindow.Size.Y));
                MainWindow.SetView(CurrentView);

                DrawBackground();
                CurrentLevel.VisualObjects.ForEach(x => MainWindow.Draw(x.ObjectSprite));

                if(CurrentLevel.PlayerObject.CurrentInteraction != null)
                {
                    ShowInteraction(CurrentLevel.PlayerObject.CurrentInteraction);
                }

                if (CurrentLevel.PlayerObject.CarState == Player.State.Crashed)
                {
                    ShowMessage("Rozbiłeś swój samochów!\nNaciśnij R żeby zrestartować poziom");
                }
                else if(CurrentLevel.PlayerObject.MessageToShow != null)
                {
                    ShowMessage(CurrentLevel.PlayerObject.MessageToShow);
                }

                MainWindow.Display();
            }
        }

        private static void ShowInteraction(Interaction currentInteraction)
        {
            GameStopped = true;
            string str = currentInteraction.Question + "\n";
            currentInteraction.Answers.ForEach(x => str += x + "\n");
            Text interactionText = new Text(str, FontsManager.Default);
            interactionText.FillColor = Color.Black;
            interactionText.Position = new Vector2f(CurrentView.Center.X + 40, CurrentView.Center.Y - VideoMode.DesktopMode.Height / 2 + 10);
            MainWindow.Draw(interactionText);
        }

        private static void ShowMessage(string messageToShow)
        {
            Text message = new Text(messageToShow, FontsManager.Default);
            message.FillColor = Color.Black;
            message.Position = new Vector2f(CurrentView.Center.X - VideoMode.DesktopMode.Width / 2 + 10, CurrentView.Center.Y - VideoMode.DesktopMode.Height / 2 + 10);
            MainWindow.Draw(message);
        }

        private static void DrawBackground()
        {
            long xCount = VideoMode.DesktopMode.Width / TexturesManager.TileSize.X + 1;
            long yCount = VideoMode.DesktopMode.Height / TexturesManager.TileSize.Y + 1;
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    VisualObject backgroundElement = new VisualObject();
                    backgroundElement.ObjectSprite = new Sprite(TexturesManager.Grass);   
                        backgroundElement.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * i + CurrentView.Center.X - VideoMode.DesktopMode.Width / 2, 
                            TexturesManager.TileSize.Y * j + CurrentView.Center.Y - VideoMode.DesktopMode.Height/2);
                    MainWindow.Draw(backgroundElement.ObjectSprite);
                }
            }

        }

        private static void RenderWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.R)
            {
                CurrentLevel = Levels[CurrentLevelIndex]();
                GameStopped = false;
            }

            if (CurrentLevel.PlayerObject.CurrentInteraction != null)
            {
                Interaction interaction = CurrentLevel.PlayerObject.CurrentInteraction;
                if (e.Code == Keyboard.Key.Num1)
                {
                    if(interaction.CorrectIndex == 0)
                    {
                        GameStopped = false;
                    }
                    else
                    {
                        CurrentLevel.PlayerObject.MessageToShow = "Błędna odpowiedź. Naciśnij R aby zrestartować poziom";
                    }

                    CurrentLevel.PlayerObject.CurrentInteraction = null;
                }
                else if (e.Code == Keyboard.Key.Num2)
                {
                    if (interaction.CorrectIndex == 1)
                    {
                        GameStopped = false;
                    }
                    else
                    {
                        CurrentLevel.PlayerObject.MessageToShow = "Błędna odpowiedź. Naciśnij R aby zrestartować poziom";
                    }

                    CurrentLevel.PlayerObject.CurrentInteraction = null;
                }
                return;
            }

            if (e.Code == Keyboard.Key.W)
            {
                float xSpeed = CurrentLevel.PlayerObject.Speed.X;
                float ySpeed = CurrentLevel.PlayerObject.Speed.Y;
                ySpeed--;
                ySpeed = Math.Max(ySpeed, -5);
                CurrentLevel.PlayerObject.Speed = new Vector2f(xSpeed, ySpeed);
            }
            else if(e.Code == Keyboard.Key.S)
            {
                float xSpeed = CurrentLevel.PlayerObject.Speed.X;
                float ySpeed = CurrentLevel.PlayerObject.Speed.Y;
                ySpeed++;
                ySpeed = Math.Min(ySpeed, 0);
                CurrentLevel.PlayerObject.Speed = new Vector2f(xSpeed, ySpeed);
            }
        }

        private static void RenderWindow_Closed(object sender, EventArgs e)
        {
            MainWindow.Close();
        }
    }
}
