using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SymulatorJazdy
{
    public class Trigger
    {
        public VisualObject TriggeringObject { get; set; }
        public FloatRect TriggeringRect { get; set; }
        public List<VisualObject> ObjectsToTrigger { get; set; }
        public Action<List<VisualObject>> ActionToTrigger { get; set; }
        public bool IsActive { get; set; }

        public Trigger(VisualObject triggeringObject, FloatRect triggeringRect, List<VisualObject> objectsToTrigger, Action<List<VisualObject>> actionToTrigger, bool isActive)
        {
            TriggeringObject = triggeringObject;
            TriggeringRect = triggeringRect;
            ObjectsToTrigger = objectsToTrigger;
            ActionToTrigger = actionToTrigger;
            IsActive = isActive;
        }

        public void TryToTrigger()
        {
            if(!IsActive)
            {
                return;
            }

            if(TriggeringObject.ObjectSprite.GetGlobalBounds().Intersects(TriggeringRect))
            {
                IsActive = false;
                ActionToTrigger(ObjectsToTrigger);
            }
        }
    }
}
