using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace SinaDesktop.Web
{
    [ServiceContract]
    public interface IDataService
    {
      
        [OperationContract]
        List<TokenKeyCollection> GetToken(string userID, string pwd);
        [OperationContract()]
        UserList VerifyCredentials(string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        List<FriendTimelineList> GetFriendTimeline(string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        List<FavoriteList> GetFavorites(string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        List<CommentTimelineList> GetCommentTimeline(string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        string UpdateStatus(string format, string status, string apiToken, string apiTokenSecret);
        [OperationContract()]
        string UploadHttpWebRequestByoAuth(string userID, string pwd, string status, string filename, byte[] imageStream, string apiToken, string apiTokenSecret);
        [OperationContract()]
        List<MentionList> GetMentions(string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        List<DirectMessageList> GetDirectMessages(string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        UnreadMessageList GetUnreaderMessage(string sinceid, string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        string UpdateCommentByID(string statusid, string format, string status, string apiToken, string apiTokenSecret); //String!!
        [OperationContract()]
        string RepostStatus(string statusid,  string status, int isComment,string format,string apiToken, string apiTokenSecret); //String!!
        [OperationContract()]
        FavoriteList CreateFavorite(string statusid, string format, string apiToken, string apiTokenSecret); //String!!
        [OperationContract()]
        FavoriteList DeleteFavorite(string statusid, string format, string apiToken, string apiTokenSecret); //String!!
        [OperationContract()]
        List<EmotionList> GetEmotions(string type, string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        string CreateFriendships(string userid, string format, string apiToken, string apiTokenSecret); //String！！
        [OperationContract()]
        List<FriendTimelineList> GetUserTimeline(string userid, string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        UserList GetUserProfile(string userid, string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        List<CommentTimelineList> GetCommentsTimelineByID(string id,string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        string ResetCount(string type, string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        string Status_ReplyComment(string twitterid, string commentcontent, string commentid, string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        List<FollowerList> GetFollower(string userid,int count, string format, string apiToken, string apiTokenSecret);
        [OperationContract()]
        string Status_Delete(string twitterid,string format, string apiToken, string apiTokenSecret);


    }
}