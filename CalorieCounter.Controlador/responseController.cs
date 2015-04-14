using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace CalorieCounter.Controlador
{
    public static class responseController
    {
        public static Object CheckJson(Object ObjResponse)
        {
            var JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 8000000;
            var JS =  HttpContext.Current.Request["JS"] ?? "true";
            var JSP = HttpContext.Current.Request["JSP"] ?? "true";


            string R = JSS.Serialize(ObjResponse);
            R = JSP.Contains("true") ? (HttpContext.Current.Request["callback"] + "(" + R + ");") : (R);
            HttpContext.Current.Response.AddHeader("Content-Type", "application/javascript");
            HttpContext.Current.Response.Write(R);
            HttpContext.Current.Response.End();
            return null;

        }
    }
}
