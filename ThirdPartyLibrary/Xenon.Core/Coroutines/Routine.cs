using XNA = Microsoft.Xna.Framework;
using System;

namespace Xenon.Core.Coroutines {
    /// <summary>
    /// A sub routine
    /// </summary>
    public abstract class Routine {
        /// <summary>
        /// Executes the routine
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Updates the routine
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(XNA.GameTime gameTime) { }

        /// <summary>
        /// Gets if this coroutine has finished
        /// </summary>
        public bool Done { get; protected set; }
    }
}
