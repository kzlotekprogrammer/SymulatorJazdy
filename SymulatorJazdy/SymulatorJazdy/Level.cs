using System;
using System.Collections.Generic;
using System.Text;

namespace SymulatorJazdy
{
    public class Level
    {
        public Player PlayerObject { get; set; }
        public List<VisualObject> VisualObjects { get; set; }
        public List<Trigger> Triggers { get; set; }
    }
}
