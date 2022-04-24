using System;
using System.Collections.Generic;
using System.Text;
using EasySaveV2;


namespace Server
{
    /// <summary>
    /// Protocol to get the class and method name to execute
    /// </summary>
    class JSONRpc
    {
        public int Id;
        public string methodName;
        public string className;
        public string[] args;
        public JSONRpc(string c, string m, string[] a, int id)
        {
            args = a;
            methodName = m;
            className = c;
            Id = id;
        }
    }

    /// <summary>
    /// Class to create new response protcol
    /// </summary>
    class JsonResponse
    {
        public int id;
        public string response;
        public JsonResponse(string r, int i)
        {
            id = i;
            response = r;
        }
    }
}
