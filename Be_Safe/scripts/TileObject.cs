using BitsCore.ObjectData.Components;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BeSafe.Scripts
{
    public abstract class TileObject : ScriptComponent
    {

        protected int curPosition = 0;
        protected int xPos; // private as the are only correct directly after a call to UpdateCurTilePos()
        protected int zPos; // private as the are only correct directly after a call to UpdateCurTilePos()


        public TileObject(int _curPosition)
        {
            this.curPosition = _curPosition;
        }


        /// <summary> Update the placement of the player-char. </summary>
        public void UpdateCurTilePos()
        {
            xPos = ((curPosition / EnvController.tileColumns) % EnvController.tileRows) * EnvController.tileDist;
            zPos = (curPosition % EnvController.tileColumns) * EnvController.tileDist;

            gameObject.transform.position = new Vector3(xPos, gameObject.transform.position.Y, zPos);
        }

        public virtual void Move(Direction dir)
        {
            SetTileIndex(dir);
            UpdateCurTilePos();
        }
        internal void SetTileIndex(Direction dir)
        {
            if (dir == Direction.Left)
            {
                curPosition = CheckMove(curPosition - 1);
                curPosition = curPosition < 0 ? 0 : curPosition;
            }
            else if (dir == Direction.Right)
            {
                curPosition = CheckMove(curPosition + 1);
                curPosition = curPosition > EnvController.tileColumns * EnvController.tileRows - 1 ? EnvController.tileColumns * EnvController.tileRows - 1 : curPosition;
            }
            else if (dir == Direction.Up)
            {
                curPosition = CheckMove(curPosition + EnvController.tileColumns);
                curPosition = curPosition > EnvController.tileColumns * EnvController.tileRows - 1 ? curPosition - EnvController.tileColumns : curPosition;
            }
            else if (dir == Direction.Down)
            {
                curPosition = CheckMove(curPosition - EnvController.tileColumns);
                curPosition = curPosition < 0 ? curPosition + EnvController.tileColumns : curPosition;
            }
        }

        /// <summary> Check if the new position is out of bounds or a un-walkable tile. </summary>
        /// <param name="newPos"> The position be checked for walk-ability. </param>
        protected int CheckMove(int newPos)
        {
            // out of bounds ----------------------------------
            // trying to move to the left
            if (curPosition > newPos && (curPosition - newPos) == 1 && curPosition % EnvController.tileColumns == 0)
            {
                return curPosition;
            }
            // trying to move to the right
            if (curPosition < newPos && (newPos - curPosition) == 1 && (curPosition + 1) % EnvController.tileColumns == 0)
            {
                return curPosition;
            }

            if (!EnvController.IsWalkableTile(curPosition, newPos))
            {
                return curPosition;
            }
            return newPos;
        }
    }
}
