using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarsServer
{
    public  class Tool
    {
        public static void Copy<T>(T oNewObject, T tObj)
        {
            for (int i = 0; i < tObj.GetType().GetProperties().Length; i++)
            {
                PropertyInfo oOldObjectProperty = (PropertyInfo)tObj.GetType().GetProperties().GetValue(i);
                Object oOldObjectValue = oOldObjectProperty.GetValue(tObj, null);

                PropertyInfo oNewObjectProperty = (PropertyInfo)oNewObject.GetType().GetProperties().GetValue(i);

                if (oOldObjectProperty.CanRead)
                {
                    oNewObjectProperty.SetValue(oNewObject, oOldObjectValue, null);
                }
            }

            //return oNewObject;
        }   
    }
}
