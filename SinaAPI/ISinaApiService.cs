using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SinaAPI
{
    [ServiceContract]
    interface ISinaApiService
    {
        /*最新公共微博*/
        [OperationContract]
        string public_timeline(string userid, string passwd, string format);

        /*最新关注人微博*/
        [OperationContract]
        string friend_timeline(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*用户发表微薄列表*/
        [OperationContract]
        string user_timeline(string userid, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /*最新n条@我的微博*/
        [OperationContract]
        string mentions(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /*最新评论*/
        [OperationContract]
        string comments_timeline(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /*发出的评论*/
        [OperationContract]
        string comments_by_me(string userid, string passwd, string format);
        /* 单条评论列表*/
        [OperationContract]
        string status_comment(string id, string commentcontent, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /*批量获取一组微博的评论数及转发数*/
        [OperationContract]
        string counts(string userid, string passwd, string format, string ids);
        /*获取当前用户未读消息数*/
        [OperationContract]
        string unread_message(string sinceid,string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /* 转发微博*/
        [OperationContract]
        string status_repost(string id, string commentcontent, int isComment, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /* 收藏微博*/
        [OperationContract]
        string create_favorite(string id, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /* 删除收藏*/
        [OperationContract]
        string delete_favorite(string id, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        /*获取单条ID的微博信息*/
        [OperationContract]
        string statuses_show(string userid, string passwd, string format, string id);
        /*获取单条ID的微博信息*/
        [OperationContract]
        string statuses_id(string userid, string passwd, string id, string uid);
        /*发布一条微博信息*/
        [OperationContract]
        string statuses_update(string format, string status,string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
        
        //私信访问接口
        [OperationContract]
        string direct_messages(string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /* 获取表情*/
        [OperationContract]
        string GetEmotions(string type, string language, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*添加关注*/
        [OperationContract]
        string create_friendships(string userid,string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*获取用户资料*/
        [OperationContract]
        string GetUserProfile(string userid,string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*获取评论By ID*/
        [OperationContract]
        string GetCommentsTimelineByID(string id, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*回复评论*/
        [OperationContract]
        string status_replycomment(string twitterid,
            string commentcontent,string commentid, string format, string apiKey,
            string apiKeySecret, string apiToken, string apiTokenSecret);

        /*重置状态*/
        [OperationContract]
        string reset_count(string type, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*获取粉丝*/
        [OperationContract]
        string get_followers(string id,int count, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*获取两者关系*/
        [OperationContract]
        string show_relationship(string userid, string targetid, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);

        /*删除微博*/
        [OperationContract]
        string status_destroy(string twitterID, string format, string apiKey, string apiKeySecret, string apiToken, string apiTokenSecret);
    }
}
