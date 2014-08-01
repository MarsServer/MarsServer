using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public class PublicZone : Room
    {
        public static readonly PublicZone instance = new PublicZone();
    }
}
