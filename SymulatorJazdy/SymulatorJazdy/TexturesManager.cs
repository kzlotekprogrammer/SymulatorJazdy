using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace SymulatorJazdy
{
    public static class TexturesManager
    {
        public static  Vector2i TileSize = new Vector2i(128, 128);

        private static string RoadTileSheetFilePath = "textures/roadTextures_tilesheet.png";
        private static string CarTextureFilePath = "textures/car-truck{0}.png";

        private static Texture _roadVertical;
        private static Texture _carTexture;
        private static Texture _carTexture2;
        private static Texture _grass;
        private static Texture _road4Cross;
        private static Texture _roadHorizontal;

        public static Texture RoadVerticalTile
        {
            get
            {
                if(_roadVertical == null)
                {
                    _roadVertical = new Texture(RoadTileSheetFilePath, new IntRect(0, 0, TileSize.X, TileSize.Y));
                }
                return _roadVertical;
            }
        }

        public static Texture RoadHorizontalTile
        {
            get
            {
                if (_roadHorizontal == null)
                {
                    _roadHorizontal = new Texture(RoadTileSheetFilePath, new IntRect(0, TileSize.Y*1, TileSize.X, TileSize.Y));
                }
                return _roadHorizontal;
            }
        }

        public static Texture Road4Cross
        {
            get
            {
                if (_road4Cross == null)
                {
                    _road4Cross = new Texture(RoadTileSheetFilePath, new IntRect(TileSize.X*9, 0, TileSize.X, TileSize.Y));
                }
                return _road4Cross;
            }
        }

        public static Texture CarTexture
        {
            get
            {
                if(_carTexture == null)
                {
                    _carTexture = new Texture(String.Format(CarTextureFilePath, 1));
                }
                return _carTexture;
            }
        }

        public static Texture CarTexture2
        {
            get
            {
                if (_carTexture2 == null)
                {
                    _carTexture2 = new Texture(String.Format(CarTextureFilePath, 2));
                }
                return _carTexture2;
            }
        }

        public static Texture Grass
        {
            get
            {
                if (_grass == null)
                {
                    _grass = new Texture(RoadTileSheetFilePath, new IntRect(0, TileSize.Y*2, TileSize.X, TileSize.Y));
                }
                return _grass;
            }
        }
    }
}
