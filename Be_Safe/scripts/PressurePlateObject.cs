using BitsCore.ObjectData;
using BitsCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Scripts
{
    public class PressurePlateObject : TileObject
    {
        public enum PressurePlateType { Door }

        PressurePlateType pressurePlateType;

        bool isAct = false; // whether the plate is beeing pressed down or not 

        GameObject objToInfluence; // the object to be affected on activation

        public PressurePlateObject(int _curPosition, PressurePlateType _pressurePlateType, GameObject _objToInfluence) : base(_curPosition)
        {
            this.type = TileObjectType.PressurePlate;
            this.pressurePlateType = _pressurePlateType;
            this.objToInfluence = _objToInfluence;
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate()
        {
        }

        public override bool Interact(Direction dir)
        {
            Activate();
            return true;
        }
        private void Activate()
        {
            if(isAct) { return; }
            isAct = true;

            if(pressurePlateType == PressurePlateType.Door)
            {
                TweenUtils.TweenPosY(objToInfluence, 2f, 0.2f);
            }

            // more coming prob.
        }
        private void Deactivate()
        {
            if(!isAct) { return; }
            isAct = false;

            if (pressurePlateType == PressurePlateType.Door)
            {
                TweenUtils.TweenPosY(objToInfluence, -2f, 0.2f);
            }
        }
    }
}
