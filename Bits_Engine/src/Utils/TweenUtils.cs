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
        enum TweenType 
        { 
            Position, PosX, PosY, PosZ, 
            Rotation, // RotX, ... not yet implented
            Scale, // ScaleX, ... not yet implented
            Color 
        }
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

                // position tweens
                else if(t.type == TweenType.Position)
                {
                    t.gameObject.transform.position = MathUtils.Lerp(t.startState, t.goalState, (GameTime.TotalElapsedSeconds - t.startT) / t.duration);
                    BBug.Log("Tween %: '" + (GameTime.TotalElapsedSeconds - t.startT) / t.duration + "'");
                }
                else if (t.type == TweenType.PosX)
                {
                    t.gameObject.transform.position = MathUtils.Lerp(new Vector3(t.startState.X, t.gameObject.transform.position.Y, t.gameObject.transform.position.Z), new Vector3(t.goalState.X, t.gameObject.transform.position.Y, t.gameObject.transform.position.Z), (GameTime.TotalElapsedSeconds - t.startT) / t.duration);
                }
                else if (t.type == TweenType.PosY)
                {
                    t.gameObject.transform.position = MathUtils.Lerp(new Vector3(t.gameObject.transform.position.X, t.startState.Y, t.gameObject.transform.position.Z), new Vector3(t.gameObject.transform.position.X, t.goalState.Y, t.gameObject.transform.position.Z), (GameTime.TotalElapsedSeconds - t.startT) / t.duration);
                }
                else if (t.type == TweenType.PosZ)
                {
                    t.gameObject.transform.position = MathUtils.Lerp(new Vector3(t.gameObject.transform.position.X, t.gameObject.transform.position.Y, t.startState.Z), new Vector3(t.gameObject.transform.position.X, t.gameObject.transform.position.Y, t.goalState.Z), (GameTime.TotalElapsedSeconds - t.startT) / t.duration);
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

        /// <summary> 
        /// Moves a <see cref="GameObject"/> from one <see cref="Transform.position"/> to another. 
        /// <para> <paramref name="mirror"/> decides whether the tween will be played in reverse once the original motion is finished. </para>
        /// </summary>
        public static void TweenPos(GameObject go, Vector3 goal, float duration, bool mirror = false)
        {
            tweens.Add(new Tween(TweenType.Position, go, go.transform.position, goal, duration, GameTime.TotalElapsedSeconds, mirror));
        }
        /// <summary> 
        /// Moves a <see cref="GameObject"/> from one <see cref="Transform.position.X"/> to another. 
        /// <para> <paramref name="mirror"/> decides whether the tween will be played in reverse once the original motion is finished. </para>
        /// </summary>
        public static void TweenPosX(GameObject go, float goal, float duration, bool mirror = false)
        {
            tweens.Add(new Tween(TweenType.PosX, go, go.transform.position, new Vector3(goal, go.transform.position.Y, go.transform.position.Z), duration, GameTime.TotalElapsedSeconds, mirror));
        }
        /// <summary> 
        /// Moves a <see cref="GameObject"/> from one <see cref="Transform.position.Y"/> to another. 
        /// <para> <paramref name="mirror"/> decides whether the tween will be played in reverse once the original motion is finished. </para>
        /// </summary>
        public static void TweenPosY(GameObject go, float goal, float duration, bool mirror = false)
        {
            tweens.Add(new Tween(TweenType.PosY, go, go.transform.position, new Vector3(go.transform.position.X, goal, go.transform.position.Z), duration, GameTime.TotalElapsedSeconds, mirror));
        }
        /// <summary> 
        /// Moves a <see cref="GameObject"/> from one <see cref="Transform.position.Z"/> to another. 
        /// <para> <paramref name="mirror"/> decides whether the tween will be played in reverse once the original motion is finished. </para>
        /// </summary>
        public static void TweenPosZ(GameObject go, float goal, float duration, bool mirror = false)
        {
            tweens.Add(new Tween(TweenType.PosZ, go, go.transform.position, new Vector3(go.transform.position.X, go.transform.position.Y, goal), duration, GameTime.TotalElapsedSeconds, mirror));
        }
    }
}
