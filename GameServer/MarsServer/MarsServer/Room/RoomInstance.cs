﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class RoomInstance : Room
    {
        public static readonly RoomInstance instance = new RoomInstance();
    }
}
