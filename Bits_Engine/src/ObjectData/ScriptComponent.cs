using BitsCore.Events_Input;

namespace BitsCore.ObjectData.Components
{
    /// <summary> Override this class to generate <see cref="Component"/> types to attach to any <see cref="GameObject"/>. </summary>
    [System.Serializable]
    public abstract class ScriptComponent : Component
    {
        public ScriptComponent()
        {
        }

        /// <summary> Gets called at the start of the <see cref="Application"/> run-cycle. </summary>
        public virtual void OnStart() { }

        /// <summary> Gets called once each frame. </summary>
        public virtual void OnUpdate() { }

        /// <summary> Gets called when the <see cref="ScriptComponent"/> is detached or it's <see cref="GameObject"/> is destroyed. </summary>
        public virtual void OnDestroy() { }


        internal override void OnAdd()
        {
            //should init already have been called
            if (EventManager.startPassed) { OnStart(); }
            else { EventManager.calledStart += OnStart; }
            
            EventManager.calledUpdate += OnUpdate;
        }

        internal override void OnRemove()
        {
            OnDestroy();
        }


    }
}
