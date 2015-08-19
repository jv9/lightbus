using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Configuration;
using SinaAPI;
using System.Collections.Generic;
using System.Xml.Linq;
using LeoShi.Soft.OpenSinaAPI;
using System.Web;

namespace SinaDesktop.Web
{
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SinaService : IDataService
    {
        #region Private Members
        private string consumerKey = WebConfigurationManager.AppSettings["consumerKey"];
        private string consumerKeySecret = WebConfigurationManager.AppSettings["consumerSecret"];
        #endregion

        #region IDataService Members
        public List<TokenKeyCollection> GetToken(string userID, string pwd)
        {
            SinaApiService myApi = new SinaApiService();
            List<string> strKey;
            strKey = myApi.GetToken(userID, pwd);
            List<TokenKeyCollection> myResults = new List<TokenKeyCollection>();
            myResults.Add(new TokenKeyCollection(strKey[0].ToString(), strKey[1].ToString()));

            return myResults;
        }

        public UserList VerifyCredentials(string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.VerifyCredentials(format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);

            XDocument doc = XDocument.Parse(myResult);
            //return UserList.Parse(doc);
            return UserList.Parse(doc.Root);
        }

        public List<FriendTimelineList> GetFriendTimeline(string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.friend_timeline(format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);

            XDocument doc = XDocument.Parse(myResult);

            return FriendTimelineList.Parse(doc.Root);
        }

        public List<FavoriteList> GetFavorites(string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.GetFavorites(format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return FavoriteList.Parse(doc.Root);
        }

        public List<CommentTimelineList> GetCommentTimeline(string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.comments_timeline(format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return CommentTimelineList.Parse(doc.Root);
        }

        public List<MentionList> GetMentions(string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.mentions(format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);

            return MentionList.Parse(doc.Root);
        }

        public List<DirectMessageList> GetDirectMessages(string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.direct_messages(format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return DirectMessageList.Parse(doc.Root);
        }

        public string UpdateStatus(string format, string status, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.statuses_update(format, status, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            return myResult;
        }

        public string UploadHttpWebRequestByoAuth(string userID, string pwd, string status, string filename, byte[] imageStream, string apiToken, string apiTokenSecret)
        {
            //SinaApiService myApi = new SinaApiService();
            //string myResult = myApi.status_uploadbyoauth(userID, pwd, "xml", status,
            //    WebConfigurationManager.AppSettings["consumerKey"],
            //    WebConfigurationManager.AppSettings["consumerSecret"], apiToken,
            //    apiTokenSecret, filename, imageStream);


            var httpRequest = HttpRequestFactory.CreateHttpRequest(Method.POST) as HttpPost;
            httpRequest.Token = apiToken;
            httpRequest.TokenSecret = apiTokenSecret;
            httpRequest.UserRemoteIP = "127.0.0.1";
            string url = "http://api.t.sina.com.cn/statuses/upload.xml?";
            string myResult = httpRequest.RequestWithPicture(url, "status=" + HttpUtility.UrlEncode(status), filename, imageStream);

            return myResult;
        }

        public UnreadMessageList GetUnreaderMessage(string sinceid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.unread_message(sinceid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return UnreadMessageList.Parse(doc.Root);
        }

        //public CommentTimelineList UpdateCommentByID(string statusid, string format, string status, string apiToken, string apiTokenSecret)
        //{
        //    SinaApiService myApi = new SinaApiService();
        //    string myResult = myApi.status_comment(statusid, status, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
        //    XDocument doc = XDocument.Parse(myResult);
        //    return CommentTimelineList.Parse(doc.Root);
        //}


        public string UpdateCommentByID(string statusid, string format, string status, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.status_comment(statusid, status, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            return myResult;
        }

        public string RepostStatus(string statusid, string status, int isComment, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();

            string myResult = myApi.status_repost(statusid, status, isComment, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            return myResult;
        }

        public FavoriteList CreateFavorite(string statusid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.create_favorite(statusid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return FavoriteList.Parse(doc.Root,0);
        }

        public FavoriteList DeleteFavorite(string statusid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.delete_favorite(statusid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return FavoriteList.Parse(doc.Root, 1);
        }
        #endregion

        // + "?type=" + type + "&language=" + language
        public List<EmotionList> GetEmotions(string type, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.GetEmotions(type, "cnname", format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return EmotionList.Parse(doc.Root);
        }

        #region IDataService Members


        public string CreateFriendships(string userid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.create_friendships(userid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            return myResult;
        }


        public List<FriendTimelineList> GetUserTimeline(string userid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.user_timeline(userid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return FriendTimelineList.Parse(doc.Root);
        }

        public UserList GetUserProfile(string userid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.GetUserProfile(userid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return UserList.Parse(doc.Root);
        }

        public List<CommentTimelineList> GetCommentsTimelineByID(string id, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.GetCommentsTimelineByID(id, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return CommentTimelineList.Parse(doc.Root);
        }

        public string ResetCount(string type, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.reset_count(type, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            return myResult;
        }

        #endregion


        public string Status_ReplyComment(string twitterid, string commentcontent,string commentid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.status_replycomment(twitterid, commentcontent, commentid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            return myResult;
        }

        #region IDataService Members


        public List<FollowerList> GetFollower(string userid,int count, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.get_followers(userid,count, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            XDocument doc = XDocument.Parse(myResult);
            return FollowerList.Parse(doc.Root);
        }

        public string Status_Delete(string twitterid, string format, string apiToken, string apiTokenSecret)
        {
            SinaApiService myApi = new SinaApiService();
            string myResult = myApi.status_destroy(twitterid, format, consumerKey, consumerKeySecret, apiToken, apiTokenSecret);
            return myResult;
        }

        #endregion
    }
}
