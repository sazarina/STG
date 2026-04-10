using Microsoft.Xna.Framework;
using System.Linq;
using static STG.Engine.Graphics.GraphicsUltis;

namespace STG.Engine.Graphics {
    public class Animation {
        public SpriteSheet spriteSheet;

        Vector2 sheetSize = new Vector2();

        //Timer timer;
        int currentFrameX;
        float timer;
        public float interval;

        bool isAnim = false;

        bool isWait = false;

        public Animation(SpriteSheet spriteSheet, float interval, bool isWait = false) {
            this.spriteSheet = spriteSheet;
            this.interval = interval;
            this.isWait = isWait;

            //timer = new Timer();
            //timer.Start();
        }

        public void RunAnim() {
            isAnim = true;
        }



        public void SetInterval(int interval) {
            this.interval = interval;
        }

        public void SetWait(bool isWait) {
            this.isWait = isWait;
        }

        public bool Update() {
            bool isFinished = false;
            timer++;
            if (timer % interval == 0 && isAnim) {
                currentFrameX++;
                if (currentFrameX > spriteSheet.SpriteTextures.Count() - 3) {
                    currentFrameX = 0;

                    if (!isWait) {
                        isAnim = false;
                        isFinished = true;
                    }
                    //currentFrame.Y++;
                    //if (currentFrame.Y >= spriteSheet.NumY) {
                    //    currentFrame.Y = 0;
                    //    isAttack = false;
                    //}
                }
            }
            return isFinished;
        }

        public void Draw(int x, int y, Vector2 center = default) {
            if (center == default) {
                center = new Vector2(0, 0);
            }

            DrawSprite(spriteSheet.GetTexture(currentFrameX), x, y, center);
        }

        public void Draw(Vector2 position, Vector2 center = default) {
            if (center == default) {
                center = new Vector2(0, 0);
            }
            DrawSprite(spriteSheet.GetTexture(currentFrameX), (int)position.X, (int)position.Y, center);
        }
        public void Draw(Vector2 position, int x, int y) {
            var center = new Vector2(x, y);

            DrawSprite(spriteSheet.GetTexture(currentFrameX), (int)position.X, (int)position.Y, center);
        }
    }
}