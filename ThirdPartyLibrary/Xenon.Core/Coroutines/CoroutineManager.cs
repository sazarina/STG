using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xenon.Core.Coroutines {
    /// <summary>
    /// Manages active co routines
    /// </summary>
    public class CoroutineManager : GameComponent, ICoroutineManager {
        private readonly IList<RoutineHandle> _routines;

        /// <summary>
        /// Initializes a new instance of <see cref="CoroutineManager"/>
        /// </summary>
        public CoroutineManager() {
            _routines = new List<RoutineHandle>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Run(IEnumerable source) {
            var handler = new RoutineHandle(source);
            _routines.Add(handler);
            handler.Step();
        }

        /// <summary>
        /// Updates this <see cref="GameComponent"/>
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            _routines.ForEach(routineHandle => {
                routineHandle.Update(gameTime); 
            });

            var handleLists = _routines.Where(handle => handle.Done).ToList();

            for (int i = 0; i < handleLists.Count(); i++) {
                _routines.Remove(handleLists[i]);
            }

            //foreach (var handle in _routines.Where(handle => handle.Done)) {
            //    _routines.Remove(handle);
            //}
        }
    }
}