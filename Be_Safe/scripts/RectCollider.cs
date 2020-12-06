using BitsCore.Debugging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe
{
    [Flags]
    public enum CollisionDir : short
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8,
    }
    public class Collision
    {
        public float xOverlap = 0f;
        public float yOverlap = 0f;

        public Collision(float xOverlap, float yOverlap)
        {
            this.xOverlap = xOverlap;
            this.yOverlap = yOverlap;
        }
    }


    public class RectCollider
    {

        public float xPos = 0f;
        public float yPos = 0f;
        
        public float width = 0f;
        public float height = 0f;

        public RectCollider(float xPos, float yPos, float width, float height)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Doesn't take rotation into account, aka it's Axis-Aligned.
        /// </summary>
        public static Collision CheckCollision(RectCollider rect_one, RectCollider rect_two)
        {

            if (OneD_LineOverlap(rect_one.xPos, rect_one.xPos + rect_one.width, rect_two.xPos, rect_two.xPos + rect_two.width) > 0f && OneD_LineOverlap(rect_one.yPos, rect_one.yPos + rect_one.height, rect_two.yPos, rect_two.yPos + rect_two.height) > 0f)
            {
                return new Collision(
                     OneD_LineOverlap(rect_one.xPos, rect_one.xPos + rect_one.width, rect_two.xPos, rect_two.xPos + rect_two.width),
                     OneD_LineOverlap(rect_one.yPos, rect_one.yPos + rect_one.height, rect_two.yPos, rect_two.yPos + rect_two.height)
                     ); ;
                
            }
            return new Collision(0f, 0f);
        }

        /// <summary>
        /// Get the overlap between two 1D lines.
        /// <para> Taken from: https://stackoverflow.com/questions/16691524/calculating-the-overlap-distance-of-two-1d-line-segments </para>
        /// </summary>
        public static float OneD_LineOverlap(float startOne, float endOne, float startTwo, float endtwo)
        {
            return Math.Max(0, Math.Min(endOne, endtwo) - Math.Max(startOne, startTwo));
        }

    }
}
