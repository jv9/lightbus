using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace SinaAPI
{
    public class SinaApiService : oAuthSina,ISinaApiService
    {
        //private oAuthSina _oauth;
        public bool oAuth(string userid, string passwd, oAuthSina _oauth)
        {
            try
            {
                if (_oauth.Token == "" || _oauth.Token == null)
                {
                    string authLink = _oauth.AuthorizationSinaGet();
                    authLink += "&userId=" + userid + "&passwd=" + passwd + "&action=submit&oauth_callback=none";
                    string html = _oauth.WebRequest(oAuthSina.Method.POST, authLink, null);
                    string pin = ParseHtml(html);
                    _oauth.Verifier = pin;
                    _oauth.AccessTokenGet(_oauth.Token);

                    return true;
                }
                else
                {
                    _oauth.AccessTokenGet(_oauth.Token);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /*最新公共微博*/
        public string public_timeline(string userid, string passwd, string format)
        {
            oAuthSina _oauth = new oAuthSina();
            if (oAuth(userid, passwd, _oauth))
            {
                string url = "http://api.t.sina.com.cn/statuses/public_timeline." + format;
                return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
            }
            else
                return null;
        }

        /*最新关注人微博 OK*/
        public string friend_timeline(string format,string apiKey,string apiKeySecret,string apiToken,string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/friends_timeline." + format;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
        }

        /*用户发表微薄列表 OK*/
        public string user_timeline(string userid, string format,string apiKey,string apiKeySecret,string apiToken,string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/user_timeline." + format + "?user_id=" + userid; ;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);

        }
        /*最新n条@我的微博*/
        public string mentions(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/mentions." + format;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
        }
        /*最新评论*/
        public string comments_timeline(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/comments_timeline." + format;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
        }

        /*发出的评论*/
        public string comments_by_me(string userid, string passwd, string format)
        {
            oAuthSina _oauth = new oAuthSina();
            if (oAuth(userid, passwd, _oauth))
            {
                string url = "http://api.t.sina.com.cn/statuses/comments_by_me." + format;
                return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
            }
            else
                return null;
        }

        /*批量获取一组微博的评论数及转发数*/
        public string counts(string userid, string passwd, string format, string ids)
        {
            oAuthSina _oauth = new oAuthSina();
            if (oAuth(userid, passwd, _oauth))
            {
                string url = "http://api.t.sina.com.cn/statuses/counts." + format + "?ids=" + ids;
                return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
            }
            else
                return null;
        }
        /**********************************************************************************************
         *************************************微博访问接口*********************************************
         **********************************************************************************************
         **********************************************************************************************/
        /*获取单条ID的微博信息*/
        public string statuses_show(string userid, string passwd, string format, string id)
        {
            oAuthSina _oauth = new oAuthSina();
            if (oAuth(userid, passwd, _oauth))
            {
                string url = "http://api.t.sina.com.cn/statuses/show/" + id + "." + format;
                return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
            }
            else
                return null;
        }
        /*获取单条ID的微博信息*/
        public string statuses_id(string userid, string passwd, string id, string uid)
        {
            oAuthSina _oauth = new oAuthSina();
            if (oAuth(userid, passwd, _oauth))
            {
                string url = "http://api.t.sina.com.cn/" + uid + "/statuses/" + id;
                return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
            }
            else
                return null;
        }
        /*发布一条微博信息*/
        public string statuses_update(string format,string status, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/update." + format;

            //System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            //byte[] bytesToPost = encoding.

              //return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, "status=" + _oauth.UrlEncode(status));
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, "status=" + UrlEncodePost(status));

            //if (oAuth(userid, passwd, _oauth))
            //{
            //    string url = "http://api.t.sina.com.cn/statuses/update." + format + "?";
            //    return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, "status=" + _oauth.UrlEncode(status));
            //}
            //else
            //    return null;
        }

        public string PostWebRequestCustomize(string userid, string passwd, string format,string status, string apiKey)
        {
         
            string usernamePassword = userid + ":" + passwd;

            string url = "http://api.t.sina.com.cn/statuses/update."+format;
            string news_title = status;
            //int news_id = 696365;
            //string t_news = string.Format("{0}，http://news.cnblogs.com/n/{1}/", news_title, news_id);

            string t_news = news_title;
            string data = "source=" + apiKey + "&status=" + System.Web.HttpUtility.UrlEncode(t_news);

            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
            System.Net.HttpWebRequest httpRequest = webRequest as System.Net.HttpWebRequest;

            System.Net.CredentialCache myCache = new System.Net.CredentialCache();
            myCache.Add(new Uri(url), "Basic", new System.Net.NetworkCredential(userid, passwd));
            httpRequest.Credentials = myCache;
            httpRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new System.Text.ASCIIEncoding().GetBytes(usernamePassword)));

            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";


            System.Text.Encoding encoding = System.Text.Encoding.ASCII;
            byte[] bytesToPost = encoding.GetBytes(data);
            httpRequest.ContentLength = bytesToPost.Length;
            System.IO.Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(bytesToPost, 0, bytesToPost.Length);
            requestStream.Close();
            string responseContent;

            System.Net.WebResponse wr = httpRequest.GetResponse();
            System.IO.Stream receiveStream = wr.GetResponseStream();
            using (System.IO.StreamReader reader = new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8))
            {
                responseContent = reader.ReadToEnd();
            }

            return responseContent;
        }

        /*上传图片并且发布一条微博信息*/
                public string statuses_upload(string userid, string passwd, string format, string status, 
                                      string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret,
                                      string filename,byte[] imageStream)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/upload." + format;

            return "";
            //return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, "status=" + UrlEncodePost(status) ,userid,passwd,filename,imageStream);

        }


        /*上传图片并且发布一条微博信息 Basic&oAuth*/
        public string status_uploadbyoauth(string userid, string passwd, string format, string status,
                                            string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret,
                                            string filename, byte[] imageStream)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/upload." + format;

            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty,userid,passwd,filename,imageStream,status);
        }


        public string status_destroy(string twitterID, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/destroy/" + twitterID.ToString() + "." + format;

            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
        }

        /* 单条评论列表*/
        public string status_comment(string id, string commentcontent, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/comment." + format + "?id=" + id +"&comment=" + commentcontent;
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
          
        }

        /* 删除单条评论*/
        public string status_comment_destroy(string userid, string passwd, string commentID, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;

            string url = "http://api.t.sina.com.cn/statuses/comment_destroy/" + commentID.ToString() + "." + format;

            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
        }

        /*转发一条微博信息*/
        public string status_repost(string id, string commentcontent,int isComment, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "";
            if (commentcontent.Length > 0)
            {
                url = "http://api.t.sina.com.cn/statuses/repost." + format + "?id=" + id + "&status=" + commentcontent + "&is_comment=" + isComment;
            }
            else
            {
                url = "http://api.t.sina.com.cn/statuses/repost." + format + "?id=" + id;
            }
            
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
        }

        /*回复微博评论*/
        public string status_replycomment(string twitterid, string commentcontent, string commentid, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/reply." + format + "?cid=" + commentid + "&comment=" + commentcontent + "&id=" + twitterid;
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
        }

        /**********************************************************************************************
         *************************************账号接口*ok********************************************
         **********************************************************************************************
         **********************************************************************************************/
        //Login
        public string VerifyCredentials(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {

            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/account/verify_credentials." + format;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);  
        }

        public List<string> GetToken(string userid, string passwd)
        {
            List<string> TokenList = new List<string>();

            oAuthSina _oauth = new oAuthSina();
            if (oAuth(userid, passwd, _oauth))
            {
                TokenList.Add(_oauth.Token);
                TokenList.Add(_oauth.TokenSecret);

                
                return TokenList;
            }
            else
                return null;
        }

        /**********************************************************************************************
        ******************************************收藏接口**OK*******************************************
        **********************************************************************************************
        **********************************************************************************************/
        public string GetFavorites(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/favorites." + format;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);  
        }

        #region ISinaApiService Members


        public string direct_messages(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/direct_messages." + format;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);  
        }

        public string unread_message(string sinceid, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/unread." + format + "?source=" + apiKey + "&with_new_status=1" + "&since_id=" + sinceid;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
        }

        public string create_favorite(string id, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/favorites/create." + format + "?id=" + id;
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);

        }

        public string delete_favorite(string id, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/favorites/destroy." + format + "?id=" + id;
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
        }

        public string GetEmotions(string type, string language, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;

            string url = "http://api.t.sina.com.cn/emotions." + format;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);

        }


        public string create_friendships(string userid, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/friendships/create." + format + "?user_id=" + userid;
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
        }


        public string GetUserProfile(string userid, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/users/show." + format + "?user_id=" + userid;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
        }

        public string GetCommentsTimelineByID(string id, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/comments." + format + "?id=" + id;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
        }

        public string reset_count(string type, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/reset_count." + format + "?type=" + type;
            return _oauth.oAuthWebRequest(oAuthSina.Method.POST, url, String.Empty);
        }

        #endregion

        #region ISinaApiService Members


        public string get_followers(string id,int count, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/statuses/followers." + format + "?id=" + id + "&count=" + count;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);  
        }

        #endregion


        public string show_relationship(string userid, string targetid, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret)
        {
            oAuthSina _oauth = new oAuthSina();
            _oauth.ConsumerKey = apiKey;
            _oauth.ConsumerSecret = apiKeySecret;
            _oauth.Token = apiToken;
            _oauth.TokenSecret = apiTokenSecret;
            string url = "http://api.t.sina.com.cn/friendships/show." + format + "?source_id=" + userid +"&target_id=" + targetid;
            return _oauth.oAuthWebRequest(oAuthSina.Method.GET, url, String.Empty);
        }
    }






}
