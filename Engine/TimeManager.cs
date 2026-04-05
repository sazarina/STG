using Microsoft.Xna.Framework;
using System;

namespace STG.Engine {
    public static class TimeManager {
        public static GameTime gameTime { get; private set; } = new GameTime();

        public static void Initialize() {

        }

        public static void Update(GameTime gameTime) {
            TimeManager.gameTime = gameTime;
        }

        public static void Draw(GameTime gameTime) {
            TimeManager.gameTime = gameTime;
        }

        public static TimeSpan prevTime = new TimeSpan();

        public static void SetPrevTime() => prevTime = total;

        public static TimeSpan GetDeltaTime() => total-prevTime;


        public static TimeSpan total => gameTime.TotalGameTime;
        /// <summary>
        /// Gets the total seconds of the last update
        /// </summary>
        public static float totalSeconds => (float)total.TotalSeconds;
        /// <summary>
        /// Gets the total milliseconds of the last update 
        /// </summary>
        public static float totalMilliseconds => (float)total.TotalMilliseconds;
        /// <summary>
        /// How much time has elapsed on the last frame
        /// </summary>
        public static TimeSpan elapsed => gameTime.ElapsedGameTime;

        public static float deltaTime => (float)elapsed.TotalSeconds;

    }
}
