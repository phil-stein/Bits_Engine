using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BitsCore.BitsGUI
{
    public class Element
    {
        //container for bitsgui.item
        public struct ElemSettings
        {
            public List<Item> items;

            public Order elementOrder;
            public Overflow elementOverflow;

            public float width;
            public float height;

            public float padding;
        }

        public List<Item> items;

        public Order elementOrder;
        public Overflow elementOverflow;

        public float width;
        public float height;
        
        public float padding;

        public Element(ElemSettings settings)
        {
            this.items = settings.items;
            this.elementOrder = settings.elementOrder;
            this.elementOverflow = settings.elementOverflow;
            this.width = settings.width;
            this.height = settings.height;
            this.padding = settings.padding;
        }
    }
}
