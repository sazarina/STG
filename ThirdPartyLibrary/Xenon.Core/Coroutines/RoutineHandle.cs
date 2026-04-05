using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Xenon.Core.Coroutines {
    internal class RoutineHandle {
        public IEnumerator routines { get; private set; }

        public RoutineHandle(IEnumerable routines) {
            this.routines = routines.GetEnumerator();
        }

        public void Update(GameTime gameTime) {

            // maybe do some nifty type detection here 
            // float values are total seconds
            // bool value of false should halt coroutine execution
            // int value skips x number of frames
            var routine = routines.Current as Routine;

            if (routine == null || routine.Done)
                Step();
            else
                routine.Update(gameTime);
        }

        public void Step() {
            if (routines.MoveNext()) {
                var routine = routines.Current as Routine;
                if (routine != null)
                    routine.Execute();
            }
        }

        public bool Done {
            get { return !routines.MoveNext(); }
        }

    }
}