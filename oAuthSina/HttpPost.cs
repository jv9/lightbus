using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace LeoShi.Soft.OpenSinaAPI
{
    public class HttpPost : BaseHttpRequest
    {
        private const string POST = "POST";
        private const string ContentEncoding = "iso-8859-1";

        public override string Request(string uri, string postData)
        {
            var appendUrl = AppendPostDataToUrl(postData, uri);
            string outUrl;
            var querystring= AppendSignatureString(POST, appendUrl, out outUrl);
            return WebRequest(POST, outUrl, querystring);
        }

        private  string WebRequest(string method, string url, string postData)
        {
            var httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = method;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            SetRemoteIPInHeader(httpWebRequest);
            return GetHttpWebResponse(httpWebRequest, postData);
        }

        private static string GetHttpWebResponse(WebRequest httpWebRequest, string postData)
        {
            var requestWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            try
            {
                requestWriter.Write(postData);
            }
            finally
            {
                requestWriter.Close();
            }
            return GetHttpWebResponse(httpWebRequest);
        }

      

        private string AppendPostDataToUrl(string postData, string url)
        {
            if (url.IndexOf("?") > 0)
            {
                url += "&";
            }
            else
            {
                url += "?";
            }
            url += ParsePostData(postData);
            return url;
        }

        private string ParsePostData(string postData)
        {
            var appendedPostData = postData + "&source=" + AppKey;
            var queryString = HttpUtility.ParseQueryString(appendedPostData);
            var resultUrl = "";
            foreach (var key in queryString.AllKeys)
            {
                if (resultUrl.Length > 0)
                {
                    resultUrl += "&";
                }
                EncodeUrl(queryString, key);
                resultUrl += (key + "=" + queryString[key]);
            }
            return resultUrl;
        }

        private void EncodeUrl(NameValueCollection queryString, string key)
        {
            queryString[key] = HttpUtility.UrlEncode(queryString[key]);
            queryString[key] = UrlEncode(queryString[key]);
        }

        public string RequestWithPicture(string url, string postData, string filepath, byte[] imageStream)
        {
            var uploadApiUrl = url;
            var status = postData.Split('=').GetValue(1).ToString();
            var appendUrl = AppendPostDataToUrl(postData, url);
            var authorizationHeader = GetAuthorizationHeader(appendUrl, POST);

            var request = (HttpWebRequest)System.Net.WebRequest.Create(uploadApiUrl);
            request.Headers.Add("Authorization", authorizationHeader);

            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Method = POST;
            request.UserAgent = "Jakarta Commons-HttpClient/3.1";

            var bytes = GetContentsBytes(request, status, filepath,imageStream);

            return GetHttpWebResponse(request, bytes);
        }

        private static string GetHttpWebResponse(WebRequest httpWebRequest, byte[] bytes)
        {
            var requestStream = httpWebRequest.GetRequestStream();
            try
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            finally
            {
                requestStream.Close();
            }
            return GetHttpWebResponse(httpWebRequest);
        }

        private byte[] GetContentsBytes(WebRequest request, string status, string filepath, byte[] imageStream)
        {
            string boundary = Guid.NewGuid().ToString();
            string header = string.Format("--{0}", boundary);
            string footer = string.Format("--{0}--", boundary);

            var contents = new StringBuilder();
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            contents.AppendLine(header);
            contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
            contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            contents.AppendLine("Content-Transfer-Encoding: 8bit");
            contents.AppendLine();
            contents.AppendLine(status);

            contents.AppendLine(header);
            contents.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "source"));
            contents.AppendLine("Content-Type: text/plain; charset=US-ASCII");
            contents.AppendLine("Content-Transfer-Encoding: 8bit");
            contents.AppendLine();
            contents.AppendLine(AppKey);


            contents.AppendLine(header);
            string fileHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "pic",
                                              filepath);

            //MemoryStream tmpImage = new MemoryStream(imageStream);

            //tmpImage.Read(buffer, 0, buffer.Length);

            string fileData = Encoding.GetEncoding(ContentEncoding).GetString(imageStream);

            contents.AppendLine(fileHeader);
            contents.AppendLine("Content-Type: application/octet-stream; charset=UTF-8");
            contents.AppendLine("Content-Transfer-Encoding: binary");
            contents.AppendLine();
            contents.AppendLine(fileData);
            contents.AppendLine(footer);

            byte[] bytes = Encoding.GetEncoding(ContentEncoding).GetBytes(contents.ToString());
            request.ContentLength = bytes.Length;
            return bytes;
        }

    }
}