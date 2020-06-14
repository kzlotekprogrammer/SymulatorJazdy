using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace SymulatorJazdy
{
    public class Player : VisualObject
    {
        public enum State
        {
            OK,
            Crashed,
            Stoped,
            Finished
        }

        public Vector2f Speed { get; set; }
        public State CarState { get; set; }

        public string MessageToShow { get; set; }
        public Interaction CurrentInteraction { get; set; }

        public Player()
        {
            CarState = State.OK;
            Actions.Add(new Action<VisualObject>(x =>
            {
                if(CarState != State.OK)
                {
                    return;
                }

                float delX = 0, delY = 0;
                if(ObjectSprite.Rotation == 0)
                {
                    delY = Speed.Y * DeltaTime / 500.0f;
                } 
                else if(ObjectSprite.Rotation == 90)
                {
                    delX = -Speed.Y * DeltaTime / 500.0f;
                }
                else if (ObjectSprite.Rotation == 180)
                {
                    delY = -Speed.Y * DeltaTime / 500.0f;
                }
                else if (ObjectSprite.Rotation == 270)
                {
                    delX = Speed.Y * DeltaTime / 500.0f;
                }
                else
                {
                    throw new Exception("Not supported rotation " + ObjectSprite.Rotation);
                }

                ObjectSprite.Position = new Vector2f(ObjectSprite.Position.X + delX, ObjectSprite.Position.Y + delY);
            }));
        }
    }
}
