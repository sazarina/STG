using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using STG.Engine.Helper;
using STG.Engine.Debugging;
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

        /// <summary>
        /// 上限数。0の場合は全て読み込む。
        /// </summary>
        public int numAll;

        /// <summary>
        /// 1個当たりのサイズ
        /// </summary>
        public Size Size;

        /// <summary>
        /// スプライト間の間隔
        /// </summary>
        public Point Spacing;

        public List<SpriteTexture> SpriteTextures = new List<SpriteTexture>();

        /// <summary>
        /// Texture2Dを取得する。indexは左上から右に向かって0から始まる。例えば、NumXが4の場合、1行目は0,1,2,3、2行目は4,5,6,7となる。
        /// </summary>
        public Texture2D GetTexture(int index) {
            return SpriteTextures[index].texture;
        }

        /// <summary>
        /// テクスチャとその位置を保持するクラス。位置はスプライトシート内の座標で、左上が(0,0)となる。
        /// </summary>
        public class SpriteTexture {
            public Texture2D texture;
            public Point point;
            public SpriteTexture(Texture2D texture, int x, int y) {
                this.texture = texture;
                point = new Point(x, y);
            }

            public void Dispose() {
                texture.Dispose();
            }
        }

        /// <summary>
        /// コンストラクタ。ビットマップからスプライトシートを作成する。
        /// </summary>
        /// <param name="path">パス</param>
        /// <param name="NumX">横の数</param>
        /// <param name="NumY">縦の数</param>
        /// <param name="spriteSize">1個当たりのサイズ</param>
        /// <param name="pad">パディング</param>
        /// <param name="spacing">スプライト間の間隔</param>
        /// <param name="numAll">読み込むスプライトの上限。0の場合は全て読み込む。</param>
        public SpriteSheet(string path, string name, int NumX, int NumY, Point spriteSize, Point pad = new Point(), Point spacing = new Point(), int numAll = 0) {
            this.NumX = NumX;
            this.NumY = NumY;
            this.numAll = numAll;
            Size = spriteSize.CastToSize();
            Spacing = spacing;

            Bitmap bmp = GraphicsUltis.LoadBitmap(path);
            LoadSpriteSheet(bmp, pad, spacing);
        }

        /// <summary>
        /// コンストラクタ。ビットマップからスプライトシートを作成する。
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        /// <param name="name">スプライトシートの名前 *今は未使用</param>
        /// <param name="NumX">横の数</param>
        /// <param name="NumY">縦の数</param>
        /// <param name="spriteSize">1個当たりのサイズ</param>
        /// <param name="padX">X方向のパディング</param>
        /// <param name="padY">Y方向のパディング</param>
        /// <param name="numAll">読み込むスプライトの上限。0の場合は全て読み込む。</param>
        public SpriteSheet(Bitmap bitmap,string name, int NumX, int NumY, Point spriteSize, Point pad = new Point(), Point spacing = new Point(), int numAll = 0) {
            this.NumX = NumX;
            this.NumY = NumY;
            this.numAll = numAll;
            this.Size = spriteSize.CastToSize();

            LoadSpriteSheet(bitmap, pad, spacing);
        }

        public SpriteSheet() { }

        /// <summary>
        /// // スプライトシートを読み込む。スプライトシートは、NumXとNumYで指定された数だけ、spriteSizeのサイズで分割される。
        /// </summary>
        /// <param name="bmp">ビットマップ</param>
        /// <param name="padX">X方向のパディング</param>
        /// <param name="padY">Y方向のパディング</param>
        void LoadSpriteSheet(Bitmap bmp, Point pad,Point spacing) {
            int count = 0;

            for (int y = 0; y < NumY; y++) {
                for (int x = 0; x < NumX; x++) {
                    Point pos = new Point(pad.X + (Size.Width + spacing.X) * x, pad.Y + (Size.Height + spacing.Y) * y);
                    var rect = new Rectangle(pos.CastToPoint(), Size);

                    if (bmp.Width <= pos.X) { break; }
                    if (bmp.Height <= pos.Y) { return; }

                    //numAllが0のときは全て読み込み、numAllが0以外のときは上限まで読み込む
                    if (numAll == 0 || count < numAll) {
                        Bitmap bitmap = bmp.Clone(rect, PixelFormat.Format32bppArgb);
                        bitmap.MakeTransparent(Color.FromArgb(255, 0, 255));

                        GraphicsUltis.ConvertBitmapToTexture2D(bitmap, out Texture2D texture);
                        
                        //不要になったBitmapは解放する
                        bitmap.Dispose();

                        var spriteTexture = new SpriteTexture(texture, x, y);
                        SpriteTextures.Add(spriteTexture);
                    }
                    count++;

                    Debug.Log($"count:{count} {rect}");
                }
            }
            
            Debug.Log(":" + SpriteTextures.Count);
        }

        /// <summary>
        /// スプライトシートを縦ごとに分割して、複数のスプライトシートを作成する。
        /// </summary>
        /// <returns></returns>
        public SpriteSheet[] SplitSheet() {
            SpriteSheet[] spriteSheets = new SpriteSheet[NumY];
            for (int y = 0; y < NumY; y++) {
                var sheet = new SpriteSheet() {
                    NumX = NumX,
                    NumY = 1,
                    Size = Size
                };

                for (int x = 0; x < NumX; x++) {
                    var index = x + y * NumX;
                    var sprite = SpriteTextures[index];

                    sheet.SpriteTextures.Add(new SpriteTexture(sprite.texture, x, y));
                }
                
                spriteSheets[y] = sheet;
            }

            return spriteSheets;
        }

        /// <summary>
        /// すべてのテクスチャを配列で取得する。
        /// </summary>
        /// <returns></returns>
        public Texture2D[] GetTextureArray() {
            List<Texture2D> texList = new List<Texture2D>();
            for (int i = 0; i < SpriteTextures.Count; i++) {
                SpriteTexture texdata = SpriteTextures[i];
                texList.Add(texdata.texture);
            }

            return texList.ToArray();
        }


        /// <summary>
        /// アンマネージドリソースを解放する。SpriteSheetクラスはTexture2Dを保持しているため、Disposeメソッドを呼び出して、すべてのテクスチャを解放する必要がある。
        /// </summary>
        public void Dispose() {
            foreach (var texture in SpriteTextures) {
                texture.Dispose();
            }

            SpriteTextures.Clear();
        }
    }
}
