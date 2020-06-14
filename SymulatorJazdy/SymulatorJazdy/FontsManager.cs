using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SymulatorJazdy
{
    public static class FontsManager
    {
        private static Font _default;

        public static Font Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new Font("fonts/AbhayaLibre-Regular.ttf");
                }
                return _default;
            }
        }
    }
}
