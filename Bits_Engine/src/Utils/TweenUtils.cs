using BitsCore.ObjectData.Components;
using BitsCore.Events;
using System.Numerics;
using System.Text;
using BitsCore.ObjectData;
using System.Collections.Generic;
using BitsCore.Debugging;

namespace BitsCore.Utils
{
    public static class TweenUtils
    {
        enum TweenType { Position, Rotation, Scale, Color }
        class Tween
        {
            public TweenType   type;
            public GameObject  gameObject;
            public Vector3     startState;
            public Vector3     goalState;
            public float       duration;
            public float       startT;
            public bool        mirror; // @TODO: implement // plays the tween in reverse after the initial tween has finished

            public Tween(TweenType _type, GameObject _gameObject, Vector3 _startState, Vector3 _goalState, float _duration, float _startT, bool _mirror = false)
            {
                this.type       = _type;
                this.gameObject = _gameObject;
                this.startState = _startState;
                this.goalState  = _goalState;
                this.duration   = _duration;
                this.startT     = _startT;
                this.mirror     = _mirror;
            }
        }

        static List<Tween>  tweens = new List<Tween>();
        static List<Tween> finishedTweens = new List<Tween>(); 

        /// <summary> 
        /// Initialize the TweenUtils class. 
        /// Without this call the classes functions don't work. 
        /// </summary>
        public static void Init()
        {
            EventManager.calledUpdate += OnUpdate; // subsribe the OnUpdate function to the actual update event
        }
        static void OnUpdate()
        {
            foreach (Tween t in tweens)
            {
                // terminate finished tweens
                if (GameTime.TotalElapsedSeconds >= t.startT + t.duration)
                {
                    finishedTweens.Add(t); // add to the list, to be terminated later
                    continue;
                }

                else if(t.type == TweenType.Position)
                {
                    t.gameObject.transform.position = MathUtils.Lerp(t.startState, t.goalState, (GameTime.TotalElapsedSeconds - t.startT) / t.duration);
                    BBug.Log("Tween %: '" + (GameTime.TotalElapsedSeconds - t.startT) / t.duration + "'");
                }
            }

            // remove the finished tweens
            foreach(Tween t in finishedTweens)
            {
                if(t.mirror)
                {
                    // add the mirrored tween
                    tweens.Add(new Tween(t.type, t.gameObject, t.gameObject.transform.position, t.startState, t.duration, GameTime.TotalElapsedSeconds, false));
                }
                tweens.Remove(t);
            }
            finishedTweens.Clear();
            
        }
        public static void TweenPos(GameObject go, Vector3 goal, float duration, bool mirror = false)
        {
            tweens.Add(new Tween(TweenType.Position, go, go.transform.position, goal, duration, GameTime.TotalElapsedSeconds, mirror));
        }

    }
}
