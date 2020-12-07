using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PolyTest.Services
{
    public class HttpService
    {
        // < ---- GET request to URL ----> //
        public Stream GET(string url, string userAgent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = userAgent;
            request.ContentType = "application/json";
            WebResponse response = request.GetResponse();
            return response.GetResponseStream();
        }
    }
}
