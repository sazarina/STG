using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using STG.Engine.Helper;
using static STG.Engine.Graphics.GraphicsUltis;
using _Debug = STG.Engine.Debug.Debug;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = System.Drawing.Rectangle;

namespace STG.Engine.Graphics {
    public class SpriteSheet : IDisposable {
        /// <summary>
        /// 横の数
        /// </summary>
        public int NumX;
        /// <summary>
        /// 縦の数
        /// </summary>
        public int NumY;
        public int numAll;


        /// <summary>
        /// 1個当たりのサイズ
        /// </summary>
        public Size spriteSize;
        /// <summary>
        /// <para>Textures[y][x]のように使う</para>
        /// <para>SpriteSheetを分割してSpriteSheetを作った場合はy=0とする</para>
        /// </summary>
        public List<List<Texture2D>> Textures = new List<List<Texture2D>>();

        public List<Texture2D_Data> Texture2D = new List<Texture2D_Data>();

        public Texture2D GetTexture2D(int index) {
            return Texture2D[index].texture;
        }

        public class Texture2D_Data {
            public Texture2D texture;
            public Point point;
            public Texture2D_Data(Texture2D texture, int x, int y) {
                this.texture = texture;
                point = new Point(x, y);
            }
        }

        List<List<Bitmap>> bitmaps = new List<List<Bitmap>>();

        GraphicsDevice device;
        public SpriteSheet(string path, string name, int NumX, int NumY, Point spriteSize, int padX = 0, int padY = 0, int numAll = 1000) {
            device = LoadHelper.Device;
            this.NumX = NumX;
            this.NumY = NumY;
            this.numAll = numAll;
            this.spriteSize = spriteSize.CastToSize();

            Bitmap bmp = LoadBitmap(path);
            SplitTexture(bmp, padX, padY);
            LoadTextures(name);
        }

        public SpriteSheet(Bitmap bitmap, string name, int NumX, int NumY, Point spriteSize, int padX = 0, int padY = 0, int numAll = 1000) {
            device = LoadHelper.Device;
            this.NumX = NumX;
            this.NumY = NumY;
            this.numAll = numAll;
            this.spriteSize = spriteSize.CastToSize();

            SplitTexture(bitmap, padX, padY);
            LoadTextures(name);
        }

        public SpriteSheet() { }

        public void SplitTexture(Bitmap bmp, int padX, int padY) {
            int count = 0;

            for (int y = 0; y < NumY; y++) {
                List<Bitmap> bmplst = new List<Bitmap>();
                for (int x = 0; x < NumX; x++) {
                    Point pos = new Point(padX + spriteSize.Width * x, padY + spriteSize.Height * y);


                    var rect = new Rectangle(pos.CastToPoint(), spriteSize);
                    if (bmp.Width <= pos.X) { break; }
                    if (bmp.Height <= pos.Y) { return; }

                    Bitmap bitmap = bmp.Clone(rect, PixelFormat.Format32bppArgb);
                    bitmap.MakeTransparent(Color.FromArgb(255, 0, 255));

                    if (count < numAll) {

                        if (numAll != 0)
                            bmplst.Add(bitmap);
                    }

                    count++;

                    _Debug.Log($"count:{count} {rect}");
                }
                bitmaps.Add(bmplst);
            }
        }

        public void LoadTextures(string name) {
            Texture2D_Data[] textures = new Texture2D_Data[NumX * NumY];

            for (int y = 0; y < bitmaps.Count; y++) {
                List<Texture2D> lst = new List<Texture2D>();
                for (int x = 0; x < bitmaps[y].Count; x++) {
                    ConvertBitmapToTexture2D(bitmaps[y][x], out Texture2D texture);

                    textures[x + bitmaps[y].Count * y] = new Texture2D_Data(texture, x, y);
                    dictBitmap[texture] = new TexData(texture, $"{name}[{x + 1}][{y + 1}]", bitmaps[y][x].Size.CastToPoint(), bitmaps[y][x]);
                    lst.Add(texture);
                }
                Textures.Add(lst);
            }
            Texture2D.AddRange(textures);
            _Debug.Log(":" + Texture2D.Count);
        }

        public SpriteSheet[] SplitSheet() {
            SpriteSheet[] spriteSheets = new SpriteSheet[NumY];
            for (int y = 0; y < NumY; y++) {
                spriteSheets[y] = new SpriteSheet();
                List<List<Texture2D>> tex = new List<List<Texture2D>> {
                    new List<Texture2D>()
                };
                for (int x = 0; x < NumX; x++) {
                    tex[0].Add(Textures[y][x]);
                    spriteSheets[y].Texture2D.Add(new Texture2D_Data(tex[0][x], x, y));
                }
                spriteSheets[y].Textures = tex;

                spriteSheets[y].NumX = NumX;
                spriteSheets[y].NumY = 1;
                spriteSheets[y].spriteSize = spriteSize;
            }

            return spriteSheets;
        }

        public Texture2D[] GetTextureArray() {
            List<Texture2D> texList = new List<Texture2D>();
            for (int i = 0; i < Texture2D.Count; i++) {
                Texture2D_Data texdata = Texture2D[i];
                texList.Add(texdata.texture);
            }

            return texList.ToArray();
        }


        public void Dispose() {

        }
    }
}
