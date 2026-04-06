using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using _Debug = STG.Engine.Debugging.Debug;
using Point = Microsoft.Xna.Framework.Point;

namespace STG.Engine.Graphics {
    /// <summary>
    /// テクスチャのサイズなどを保持するクラス
    /// </summary>
    public class TexData {
        public Texture2D texture;
        public Bitmap bitmap;
        public Point size;

        public TexData(Texture2D texture, string name, Point size, Bitmap bitmap) {
            this.texture = texture;
            this.size = size;
            this.bitmap = bitmap;

            _Debug.Log($"name:{name} , size:{size}");
        }
    }
}
