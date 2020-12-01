using BitsCore.Debugging;
using BitsCore.Rendering.Display;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace BitsCore.BitsGUI
{
    public static class BitsGUIRenderer
    {
        public static Item[] FormatContainerTree(Container masterCont)
        {
            List<Item> returnItems = new List<Item>();

            //hardcode the values because this containers 'parent' is the screen (conceptually)
            Container screenCont = new Container(new ContainerSettings()); //default constructor as none of the values get used
            screenCont.containerAlignment = Alignment.TopLeft;
            screenCont.containerOrder = Order.FreeFloat;
            screenCont.containerOverflow = Overflow.Show;
            screenCont.xPosition = 0.0f;
            screenCont.yPosition = 0.0f;
            screenCont.width = 0f;
            screenCont.maxHeight = 0f;
            screenCont.minWidth = 0f;
            screenCont.height = 0f;
            screenCont.maxHeight = 0f;
            screenCont.minHeight = 0f;
            screenCont.screen_posX = 0;
            screenCont.screen_posY = 0;
            screenCont.screen_width = (int)DisplayManager.WindowSize.X;
            screenCont.screen_height = (int)DisplayManager.WindowSize.Y;

            BBug.Log("ScreenCont width: " + screenCont.screen_width);
            BBug.Log("ScreenCont height: " + screenCont.screen_height);
            
            returnItems.AddRange(masterCont.FormatContainer(screenCont));

            BBug.Log("MasterCont screen_posX: " + masterCont.screen_posX);
            BBug.Log("MasterCont screen_posY: " + masterCont.screen_posY);
            BBug.Log("MasterCont screen_width: " + masterCont.screen_width);
            BBug.Log("MasterCont screen_height: " + masterCont.screen_height);

            ////uses recursion in 'FormatContainer()' to format all containers
            //foreach (Container cont in masterCont.subContainers)
            //{
            //    returnItems.AddRange(cont.FormatContainer(masterCont));
            //}

            return returnItems.ToArray();
        }

        private static Item[] FormatContainer(this Container cont, Container parent)
        {
            //first calc width, height because the position-calcs alignment takes it into consideration

            #region WIDTH_&_HEIGHT
            //calc container width, height
            float contWidth = cont.width;
            if(cont.maxWidth > 0 && contWidth > cont.maxWidth)
            {
                contWidth = cont.maxWidth;
            }
            if (cont.minWidth > 0 && contWidth < cont.minWidth)
            {
                contWidth = cont.minWidth;
            }

            float contHeight = cont.height;
            if (cont.maxHeight > 0 && contHeight > cont.maxHeight)
            {
                contHeight = cont.maxHeight;
            }
            if (cont.minHeight > 0 && contHeight < cont.minHeight)
            {
                contHeight = cont.minHeight;
            }

            BBug.Log("ContWidth: " + contWidth + ", ContHeight: " + contHeight);

            cont.screen_width = (int)(parent.screen_width * contWidth);
            cont.screen_height = (int)(parent.screen_height * contHeight);

            BBug.Log("cont.screen_width: " + cont.screen_width + ", cont.screen_height: " + cont.screen_height);
            #endregion

            #region POSITION
            //calc x-, y-coords, based on the containers alignment

            //adjusts the x/y values so that the calc. for the items can all be made assuming the container is layed out from the top-left
            //the container always gets drawn with an 'anchor-point' in the top-left corner (just how opengl works)
            //because of this we shift the position of that top-left anchor to be where it would be if 
            //opengl instead drew its objects with the speicfied anchor so that when making the layout for ui you can simply choose where you want the anchor
            if (cont.containerAlignment == Alignment.TopLeft)
            {
                cont.screen_posX = (int)(parent.screen_width * cont.xPosition);
                cont.screen_posY = (int)(parent.screen_height * cont.yPosition);
            }
            //incorrect
            else if (cont.containerAlignment == Alignment.TopCenter)
            {
                cont.screen_posX = parent.screen_posX - (int)(parent.screen_width * .5f);
                cont.screen_posY = parent.screen_posY;
            }
            else if (cont.containerAlignment == Alignment.TopRight)
            {
                cont.screen_posX = parent.screen_posX - parent.screen_width;
                cont.screen_posY = parent.screen_posY;
            }
            else if (cont.containerAlignment == Alignment.MiddleLeft)
            {
                cont.screen_posX = parent.screen_posX;
                cont.screen_posY = parent.screen_posY - (int)(cont.screen_height * .5f);
            }
            else if (cont.containerAlignment == Alignment.MiddleCenter)
            {
                cont.screen_posX = parent.screen_posX - (int)(cont.screen_width * .5f);
                cont.screen_posY = parent.screen_posY - (int)(cont.screen_height * .5f);
            }
            else if (cont.containerAlignment == Alignment.MiddleRight)
            {
                cont.screen_posX = parent.screen_posX - cont.screen_width;
                cont.screen_posY = parent.screen_posY - (int)(cont.screen_height * .5f);
            }
            else if (cont.containerAlignment == Alignment.BottomLeft)
            {
                cont.screen_posX = parent.screen_posX;
                cont.screen_posY = parent.screen_posY - cont.screen_height;
            }
            else if (cont.containerAlignment == Alignment.BottomCenter)
            {
                cont.screen_posX = parent.screen_posX - (int)(cont.screen_width * .5f);
                cont.screen_posY = parent.screen_posY - cont.screen_height;
            }
            else if (cont.containerAlignment == Alignment.BottomRight)
            {
                cont.screen_posX = parent.screen_posX - cont.screen_width;
                cont.screen_posY = parent.screen_posY - cont.screen_height;
            }
            else { throw new System.Exception("ContainerAlignment not supported."); }

            BBug.Log("cont.screen_posX: " + cont.screen_posX + ", cont.screen_posY: " + cont.screen_posY);

            #endregion

            #region ORDER
            //adjust based on parent-order

            //if (parent.containerOrder == Order.FreeFloat)
            //stays the same, i.e. unaffected by parent
            if (parent.containerOrder == Order.Fill)
            {
                cont.screen_posX = parent.screen_posX;
                cont.screen_posY = parent.screen_posY;
                cont.screen_width = parent.screen_width;
                cont.screen_height = parent.screen_height;

            }
            else if (parent.containerOrder == Order.VerticalDescending)
            {
                int newPosY = cont.screen_posY + parent.hierachy_offsetY;
                parent.hierachy_offsetY = cont.screen_height;
                cont.screen_posY = newPosY;
            }
            //...
            #endregion

            List<Item> result = new List<Item>();
            foreach (Item item in cont.items)
            {
                item.FormatItem(cont);
                result.Add(item);
            }

            //use recursion to format all child-containers
            foreach (Container childCont in cont.subContainers)
            {
                result.AddRange(childCont.FormatContainer(cont));
            }

            return result.ToArray();
        }

        private static void FormatItem(this Item item, Container parent)
        {
            //just fills the entire container with the item
            item.screen_posX = parent.screen_posX;
            item.screen_posY = parent.screen_posY;
            item.screen_width = parent.screen_width;
            item.screen_height = parent.screen_height;

            BBug.Log("Item screen_posX: " + item.screen_posX);
            BBug.Log("Item screen_posY: " + item.screen_posY);
            BBug.Log("Item screen_width: " + item.screen_width);
            BBug.Log("Item screen_height: " + item.screen_height);
        }
    }
}
