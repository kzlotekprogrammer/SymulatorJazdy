using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SymulatorJazdy
{
    public class VisualObject
    {
        public long DeltaTime { get; set; }
        public Sprite ObjectSprite { get; set; }

        public List<Action<VisualObject>> Actions { get; set; }

        public VisualObject()
        {
            Actions = new List<Action<VisualObject>>();
        }

        public virtual void DoActions()
        {
            if(Actions == null)
            {
                return;
            }

            Actions.ForEach(action => action(this));
        }
    }
}
