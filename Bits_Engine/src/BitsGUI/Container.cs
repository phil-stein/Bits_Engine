using BitsCore.Utils;
using System.Collections.Generic;

namespace BitsCore.BitsGUI
{
    public class ContainerSettings
    {
        public List<Container> subSontainer = new List<Container>();
        public List<Item> items = new List<Item>();

        public Alignment containerAlignment = Alignment.TopLeft;
        public Order containerOrder = Order.HorizontalDescending;
        public Overflow containerOverflow = Overflow.Show; //...

        public float width = 0f;
        public float maxWidth = 0f;
        public float minWidth = 0f;

        public float height = 0f;
        public float maxHeight = 0f;
        public float minHeight = 0f;

        public float xPosition = 0f;
        public float yPosition = 0f;

        public float xPadding = 0f;
        public float yPadding = 0f;
    }

    public class Container
    {

        public Container[] subContainers;
        public Item[] items;

        /// <summary> Defines whether the containers position is it's <see cref="Alignment.TopLeft"/>, <see cref="Alignment.MiddleRight"/>, etc. coordinate </summary>
        public Alignment containerAlignment;
        /// <summary> Defines how the containers sub-elements should be layed out, i.e. if it should be <see cref="Order.VerticalDescending"/>, <see cref="Order.Fill"/>, etc. </summary>
        public Order containerOrder;
        /// <summary>  </summary>
        public Overflow containerOverflow; //...

        /// <summary> The width of the container in %. </summary>
        public float width;
        /// <summary> The max-width of the container in %. </summary>
        public float maxWidth;
        /// <summary> The min-width of the container in %. </summary>
        public float minWidth;

        /// <summary> The height of the container in %. </summary>
        public float height;
        /// <summary> The max-height of the container in %. </summary>
        public float maxHeight;
        /// <summary> The min-height of the container in %. </summary>
        public float minHeight;

        /// <summary> The x-coord of the container in %. </summary>
        public float xPosition;
        /// <summary> The y-coord of the container in %. </summary>
        public float yPosition;

        public float xPadding;
        public float yPadding;

        //values used during rendering
        internal int hierachy_offsetX, hierachy_offsetY; //used for order
        internal int screen_posX, screen_posY; //position in pixel
        internal int screen_width, screen_height; //width, height in pixel

        public Container(ContainerSettings settings)
        {
            this.subContainers = settings.subSontainer.ToArray();
            this.items = settings.items.ToArray();

            this.containerAlignment = settings.containerAlignment;
            this.containerOrder = settings.containerOrder;
            this.containerOverflow = settings.containerOverflow;

            this.width = settings.width;
            this.maxHeight = settings.maxWidth;
            this.minWidth = settings.minWidth;

            this.height = settings.height;
            this.maxHeight = settings.maxHeight;
            this.minHeight = settings.minHeight;

            this.xPosition = settings.xPosition;
            this.yPosition = settings.yPosition;

            this.xPadding = settings.xPadding;
            this.yPadding = settings.yPadding;

            //values used during rendering
            hierachy_offsetX = hierachy_offsetY = 0;
            screen_posX = screen_posY = 0;
        }

        public void AddElement(Container container)
        {
            ArrayUtils.AddToEnd(subContainers, container);
        }
    }
}
