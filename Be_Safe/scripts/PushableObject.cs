﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Scripts
{
    public class PushableObject : TileObject
    {
        public PushableObject(int _curPosition) : base(_curPosition)
        {
            type = TileObjectType.Pushable;
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}
