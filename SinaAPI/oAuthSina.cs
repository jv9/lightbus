using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Web.Configuration;

namespace SinaAPI
{
    public class oAuthSina : oAuthBase
    {
        public enum Method { GET, POST, UPLOAD, DELETE };
        public const string REQUEST_TOKEN = "http://api.t.sina.com.cn/oauth/request_token";
        public const string AUTHORIZE = "http://api.t.sina.com.cn/oauth/authorize";
        public const string ACCESS_TOKEN = "http://api.t.sina.com.cn/oauth/access_token";

        private string _consumerKey = "";
        private string _consumerSecret = "";
        private string _token = "";
        private string _tokenSecret = "";

       //private bool IsProxy = false;

        #region Properties
        public string ConsumerKey
        {
            get
            {
                if (_consumerKey.Length == 0)
                {
                    _consumerKey = "1038835753";
                }
                return _consumerKey;
            }
            set { _consumerKey = value; }
        }

        public string ConsumerSecret
        {
            get
            {
                if (_consumerSecret.Length == 0)
                {
                    _consumerSecret = "43d5fbc6e567a9a59282e7d506b10642";
                }
                return _consumerSecret;
            }
            set { _consumerSecret = value; }
        }

        public string Token { get { return _token; } set { _token = value; } }
        public string TokenSecret { get { return _tokenSecret; } set { _tokenSecret = value; } }
        #endregion

        /// <summary>
        /// Get the link to Twitter's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationSinaGet()
        {
            string ret = null;


            string response = oAuthWebRequest(Method.GET, REQUEST_TOKEN, String.Empty);
            if (response.Length > 0)
            {
                //response contains token and token secret.  We only need the token.
                //oauth_token=36d1871d-5315-499f-a256-7231fdb6a1e0&oauth_token_secret=34a6cb8e-4279-4a0b-b840-085234678ab4&oauth_callback_confirmed=true
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                if (qs["oauth_token"] != null)
                {
                    this.Token = qs["oauth_token"];
                    this.TokenSecret = qs["oauth_token_secret"];
                    ret = AUTHORIZE + "?oauth_token=" + this.Token;
                }
            }
            return ret;
        }

        /// <summary>
        /// Exchange the request token for an access token.
        /// </summary>
        /// <param name="authToken">The oauth_token is supplied by Twitter's authorization page following the callback.</param>
        public void AccessTokenGet(string authToken)
        {
            this.Token = authToken;

            string response = oAuthWebRequest(Method.GET, ACCESS_TOKEN, string.Empty);

            if (response.Length > 0)
            {
                //Store the Token and Token Secret
                NameValueCollection qs = HttpUtility.ParseQueryString(response);
                if (qs["oauth_token"] != null)
                {
                    this.Token = qs["oauth_token"];
                }
                if (qs["oauth_token_secret"] != null)
                {
                    this.TokenSecret = qs["oauth_token_secret"];
                }
            }
        }

        /// <summary>
        /// Submit a web request using oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <returns>The web server response.</returns>
        public string oAuthWebRequest(Method method, string url, string postData)
        {
            string outUrl = "";
            string querystring = "";
            string ret = "";


            //Setup postData for signing.
            //Add the postData to the querystring.
            if (method == Method.POST || method == Method.UPLOAD)
            {
                if (postData.Length > 0)
                {
                    //Decode the parameters and re-encode using the oAuth UrlEncode method.
                    //NameValueCollection qs = HttpUtility.ParseQueryString(postData);
                    //postData = "";
                    //foreach (string key in qs.AllKeys)
                    //{
                    //    if (postData.Length > 0)
                    //    {
                    //        postData += "&";
                    //    }
                    //    qs[key] = HttpUtility.UrlDecode(qs[key]);
                    //    qs[key] = this.UrlEncode(qs[key]);
                    //    postData += key + "=" + qs[key];

                    //}


                    if (url.IndexOf("?") > 0)
                    {
                        url += "&";
                    }
                    else
                    {
                        url += "?";
                    }
                    url += postData;
                }
            }

            Uri uri = new Uri(url);

            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            //Generate Signature
            string sig = this.GenerateSignature(uri,
                this.ConsumerKey,
                this.ConsumerSecret,
                this.Token,
                this.TokenSecret,
                method.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);


            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(sig);

            //Convert the querystring to postData
            if (method == Method.POST)
            {
                postData = querystring;
                querystring = "";
            }

            if (querystring.Length > 0)
            {
                outUrl += "?";
            }

            if (method == Method.POST || method == Method.GET)
                ret = WebRequest(method, outUrl + querystring, postData);
            //else if (method == Method.PUT)
            //ret = WebRequestWithPut(Method.PUT,outUrl + querystring, postData);
            return ret;
        }


       

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequestWithPut(Method method, string url, string postData)
        {

            //oauth_consumer_key=5s5OSkySDF_mG_dLkduT3l7gokQ68hBtEpY5a9ebJVDH2r5BG8Opb6mUQhPgmQEB
            //oauth_nonce=9708457
            //oauth_signature_method=HMAC-SHA1
            //oauth_timestamp=1259135648
            //oauth_token=387026c7-27bd-4a11-b76e-fbe052ce88ad
            //oauth_verifier=31838
            //oauth_version=1.0
            //oauth_signature=4wYKoBy4ndrR4ziDNTd5mQV%2fcLY%3d

            //url = "http://api.linkedin.com/v1/people/~/current-status";
            Uri uri = new Uri(url);

            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            string outUrl, querystring;

            //Generate Signature
            string sig = this.GenerateSignatureBase(uri,
                this.ConsumerKey,
                this.ConsumerSecret,
                this.Token,
                this.TokenSecret,
                "PUT",
                timeStamp,
                nonce,
                out outUrl,
                out querystring);

            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(sig);
            NameValueCollection qs = HttpUtility.ParseQueryString(querystring);
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xml += "<current-status>is setting their status using the LinkedIn API.</current-status>";

            HttpWebRequest webRequest = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = "text/xml";
            webRequest.Method = "PUT";
         
            webRequest.ServicePoint.Expect100Continue = false;

            webRequest.Headers.Add("Authorization", "OAuth realm=\"\"");
            webRequest.Headers.Add("oauth_consumer_key", this.ConsumerKey);
            webRequest.Headers.Add("oauth_token", this.Token);
            webRequest.Headers.Add("oauth_signature_method", "HMAC-SHA1");
            webRequest.Headers.Add("oauth_signature", sig);
            webRequest.Headers.Add("oauth_timestamp", timeStamp);
            webRequest.Headers.Add("oauth_nonce", nonce);
            webRequest.Headers.Add("oauth_verifier", this.Verifier);
            webRequest.Headers.Add("oauth_version", "1.0");

            //webRequest.KeepAlive = true;

            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            try
            {
                requestWriter.Write(postData);
            }
            catch
            {
                throw;
            }
            finally
            {
                requestWriter.Close();
                requestWriter = null;
            }


            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            string returnString = response.StatusCode.ToString();

            webRequest = null;

            return responseData;

        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;        

            //string boundary = System.Environment.TickCount.ToString();

            if (method == Method.POST || method == Method.UPLOAD)
            {
                if (method == Method.UPLOAD)
                {
                    //webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                    webRequest.Method = "POST";
                    //string postData1 = "";
                    //postData1 = "--" + boundary + "\r\n" +
                    //    "Content-Disposition: form-data; name=\"username\"\r\n\r\n" + Settings.Default.Username + "\r\n" +
                    //    "--" + boundary + "\r\n" +
                    //    "Content-Disposition: form-data; name=\"password\"\r\n\r\n" + Settings.Default.Password + "\r\n" +
                    //    "--" + boundary + "\r\n" +
                    //    "Content-Disposition: form-data; name=\"media\"; filename=\"" + Path.GetFileName(path) + "\"\r\n" +
                    //    "Content-Type: application/octet-stream\r\n" +
                    //    "Content-Transfer-Encoding: binary\r\n\r\n";
                }
                else
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                //webRequest.ContentType = "multipart/form-data";
                
                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }
           
            responseData = WebResponseGet(webRequest);

            webRequest = null;

            return responseData;

        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream(),Encoding.UTF8);
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }

        public string ParseHtml(string html)
        {
            //Regex htmlRegex = new Regex("获取到授权码：[0-9]{6}</b>");
            Regex htmlRegex = new Regex("获取到的授权码：<span class=\"fb\">[0-9]{6}</span>");
            Match m = htmlRegex.Match(html);
            Regex pinRegex = new Regex("[0-9]{6}");
            Match m1 = pinRegex.Match(m.Value);
            return m1.Value;
        }

        public string UploadPhoto(string username, string passwd,string msg,string imageFileName, byte[] uploadimage)
        {
            //HttpWebRequest webRequest = null;
            //StreamWriter requestWriter = null;
            //string responseData = "";

            //webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            //webRequest.Method = method.ToString();
            //webRequest.ServicePoint.Expect100Continue = false;

            //string boundary = System.Environment.TickCount.ToString();
            //webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            //webRequest.Method = "POST";

            string responseData = "";
            StreamReader responseReader = null;

            string url = "http://api.t.sina.com.cn/statuses/upload.xml";
            string boundary = System.Environment.TickCount.ToString();
            var req = System.Net.WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;


            string postData = "";
            postData = "--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"username\"\r\n\r\n" + username + "\r\n" +
                "--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"password\"\r\n\r\n" + passwd + "\r\n" +
                "--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"media\"; filename=\"" + imageFileName + "\"\r\n" +
                "Content-Type: application/octet-stream\r\n" +
                "Content-Transfer-Encoding: binary\r\n\r\n";

            var enc = Encoding.UTF8;

            byte[] startData = enc.GetBytes(postData);

            postData = "\r\n--" + boundary + "--\r\n";
            byte[] endData = enc.GetBytes(postData);



            req.ContentLength = startData.Length + uploadimage.Length + endData.Length;



            var reqStream = req.GetRequestStream();

            reqStream.Write(startData, 0, startData.Length);

            reqStream.Write(uploadimage, 0, uploadimage.Length);

            reqStream.Write(endData, 0, endData.Length);
            reqStream.Close();


            responseReader = new StreamReader(reqStream, Encoding.UTF8);
            responseData = responseReader.ReadToEnd();



            return responseData;

        }


        /// <summary>
        /// Submit a web request using oAuth.
        /// </summary>
        /// <param name="method">GET or POST</param>
        /// <param name="url">The full url, including the querystring.</param>
        /// <param name="postData">Data to post (querystring format)</param>
        /// <returns>The web server response.</returns>
        public string oAuthWebRequest(Method method, string url, string postData,string username,string passwd,string filename, byte[] imageStream,string status)
        {
            string outUrl = "";
            string querystring = "";
            string ret = "";

            if (method == Method.POST || method == Method.UPLOAD)
            {
                if (postData.Length > 0)
                {
                    if (url.IndexOf("?") > 0)
                    {
                        url += "&";
                    }
                    else
                    {
                        url += "?";
                    }
                    url += postData;
                }
            }

            Uri uri = new Uri(url);

            string nonce = this.GenerateNonce();
            string timeStamp = this.GenerateTimeStamp();

            //Generate Signature
            string sig = this.GenerateSignature(uri,
                this.ConsumerKey,
                this.ConsumerSecret,
                this.Token,
                this.TokenSecret,
                method.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);

            
            //querystring += "&pic=" + content;
            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(sig);

            //Convert the querystring to postData
            if (method == Method.POST)
            {
                postData = querystring;
                querystring = "";
            }

            if (querystring.Length > 0)
            {
                outUrl += "?";
            }

            if (method == Method.POST || method == Method.GET)
            {

                //ret = PostHttpWebRequestbyoAuth(status, filename, imageStream, postData, username, passwd,this.ConsumerKey,nonce, "HMAC-SHA1", timeStamp, this.Token, "1.0", HttpUtility.UrlEncode(sig));
                ret = PostHttpWebRequestbyoAuth(status, filename, imageStream, postData, username, passwd);
            }
               

            return ret;
        }

        //Basic  Using
        public string PostHttpWebRequestbyoAuth(string statuses, 
                                                string filename, 
                                                byte[] imageStream,string postdata,string username,string passwd)
        {
            string url = "http://api.t.sina.com.cn/statuses/upload.xml";
            postdata = postdata.Replace("&", ",");

            HttpWebRequest webrequest = null;
            webrequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webrequest.ContentType = "multipart/form-data; boundary=---------------------------7da1383ae0206";
            webrequest.Method = "POST";
            //webrequest.Headers.Add("Cookie: " + cookie);
            //webrequest.Referer = refre;
            webrequest.Timeout = Timeout.Infinite;

            string usernamePassword = username + ":" + passwd;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri(url), "Basic", new NetworkCredential(username, passwd));

            webrequest.Credentials = mycache;
            webrequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));



            //string usernamePassword = username + ":" + passwd;
            //CredentialCache mycache = new CredentialCache();
            //mycache.Add(new Uri(url), "OAuth", new NetworkCredential(username, passwd));


            // 构造发送数据 
            StringBuilder sb = new StringBuilder();

            //sb.Append("-----------------------------7da1383ae0206");
            //sb.Append("\r\n");
            //sb.Append("Content-Disposition: form-data; name=\"Authorization\"");
            //sb.Append("\r\n\r\n");
            //sb.Append("OAuth " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(postdata)));
            //sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"status\"");
            sb.Append("\r\n\r\n");
            sb.Append(statuses);
            sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"source\"");
            sb.Append("\r\n\r\n");
            sb.Append(WebConfigurationManager.AppSettings["consumerKey"]);
            sb.Append("\r\n");


            // 文件域的数据 
            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"pic\";filename=\"" + filename + "\"");
            sb.Append("\r\n");

            sb.Append("Content-Type: ");
            sb.Append("image/pjpeg");
            sb.Append("\r\n\r\n");



            //Response.Write(sb.ToString());
            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            //构造尾部数据 
            byte[] boundaryBytes = Encoding.UTF8.GetBytes("\r\n-----------------------------7da1383ae0206--\r\n");

            //FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            MemoryStream tmpImage = new MemoryStream(imageStream);


            long length = postHeaderBytes.Length + tmpImage.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // 输入头部数据 
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // 输入文件流数据 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)tmpImage.Length))];
            int bytesRead = 0;
            while ((bytesRead = tmpImage.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);


            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            return sr.ReadToEnd();

            //return "";

        }

        //oauth_consumer_key=1038835753,
//oauth_nonce=4936493,
//oauth_signature_method=HMAC-SHA1,
//oauth_timestamp=1276888610,
//oauth_token=0eb8563f169d96769599bc6de37740fe,
//oauth_version=1.0,
//oauth_signature=xqRTV9VFmWFKsI9OxpPlsJjtfnI%3d

        //For oAuth
        public string PostHttpWebRequestbyoAuth(string statuses, 
            string filename, byte[] imageStream, string postdata, 
            string username, string passwd,string consumerkey,
            string oauthnonce,string authsignaturemethod, string oauthtimestamp, string token,
            string oauthversion,string oauthsignature)
        {
            string url = "http://api.t.sina.com.cn/statuses/upload.xml";
            //string[] oauthString = postdata.Split(

            HttpWebRequest webrequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webrequest.ContentType = "multipart/form-data; boundary=---------------------------7da1383ae0206";
            webrequest.Method = "POST";
            //webrequest.Headers.Add("Cookie: " + cookie);
            //webrequest.Referer = refre;
            webrequest.Timeout = Timeout.Infinite;



            // 构造发送数据 
            StringBuilder sb = new StringBuilder();

            //sb.Append("-----------------------------7da1383ae0206");
            //sb.Append("\r\n");
            //sb.Append("Content-Disposition: form-data; name=\"Authorization\"");
            //sb.Append("\r\n\r\n");
            //sb.Append("OAuth " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(postdata)));
            //sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"status\"");
            sb.Append("\r\n\r\n");
            sb.Append(statuses);
            sb.Append("\r\n");

            //sb.Append("-----------------------------7da1383ae0206");
            //sb.Append("\r\n");
            //sb.Append("Content-Disposition: form-data; name=\"source\"");
            //sb.Append("\r\n\r\n");
            //sb.Append(WebConfigurationManager.AppSettings["consumerKey"]);
            //sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"oauth_consumer_key\"");
            sb.Append("\r\n\r\n");
            sb.Append(WebConfigurationManager.AppSettings["consumerKey"]);
            sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"oauth_nonce\"");
            sb.Append("\r\n\r\n");
            sb.Append(oauthnonce);
            sb.Append("\r\n");


            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"oauth_signature_method\"");
            sb.Append("\r\n\r\n");
            sb.Append(oauthtimestamp);
            sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"oauth_signature_method\"");
            sb.Append("\r\n\r\n");
            sb.Append(oauthtimestamp);
            sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"oauth_token\"");
            sb.Append("\r\n\r\n");
            sb.Append(token);
            sb.Append("\r\n");


            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"oauth_version\"");
            sb.Append("\r\n\r\n");
            sb.Append(oauthversion);
            sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"oauth_signature\"");
            sb.Append("\r\n\r\n");
            sb.Append(oauthsignature);
            sb.Append("\r\n");

            // 文件域的数据 
            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"pic\";filename=\"" + filename + "\"");
            sb.Append("\r\n");

            sb.Append("Content-Type: ");
            sb.Append("image/pjpeg");
            sb.Append("\r\n\r\n");



            //Response.Write(sb.ToString());
            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            //构造尾部数据 
            byte[] boundaryBytes = Encoding.UTF8.GetBytes("\r\n-----------------------------7da1383ae0206--\r\n");

            //FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            MemoryStream tmpImage = new MemoryStream(imageStream);


            long length = postHeaderBytes.Length + tmpImage.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // 输入头部数据 
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // 输入文件流数据 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)tmpImage.Length))];
            int bytesRead = 0;
            while ((bytesRead = tmpImage.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);


            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            return sr.ReadToEnd();

            //return "";

        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData, string username, string passwd, string filename, byte[] imageStream)
        {
            //HttpWebRequest webRequest = null;
            //StreamWriter requestWriter = null;
            //string responseData = "";

            string boundary = System.Environment.TickCount.ToString();
            var req = System.Net.WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            req.ServicePoint.Expect100Continue = false;


            //StreamReader responseReader = null;
            //string ContentData;
            //postData += "--" + boundary + "\r\n" +
            //    "Content-Disposition: form-data; name=\"username\"\r\n\r\n" + username + "\r\n" +
            //    "--" + boundary + "\r\n" +
            //    "Content-Disposition: form-data; name=\"password\"\r\n\r\n" + passwd + "\r\n" +
            //    "--" + boundary + "\r\n" +
            //    "Content-Disposition: form-data; name=\"media\"; filename=\"" + filename + "\"\r\n" +
            //    "Content-Type: application/octet-stream\r\n" +
            //    "Content-Transfer-Encoding: binary\r\n\r\n";
            //------------------------------------------------


            //注意这里的格式哦，为 "username:password"
            string usernamePassword = username + ":" + passwd;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri(url), "Basic", new NetworkCredential(username, passwd));

            req.Credentials = mycache;
            req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));


            // 构造发送数据 
            StringBuilder sb = new StringBuilder();

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"status\"");
            sb.Append("\r\n\r\n");
            sb.Append("myemxa"); //Status
            sb.Append("\r\n");

            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"source\"");
            sb.Append("\r\n\r\n");
            sb.Append("1038835753"); //ApiKey
            sb.Append("\r\n");


            // 文件域的数据 
            sb.Append("-----------------------------7da1383ae0206");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"pic\";filename=\"" + filename + "\"");
            sb.Append("\r\n");

            sb.Append("Content-Type: ");
            sb.Append("image/pjpeg");
            sb.Append("\r\n\r\n");



            //Response.Write(sb.ToString());
            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            //构造尾部数据 
            byte[] boundaryBytes = Encoding.UTF8.GetBytes("\r\n-----------------------------7da1383ae0206--\r\n");

            //FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            //StreamReader reader = new StreamReader(imageStream,);

            //FileStream fileStream = new FileStream(imageStream,FileMode.Open, FileAccess.Read);

            long length = postHeaderBytes.Length + imageStream.Length + boundaryBytes.Length;
            req.ContentLength = length;
            
            Stream requestStream = req.GetRequestStream();

            // 输入头部数据 
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // 输入文件流数据 
            byte[] buffer = new Byte[checked((uint)Math.Min(1024, (int)imageStream.Length))];
            //int bytesRead = 0;

            //BufferedReader reader = new BufferedReader(new InputStreamReader

            //foreach(byte tmpBuff in imageStream)
            //{
            //    buffer = tmpBuff;
            //}

            //for (bytesRead = 0; bytesRead < (int)imageStream.Length; bytesRead++)
            //{
            //    buffer = imageStream[bytesRead];

            //    //buffer[bytesRead] = imageStream[bytesRead];
            //    requestStream.Write(buffer, 0, bytesRead);
            //}

            buffer = imageStream;
            requestStream.Write(buffer, 0, (int)imageStream.Length);

            //while ((bytesRead = imageStream(buffer, 0, buffer.Length)) != 0)
            //    requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

            requestStream.Close();
            //responseData = WebResponseGet(req);

            //req = null;
            //return responseData;

            WebResponse responce = req.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码) 
            return sr.ReadToEnd();
 


            //-----------------------------------------------
            //var enc = Encoding.UTF8;

            //byte[] startData = enc.GetBytes(postData);

            //postData += "\r\n--" + boundary + "--\r\n";
            //byte[] endData = enc.GetBytes(postData);

            //req.ContentLength = startData.Length + imageStream.Length + endData.Length;

            //if (method == Method.POST || method == Method.UPLOAD)
            //{

            //    var reqStream = req.GetRequestStream();

            //    reqStream.Write(startData, 0, startData.Length);

            //    //byte[] readData = new byte[0x10000];
            //    //int readSize = 0;

            //    //for (readSize = 0; readSize < imageStream.Length; readSize++)
            //    //{
            //    //    reqStream.Write(imageStream[readSize], 0, imageStream.Length);
            //    //    //readData[readSize] = ;
            //    //}

            //    //while (true)
            //    //{
            //    //    readSize = imageStream.Length;
            //    //    if (readSize == 0)
            //    //    {
            //    //        break;
            //    //    }

            //    //}

            //    reqStream.Write(imageStream, 0, imageStream.Length);

            //    reqStream.Write(endData, 0, endData.Length);
            //    //reqStream.Close();



            //    //responseReader = new StreamReader(reqStream, Encoding.UTF8);
            //    //responseData = responseReader.ReadToEnd();



                
            //    //responseReader = new StreamReader(reqStream, Encoding.UTF8);
            //    //responseData = responseReader.ReadToEnd();


            //    //POST the data.
            //    //requestWriter = new StreamWriter(webRequest.GetRequestStream());
            //    //try
            //    //{
            //    //    requestWriter.Write(postData);
            //    //}
            //    //catch
            //    //{
            //    //    throw;
            //    //}
            //    //finally
            //    //{
            //    //    requestWriter.Close();
            //    //    requestWriter = null;
            //    //}
            //}

            //responseData = WebResponseGet(req);

            //req = null;
            //return responseData;
            //return responseData;

        }



    }
}
