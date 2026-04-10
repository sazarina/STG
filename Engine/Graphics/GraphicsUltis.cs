using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using STG.Engine.Component;
using static STG.Engine.Helper.FileLocation;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace STG.Engine.Graphics {
    public static class GraphicsUltis {
        static GraphicsDevice device;
        static SpriteBatch spriteBatch;

        //Texture2DにWidthとHeightがあるため、不要になった
        //public static Dictionary<Texture2D, TexData> dictBitmap = new Dictionary<Texture2D, TexData>();

        static SpriteFont spriteFont;

        public static void Initialize(GraphicsDevice device, SpriteBatch spriteBatch) {
            GraphicsUltis.device = device;
            GraphicsUltis.spriteBatch = spriteBatch;
            spriteFont = LoadHelper.Content.Load<SpriteFont>("DebugText");
        }

        public static void DrawSprite(Texture2D texture, Transform transform) {
            Vector2 vec = transform.center;
            var sourceRect = new Rectangle(new Point(0, 0), new Point(texture.Width, texture.Height));
            var color = Color.White;
            //spriteBatch.Draw(texture, sourceRect, vec, position, color);
            spriteBatch.Draw(texture, transform.position, sourceRect, color);
        }

        public static void DrawSprite(Texture2D texture, Vector2 position, float rotation,Vector2 scale, SpriteEffects spriteEffects = SpriteEffects.None) {
            Vector2 vec = new Vector2(0, 0);
            var sourceRect = new Rectangle(new Point(0, 0), new Point(texture.Width, texture.Height));
            var color = Color.White;

            //origin と scaleが追加されている
            var origin = new Vector2(texture.Width / 2, texture.Height / 2);

            spriteBatch.Draw(texture, position: position, sourceRectangle: sourceRect, color: color, rotation,origin, scale, effects: spriteEffects, 0);
        }

        //public static void DrawSprite(SpriteTextures texture, Vector2 position, float rotation, Vector2 scale, SpriteEffects spriteEffects = SpriteEffects.None) {
        //    Vector2 vec = new Vector2(0, 0);
        //    var sourceRect = new Rectangle(new Point(0, 0), dictBitmap[texture].size);
        //    var color = Color.White;

        //    var origin = new Vector2(texture.Width / 2, texture.Height / 2);

        //    spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, spriteEffects, 0);
        //}

        public static void DrawSprite(Texture2D texture, int x, int y, Vector2 center = default) {
            var sourceRect = new Rectangle(new Point(0, 0), new Point(texture.Width, texture.Height));
            var position = new Vector2(x, y) + center;
            var color = Color.White;
            spriteBatch.Draw(texture, position, sourceRect, color);
        }

        public static void DrawText(string text, Vector2 vector, Color color = default) {
            if (color == default) {
                color = Color.White;
            }
            spriteBatch.DrawString(spriteFont, text, vector, color);
        }

        public static void DrawText(object text, Vector2 vector, Color color = default) {
            if (color == default) {
                color = Color.White;
            }
            spriteBatch.DrawString(spriteFont, text.ToString(), vector, color);
        }

        public static Bitmap LoadBitmap(string path) => new Bitmap(ContentFolderDirectory + path);

        public static Texture2D CreateTexture(string path, string name) {
            Bitmap bitmap = LoadBitmap(path);
            bitmap.MakeTransparent(System.Drawing.Color.FromArgb(255, 0, 255));
            ConvertBitmapToTexture2D(bitmap, out Texture2D texture);
            if (texture == null) {
                throw new Exception("textureがnullです");
            }
            //dictBitmap[texture] = new TexData(texture, name, bitmap.Size.CastToPoint(), bitmap);
            return texture;
        }

        /// <summary>
        /// サイトから引用
        /// http://schroedinger.tea-nifty.com/blog/xna/index.html
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static void ConvertBitmapToTexture2D(Bitmap bitmap, out Texture2D texture) {
            BitmapData bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height)
                , ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            int pointSize = bitmapData.Stride / bitmap.Width;
            int bufferSize1 = bitmapData.Stride * bitmapData.Height;
            byte[] bytes1 = new byte[bufferSize1];
            Marshal.Copy(bitmapData.Scan0, bytes1, 0
                , bufferSize1);

            int bufferSize2 = bitmap.Width * bitmap.Height * 4;
            byte[] bytes2 = new byte[bufferSize2];
            for (int y = 0; y < bitmap.Height; y++) {
                for (int x = 0; x < bitmap.Width; x++) {
                    int adr2 = y * bitmap.Width * 4 + x * 4;
                    int adr1 = y * bitmapData.Stride + x * pointSize;
                    bytes2[adr2 + 0] = bytes1[adr1 + 2];//R
                    bytes2[adr2 + 1] = bytes1[adr1 + 1];//G
                    bytes2[adr2 + 2] = bytes1[adr1 + 0];//B
                    if (pointSize == 4) {
                        bytes2[adr2 + 3] = bytes1[adr1 + 3];//A
                    } else {
                        bytes2[adr2 + 3] = 0xff;//A
                    }
                }
            }

            texture = new Texture2D(device, bitmap.Width, bitmap.Height);
            texture.SetData(bytes2);
            bitmap.UnlockBits(bitmapData);
        }
    }
}
