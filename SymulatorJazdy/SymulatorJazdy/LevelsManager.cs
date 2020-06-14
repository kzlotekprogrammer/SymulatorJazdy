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
                new LevelDelegate(Level001)
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
                //(x[0] as Player).CarState = Player.State.Stoped;
                Interaction interaction = new Interaction();
                interaction.Question = "Pojazd żółty jedzie prosto.\nJaka powinna być kolejność przejazdu pojazdów?";
                interaction.Answers = new List<string> { "1. Żółty, Ty", "2. Ty, Żółty" };
                interaction.CorrectIndex = 1;
                (x[0] as Player).CurrentInteraction = interaction;
            }), true);
            triggers.Add(interactionTrigger);

            player.MessageToShow = "Na następnym skrzyżowaniu skręć w lewo";
            Trigger turnLeft = new Trigger(player, GetTurnRect(cross.ObjectSprite.GetGlobalBounds(), TurnDirection.TopToLeft), new List<VisualObject>() { player },
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

        private enum TurnDirection
        {
            TopToLeft,
            TopToRight
        }

        private static FloatRect GetTurnRect(FloatRect rect, TurnDirection turn)
        {
            if (turn == TurnDirection.TopToLeft)
            {
                return new FloatRect(rect.Left + rect.Width / 2, rect.Top + 10, rect.Width / 2, 10);
            }
            else
            {
                //toptoright
                return new FloatRect(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2 - 15, rect.Width / 2, 10);
            }
        }
    }
}
