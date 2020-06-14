using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace SymulatorJazdy
{
    public static class LevelsManager
    {
        public delegate Level LevelDelegate();

        public static List<LevelDelegate> AllLevels()
        {
            return new List<LevelDelegate>()
            {
                new LevelDelegate(Level001),
                new LevelDelegate(Level002),
                new LevelDelegate(Level003),
                new LevelDelegate(Level004),
                new LevelDelegate(Level005)
            };
        }

        public static Level Level001()
        {
            List<VisualObject> visualObjects = new List<VisualObject>();
            List<Trigger> triggers = new List<Trigger>();

            int x = 0, y = 0;
            Player player = new Player();
            Texture carTexture = TexturesManager.CarTexture;
            player.ObjectSprite = new Sprite(carTexture);
            player.ObjectSprite.Origin = new Vector2f(carTexture.Size.X / 2, carTexture.Size.Y / 2);
            player.ObjectSprite.Position = new Vector2f(x * TexturesManager.TileSize.X + TexturesManager.TileSize.X / 4 - 10, y * -TexturesManager.TileSize.Y);
            player.ObjectSprite.Rotation = 0;

            x = 0; y = -10;
            for (int i = y; i < 5; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X/2, TexturesManager.TileSize.Y/2);
                roadVertical.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * i);
                visualObjects.Add(roadVertical);
            }

            x = 0; y = 5;

            VisualObject carLeft = new VisualObject();
            Texture carLeftTexture = TexturesManager.CarTexture2;
            carLeft.ObjectSprite = new Sprite(carLeftTexture);
            carLeft.ObjectSprite.Origin = new Vector2f(carLeftTexture.Size.X / 2, carLeftTexture.Size.Y / 2);
            carLeft.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x-1) + 40, -TexturesManager.TileSize.Y * y + TexturesManager.TileSize.Y/4 - 10);
            carLeft.ObjectSprite.Rotation = 90;

            VisualObject cross = new VisualObject();
            cross.ObjectSprite = new Sprite(TexturesManager.Road4Cross);
            cross.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
            cross.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * y);
            visualObjects.Add(cross);

            Trigger interactionTrigger = new Trigger(player, cross.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player },
            new Action<List<VisualObject>>(x =>
            {
                Interaction interaction = new Interaction();
                interaction.Question = "Pojazd żółty jedzie prosto.\nJaka powinna być kolejność przejazdu pojazdów?";
                interaction.Answers = new List<string> { "1. Żółty, Ty", "2. Ty, Żółty" };
                interaction.CorrectIndex = 1;
                (x[0] as Player).CurrentInteraction = interaction;
            }), true);
            triggers.Add(interactionTrigger);

            player.MessageToShow = "Na następnym skrzyżowaniu skręć w lewo";
            Trigger turnLeft = new Trigger(player, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.DownTurnLeft), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    x[0].ObjectSprite.Rotation = 270;
                    (x[0] as Player).MessageToShow = "Dojedź do końca aby zaliczyć poziom";
                }), true);
            triggers.Add(turnLeft);

            //right road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (i + x), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
            }

            //top road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x, -TexturesManager.TileSize.Y * (y + i));
                visualObjects.Add(roadVertical);
            }

            //left road
            x--;
            for (; x > -10; x--)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x, -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
            }

            Trigger finish = new Trigger(player, visualObjects[visualObjects.Count - 1].ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player }, 
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).CarState = Player.State.Finished;
                }), true);
            triggers.Add(finish);

            //dodaje po skrzyzowaniu żeby był na wierzchu
            visualObjects.Add(carLeft);

            //dodaje na końcu, żeby był rysowany zawsze "na wierchu"
            visualObjects.Add(player);

            Level level = new Level();
            level.PlayerObject = player;
            level.VisualObjects = visualObjects;
            level.Triggers = triggers;
            return level;
        }

        public static Level Level002()
        {
            List<VisualObject> visualObjects = new List<VisualObject>();
            List<Trigger> triggers = new List<Trigger>();

            int x = 0, y = 0;
            Player player = new Player();
            Texture carTexture = TexturesManager.CarTexture;
            player.ObjectSprite = new Sprite(carTexture);
            player.ObjectSprite.Origin = new Vector2f(carTexture.Size.X / 2, carTexture.Size.Y / 2);
            player.ObjectSprite.Position = new Vector2f(x * TexturesManager.TileSize.X + TexturesManager.TileSize.X / 4 - 10, y * -TexturesManager.TileSize.Y);
            player.ObjectSprite.Rotation = 0;

            x = 0; y = -10;
            for (int i = y; i < 5; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * i);
                visualObjects.Add(roadVertical);
            }

            x = 0; y = 5;

            VisualObject carRight = new VisualObject();
            Texture carRightTexture = TexturesManager.CarTexture3;
            carRight.ObjectSprite = new Sprite(carRightTexture);
            carRight.ObjectSprite.Origin = new Vector2f(carRightTexture.Size.X / 2, carRightTexture.Size.Y / 2);
            carRight.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x + 1) - 40, -TexturesManager.TileSize.Y * y - TexturesManager.TileSize.Y / 4 + 10);
            carRight.ObjectSprite.Rotation = 270;

            VisualObject cross = new VisualObject();
            cross.ObjectSprite = new Sprite(TexturesManager.Road4Cross);
            cross.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
            cross.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * y);
            visualObjects.Add(cross);

            Trigger interactionTrigger = new Trigger(player, cross.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player, carRight },
            new Action<List<VisualObject>>(x =>
            {
                Interaction interaction = new Interaction();
                interaction.Question = "Pojazd niebieski jedzie prosto.\nJaka powinna być kolejność przejazdu pojazdów?";
                interaction.Answers = new List<string> { "1. Niebieski, Ty", "2. Ty, Niebieski", "3. Mogą jechać jednocześnie" };
                interaction.CorrectIndex = 2;
                (x[0] as Player).CurrentInteraction = interaction;

                x[1].Actions.Add(new Action<VisualObject>(new Action<VisualObject>(x =>
                {
                    x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X - 0.01f * x.DeltaTime, x.ObjectSprite.Position.Y);
                })));
            }), true);
            triggers.Add(interactionTrigger);

            player.MessageToShow = "Na następnym skrzyżowaniu skręć w prawo";
            Trigger turnRight = new Trigger(player, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.DownTurnRight), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    x[0].ObjectSprite.Rotation = 90;
                    (x[0] as Player).MessageToShow = "Dojedź do końca aby zaliczyć poziom";
                }), true);
            triggers.Add(turnRight);

            //right road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (i + x), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
            }

            Trigger finish = new Trigger(player, visualObjects[visualObjects.Count - 1].ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).CarState = Player.State.Finished;
                }), true);
            triggers.Add(finish);

            //top road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x, -TexturesManager.TileSize.Y * (y + i));
                visualObjects.Add(roadVertical);
            }

            //left road
            x--;
            for (; x > -10; x--)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x, -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
            }

            //dodaje po skrzyzowaniu żeby był na wierzchu
            visualObjects.Add(carRight);

            //dodaje na końcu, żeby był rysowany zawsze "na wierchu"
            visualObjects.Add(player);

            Level level = new Level();
            level.PlayerObject = player;
            level.VisualObjects = visualObjects;
            level.Triggers = triggers;
            return level;
        }

        public static Level Level003()
        {
            List<VisualObject> visualObjects = new List<VisualObject>();
            List<Trigger> triggers = new List<Trigger>();

            int x = 0, y = 0;
            Player player = new Player();
            Texture carTexture = TexturesManager.CarTexture;
            player.ObjectSprite = new Sprite(carTexture);
            player.ObjectSprite.Origin = new Vector2f(carTexture.Size.X / 2, carTexture.Size.Y / 2);
            player.ObjectSprite.Position = new Vector2f(x * TexturesManager.TileSize.X + TexturesManager.TileSize.X / 4 - 10, y * -TexturesManager.TileSize.Y);
            player.ObjectSprite.Rotation = 0;

            x = 0; y = -10;
            for (int i = y; i < 5; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * i);
                visualObjects.Add(roadVertical);
            }

            x = 0; y = 5;

            VisualObject carLeft = new VisualObject();
            Texture carLeftTexture = TexturesManager.CarTexture2;
            carLeft.ObjectSprite = new Sprite(carLeftTexture);
            carLeft.ObjectSprite.Origin = new Vector2f(carLeftTexture.Size.X / 2, carLeftTexture.Size.Y / 2);
            carLeft.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x - 1) + 40, -TexturesManager.TileSize.Y * y + TexturesManager.TileSize.Y / 4 - 10);
            carLeft.ObjectSprite.Rotation = 90;

            VisualObject carRight = new VisualObject();
            Texture carRightTexture = TexturesManager.CarTexture3;
            carRight.ObjectSprite = new Sprite(carRightTexture);
            carRight.ObjectSprite.Origin = new Vector2f(carRightTexture.Size.X / 2, carRightTexture.Size.Y / 2);
            carRight.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x + 1) - 40, -TexturesManager.TileSize.Y * y - TexturesManager.TileSize.Y / 4 + 10);
            carRight.ObjectSprite.Rotation = 270;

            VisualObject cross = new VisualObject();
            cross.ObjectSprite = new Sprite(TexturesManager.Road4Cross);
            cross.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
            cross.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * y);
            visualObjects.Add(cross);

            Trigger interactionTrigger = new Trigger(player, cross.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player, carRight },
            new Action<List<VisualObject>>(x =>
            {

                Interaction interaction = new Interaction();
                interaction.Question = "Wszystkie pojazdy jadą prosto.\nJaka powinna być kolejność przejazdu pojazdów?";
                interaction.Answers = new List<string> { "1. Niebieski, Żółty, Ty", "2. Żółty, Ty, Niebieski", "3. Żółty, Niebieski, Ty", "4. Niebieski, Ty, Żółty" };
                interaction.CorrectIndex = 3;
                (x[0] as Player).CurrentInteraction = interaction;
                (x[0] as Player).CarState = Player.State.Stoped;

                x[1].Actions.Add(new Action<VisualObject>(new Action<VisualObject>(x =>
                {
                    x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X - 0.01f * x.DeltaTime, x.ObjectSprite.Position.Y);
                })));
            }), true);
            triggers.Add(interactionTrigger);

            player.MessageToShow = "Na następnym skrzyżowaniu jedź prosto";
            Trigger turnRight = new Trigger(player, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.DownTurnLeft), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).MessageToShow = "Dojedź do końca aby zaliczyć poziom";
                }), true);
            triggers.Add(turnRight);

            //right road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (i + x), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
            }

            //top road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x, -TexturesManager.TileSize.Y * (y + i));
                visualObjects.Add(roadVertical);
            }

            Trigger finish = new Trigger(player, visualObjects[visualObjects.Count - 1].ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).CarState = Player.State.Finished;
                }), true);
            triggers.Add(finish);

            //left road
            x--;
            VisualObject secondLeft = null;
            for (int i = 0; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x - i), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
                if(i == 1)
                {
                    secondLeft = roadVertical;
                }
            }

            Trigger rightCarFinished = new Trigger(carRight, secondLeft.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).CarState = Player.State.OK;
                }), true);
            triggers.Add(rightCarFinished);



            //dodaje na końcu żeby były na wierzchu
            visualObjects.Add(carRight);
            visualObjects.Add(carLeft);
            visualObjects.Add(player);

            Level level = new Level();
            level.PlayerObject = player;
            level.VisualObjects = visualObjects;
            level.Triggers = triggers;
            return level;
        }

        public static Level Level004()
        {
            List<VisualObject> visualObjects = new List<VisualObject>();
            List<Trigger> triggers = new List<Trigger>();

            int x = 0, y = 0;
            Player player = new Player();
            Texture carTexture = TexturesManager.CarTexture;
            player.ObjectSprite = new Sprite(carTexture);
            player.ObjectSprite.Origin = new Vector2f(carTexture.Size.X / 2, carTexture.Size.Y / 2);
            player.ObjectSprite.Position = new Vector2f(x * TexturesManager.TileSize.X + TexturesManager.TileSize.X / 4 - 10, y * -TexturesManager.TileSize.Y);
            player.ObjectSprite.Rotation = 0;

            x = 0; y = -10;
            for (int i = y; i < 5; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * i);
                visualObjects.Add(roadVertical);
            }

            x = 0; y = 5;

            VisualObject carRight = new VisualObject();
            Texture carRightTexture = TexturesManager.CarTexture3;
            carRight.ObjectSprite = new Sprite(carRightTexture);
            carRight.ObjectSprite.Origin = new Vector2f(carRightTexture.Size.X / 2, carRightTexture.Size.Y / 2);
            carRight.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x + 1) - 40, -TexturesManager.TileSize.Y * y - TexturesManager.TileSize.Y / 4 + 10);
            carRight.ObjectSprite.Rotation = 270;

            VisualObject carTop = new VisualObject();
            Texture carTopTexture = TexturesManager.CarTexture4;
            carTop.ObjectSprite = new Sprite(carTopTexture);
            carTop.ObjectSprite.Origin = new Vector2f(carTopTexture.Size.X / 2, carTopTexture.Size.Y / 2);
            carTop.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x - TexturesManager.TileSize.X/4 + 10, -TexturesManager.TileSize.Y * (y + 1) + 40);
            carTop.ObjectSprite.Rotation = 180;

            VisualObject cross = new VisualObject();
            cross.ObjectSprite = new Sprite(TexturesManager.Road4Cross);
            cross.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
            cross.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * y);
            visualObjects.Add(cross);

            Trigger interactionTrigger = new Trigger(player, cross.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player, carTop },
            new Action<List<VisualObject>>(x =>
            {
                Interaction interaction = new Interaction();
                interaction.Question = "Pojazd ciężarowy skręca w lewo.\nPojazd niebieski jedzie prosto.\nJaka powinna być kolejność przejazdu pojazdów?";
                interaction.Answers = new List<string> { "1. Ciężarowy, Niebieski, Ty", "2. Ty, Niebieski, Ciężarowy", "3. Niebieski, Ciężarowy, Ty", "4. Niebieski, Ty, Ciężarowy" };
                interaction.CorrectIndex = 0;
                (x[0] as Player).CurrentInteraction = interaction;
                (x[0] as Player).CarState = Player.State.Stoped;

                x[1].Actions.Add(new Action<VisualObject>(new Action<VisualObject>(x =>
                {
                    x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X, x.ObjectSprite.Position.Y + 0.01f * x.DeltaTime);
                })));
            }), true);
            triggers.Add(interactionTrigger);

            player.MessageToShow = "Na następnym skrzyżowaniu jedź prosto";
            Trigger turnRight = new Trigger(player, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.DownTurnLeft), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).MessageToShow = "Dojedź do końca aby zaliczyć poziom";
                }), true);
            triggers.Add(turnRight);

            VisualObject secondRight = null;
            //right road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (i + x), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
                if(i == 2)
                {
                    secondRight = roadVertical;
                }
            }

            //top road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x, -TexturesManager.TileSize.Y * (y + i));
                visualObjects.Add(roadVertical);
            }

            Trigger finish = new Trigger(player, visualObjects[visualObjects.Count - 1].ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).CarState = Player.State.Finished;
                }), true);
            triggers.Add(finish);

            //left road
            x--;
            VisualObject secondLeft = null;
            for (int i = 0; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x - i), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
                if (i == 1)
                {
                    secondLeft = roadVertical;
                }
            }

            Trigger topCarTurningLeft = new Trigger(carTop, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.TopTurnLeft), new List<VisualObject>() { carTop },
                new Action<List<VisualObject>>(x =>
                {
                    x[0].ObjectSprite.Rotation = 90;
                    x[0].Actions[0] = new Action<VisualObject>(x =>
                    {
                        x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X + 0.01f * x.DeltaTime, x.ObjectSprite.Position.Y);
                    });
                    //(x[0] as Player).CarState = Player.State.OK;
                }), true);
            triggers.Add(topCarTurningLeft);

            Trigger topCarFinished = new Trigger(carTop, secondRight.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { carRight },
                new Action<List<VisualObject>>(x =>
                {
                    x[0].Actions.Add(new Action<VisualObject>(x =>
                    {
                        x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X - 0.01f * x.DeltaTime, x.ObjectSprite.Position.Y);
                    }));
                }), true);
            triggers.Add(topCarFinished);

            Trigger rightCarFinished = new Trigger(carRight, secondLeft.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player }, 
                new Action<List<VisualObject>>(x => {
                    (x[0] as Player).CarState = Player.State.OK;
                }), true);
            triggers.Add(rightCarFinished);

            //dodaje na końcu żeby były na wierzchu
            visualObjects.Add(carTop);
            visualObjects.Add(carRight);
            visualObjects.Add(player);

            Level level = new Level();
            level.PlayerObject = player;
            level.VisualObjects = visualObjects;
            level.Triggers = triggers;
            return level;
        }

        public static Level Level005()
        {
            List<VisualObject> visualObjects = new List<VisualObject>();
            List<Trigger> triggers = new List<Trigger>();

            int x = 0, y = 0;
            Player player = new Player();
            Texture carTexture = TexturesManager.CarTexture;
            player.ObjectSprite = new Sprite(carTexture);
            player.ObjectSprite.Origin = new Vector2f(carTexture.Size.X / 2, carTexture.Size.Y / 2);
            player.ObjectSprite.Position = new Vector2f(x * TexturesManager.TileSize.X + TexturesManager.TileSize.X / 4 - 10, y * -TexturesManager.TileSize.Y);
            player.ObjectSprite.Rotation = 0;

            x = 0; y = -10;
            for (int i = y; i < 5; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * i);
                visualObjects.Add(roadVertical);
            }

            x = 0; y = 5;

            VisualObject ambulance = new VisualObject();
            Texture ambulanceTexture = TexturesManager.AmbulanceTexture;
            ambulance.ObjectSprite = new Sprite(ambulanceTexture);
            ambulance.ObjectSprite.Origin = new Vector2f(ambulanceTexture.Size.X / 2, ambulanceTexture.Size.Y / 2);
            ambulance.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x - 1) + 30, -TexturesManager.TileSize.Y * y + TexturesManager.TileSize.Y / 4 - 10);
            ambulance.ObjectSprite.Rotation = 90;

            VisualObject carTop = new VisualObject();
            Texture carTopTexture = TexturesManager.CarTexture4;
            carTop.ObjectSprite = new Sprite(carTopTexture);
            carTop.ObjectSprite.Origin = new Vector2f(carTopTexture.Size.X / 2, carTopTexture.Size.Y / 2);
            carTop.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x - TexturesManager.TileSize.X / 4 + 10, -TexturesManager.TileSize.Y * (y + 1) + 40);
            carTop.ObjectSprite.Rotation = 180;

            VisualObject cross = new VisualObject();
            cross.ObjectSprite = new Sprite(TexturesManager.Road4Cross);
            cross.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
            cross.ObjectSprite.Position = new Vector2f(0, -TexturesManager.TileSize.Y * y);
            visualObjects.Add(cross);

            Trigger interactionTrigger = new Trigger(player, cross.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player, ambulance },
            new Action<List<VisualObject>>(x =>
            {
                Interaction interaction = new Interaction();
                interaction.Question = "Pojazd uprzywilejowany jedzie prosto.\nPojazd ciężarowy skręca w prawo.\nJaka powinna być kolejność przejazdu pojazdów?";
                interaction.Answers = new List<string> { "1. Uprzywilejowany a później Ciężarowy i Ty jednocześnie", "2. Uprzywilejowany, Ty, Ciężarowy", "3. Uprzywilejowany, Ciężarowy, Ty", 
                    "4. Ty, Ciężarowy, Uprzywilejowany" };
                interaction.CorrectIndex = 0;
                (x[0] as Player).CurrentInteraction = interaction;
                (x[0] as Player).CarState = Player.State.Stoped;

                x[1].Actions.Add(new Action<VisualObject>(new Action<VisualObject>(x =>
                {
                    x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X + 0.01f * x.DeltaTime, x.ObjectSprite.Position.Y);
                })));
            }), true);
            triggers.Add(interactionTrigger);

            player.MessageToShow = "Na następnym skrzyżowaniu jedź prosto";
            Trigger turnRight = new Trigger(player, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.DownTurnLeft), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).MessageToShow = "Dojedź do końca aby zaliczyć poziom";
                }), true);
            triggers.Add(turnRight);

            VisualObject secondRight = null;
            //right road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (i + x), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
                if (i == 2)
                {
                    secondRight = roadVertical;
                }
            }

            //top road
            for (int i = 1; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadVerticalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * x, -TexturesManager.TileSize.Y * (y + i));
                visualObjects.Add(roadVertical);
            }

            Trigger finish = new Trigger(player, visualObjects[visualObjects.Count - 1].ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).CarState = Player.State.Finished;
                }), true);
            triggers.Add(finish);

            //left road
            x--;
            VisualObject secondLeft = null;
            for (int i = 0; i < 10; i++)
            {
                VisualObject roadVertical = new VisualObject();
                roadVertical.ObjectSprite = new Sprite(TexturesManager.RoadHorizontalTile);
                roadVertical.ObjectSprite.Origin = new Vector2f(TexturesManager.TileSize.X / 2, TexturesManager.TileSize.Y / 2);
                roadVertical.ObjectSprite.Position = new Vector2f(TexturesManager.TileSize.X * (x - i), -TexturesManager.TileSize.Y * y);
                visualObjects.Add(roadVertical);
                if (i == 1)
                {
                    secondLeft = roadVertical;
                }
            }

            Trigger topCarTurningRight = new Trigger(carTop, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.TopTurnRight), new List<VisualObject>() { carTop },
                new Action<List<VisualObject>>(x =>
                {
                    x[0].ObjectSprite.Rotation = 270;
                    x[0].Actions[0] = new Action<VisualObject>(x =>
                    {
                        x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X - 0.01f * x.DeltaTime, x.ObjectSprite.Position.Y);
                    });
                }), true);
            triggers.Add(topCarTurningRight);

            Trigger ambulanceFinished = new Trigger(ambulance, secondRight.ObjectSprite.GetGlobalBounds(), new List<VisualObject>() { player, carTop },
                new Action<List<VisualObject>>(x =>
                {
                    (x[0] as Player).CarState = Player.State.OK;
                    x[1].Actions.Add(new Action<VisualObject>(x =>
                    {
                        x.ObjectSprite.Position = new Vector2f(x.ObjectSprite.Position.X, x.ObjectSprite.Position.Y + 0.01f * x.DeltaTime);
                    }));
                }), true);
            triggers.Add(ambulanceFinished);


            //dodaje na końcu żeby były na wierzchu
            visualObjects.Add(carTop);
            visualObjects.Add(ambulance);
            visualObjects.Add(player);

            Level level = new Level();
            level.PlayerObject = player;
            level.VisualObjects = visualObjects;
            level.Triggers = triggers;
            return level;
        }

        private enum TurnDirection
        {
            DownTurnLeft,
            DownTurnRight,
            TopTurnLeft,
            TopTurnRight
        }

        private static FloatRect GetTurnRect(FloatRect rect, TurnDirection turn)
        {
            if (turn == TurnDirection.DownTurnLeft)
            {
                return new FloatRect(rect.Left + rect.Width / 2, rect.Top + 10, rect.Width / 2, 10);
            }
            else if(turn == TurnDirection.DownTurnRight)
            {
                return new FloatRect(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2 - 15, rect.Width / 2, 10);
            }
            else if(turn == TurnDirection.TopTurnLeft)
            {
                return new FloatRect(rect.Left, rect.Top + rect.Height - 10, rect.Width / 2, 15);
            }
            else
            {
                //top turn right
                return new FloatRect(rect.Left, rect.Top + rect.Height/2 + 15, rect.Width / 2, 15);
            }
        }
    }
}
