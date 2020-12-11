using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace BitsCore.ObjectData.Components
{
    [System.Serializable]
    public abstract class Component
    {
        /// <summary> The GameObject the Component is attached to. </summary>
        public GameObject gameObject { get; protected set; }

        /// <summary> The byte that identifies this component in de-/serialization. </summary>
        public byte ID { get; protected set; }



        /// <summary> Generates a Component attached to gameObject. </summary>
        public Component()
        {
            this.ID = GetCompID(this.GetType());
            
        }

        /// <summary> 
        /// Gets called when the component is added to a GameObject. Sets the 'gameObject' variable and calls OnAdd(). 
        /// <para> Don't override unless you know what you are doing. </para>
        /// </summary>
        internal virtual void OnAddEvent(GameObject _gameObject)
        {
            //Debug.WriteLine("OnAddEvent() called on: " + _gameObject.ToString());
            gameObject = _gameObject;
            
            OnAdd();
        }
        /// <summary> Gets called when the component is added to a GameObject. </summary>
        internal abstract void OnAdd();

        /// <summary> 
        /// Gets called when the component is removed from a GameObject. 
        /// <para> Don't override unless you know what you are doing. </para>
        /// </summary>
        internal virtual void OnRemoveEvent()
        {
            //in case anything needs to be done to all components on remove
            OnRemove();
        }
        /// <summary> Gets called when the component is removed from its GameObject. </summary>
        internal abstract void OnRemove();

        /// <summary> 
        /// Gets the Component-ID of the given Component-Type.
        /// <para>One static functions to declare the ID's for each component type in so that they are set in one place. </para>
        /// </summary>
        /// <param name="T"> The Type of the Component. </param>
        public static byte GetCompID(Type T)
        {
            byte b = 0;
            if (T == typeof(Transform))
            {
                b = 1;
            }
            else if(T == typeof(Mesh))
            {
                b = 2;
            }
            else if (T == typeof(Billboard))
            {
                b = 3;
            }
            else if (T == typeof(RandomHeight))
            {
                b = 4;
            }

            //Debug.WriteLine("Component.GetCompID(): " + T.Name + ", ID: " + b.ToString());
            return b;
        }

        /// <summary> Gets the Component-Type of the given Component-ID. </summary>
        /// <param name="id"> The ID of the desired Component-Type. </param>
        public static Type GetCompByID(byte id)
        {
            if (id == GetCompID(typeof(Transform)))
            {
                return typeof(Transform);
            }
            else if (id == GetCompID(typeof(Mesh)))
            {
                return typeof(Mesh);
            }
            else if (id == GetCompID(typeof(Billboard)))
            {
                return typeof(Billboard);
            }
            else if (id == GetCompID(typeof(RandomHeight)))
            {
                return typeof(RandomHeight);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! Comp-Type '" + id.ToString() + "' doesn't have a defined Type !!!");
                return null;
            }
        }
    }
}
