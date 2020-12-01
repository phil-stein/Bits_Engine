using BitsCore.ObjectData;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.BitsGUI
{
    public abstract class Item
    {
        public Vector3 color;
        public Texture texture;

        /// <summary> The width of the item in %. </summary>
        public float width;
        /// <summary> The max-width of the item in %. </summary>
        public float maxWidth;
        /// <summary> The min-width of the item in %. </summary>
        public float minWidth;

        /// <summary> The height of the item in %. </summary>
        public float height;
        /// <summary> The max-height of the item in %. </summary>
        public float maxHeight;
        /// <summary> The min-height of the item in %. </summary>
        public float minHeight;

        /// <summary> The x-coord of the item in %. </summary>
        public float xPosition;
        /// <summary> The y-coord of the item in %. </summary>
        public float yPosition;

        public float xPadding;
        public float yPadding;

        //values used during rendering
        internal int hierachy_offsetX, hierachy_offsetY; //used for order
        internal int screen_posX, screen_posY; //position in pixel
        internal int screen_width, screen_height; //width, height in pixel

        protected Item()
        {
            this.texture = null;
            this.color = Vector3.Zero;
            
            this.width = 0;
            this.height = 0;
            this.xPosition = 0f;
            this.yPosition = 0f;
            this.xPadding = 0f;
            this.yPadding = 0f;
        }
    }
}
