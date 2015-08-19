using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SinaDesktop.Web
{
    #region Data Member Object Base Class
    [DataContract]
    public abstract class DataMemberObjectBase
    {
        protected static string GetElementValue(XElement element, string elementName)
        {

            if (element.Element(XName.Get(elementName)) != null)
            {
                return element.Element(XName.Get(elementName)).Value;
            }
            else
                return "";
        }

        protected static string GetGroupElementValue(XElement element, string elementName)
        {
            XElement statusesElement = element.Element("statuses");
            XElement statusElement = element.Element("status");


            return statusElement.Element(XName.Get(elementName)).Value;

        }

    }


    #endregion

    #region Token Key Collection
    [DataContract]
    public class TokenKeyCollection
    {
        [DataMember]
        public string TokenKeyString
        {
            get;
            set;
        }

        [DataMember]
        public string TokenKeySecretString
        {
            get;
            set;
        }

        public TokenKeyCollection(string sTokenKeyString, string sTokenKeySecretString)
        {
            this.TokenKeyString = sTokenKeyString;
            this.TokenKeySecretString = sTokenKeySecretString;
        }

    }
    #endregion

    #region User Collection
    [DataContract()]
    public class UserList : DataMemberObjectBase
    {
        #region Data Members
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string TwitterName { get; set; }
        [DataMember]
        public string FriendName { get; set; }
        [DataMember]
        public string Province { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string BlogURL { get; set; }
        [DataMember]
        public string CustomizeImageURL { get; set; }
        [DataMember]
        public string Domain { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public int FollowerCount { get; set; }
        [DataMember]
        public int FriendCount { get; set; }
        [DataMember]
        public int StatusesCount { get; set; }
        [DataMember]
        public int FavouriteCount { get; set; }
        [DataMember]
        public string CreateTime { get; set; }
        [DataMember]
        public bool IsFollowing { get; set; }
        [DataMember]
        public bool IsVerified { get; set; }
        [DataMember]
        public bool IsEnableGeo { get; set; }
        [DataMember]
        public StatusList StatusItems { get; set; }
        #endregion

        #region Constructor
        public UserList() { }

        public static UserList Parse(XElement element)
        {
            UserList users = new UserList();
            users.UserID = GetElementValue(element, "id");
            users.TwitterName = GetElementValue(element, "screen_name");
            users.FriendName = GetElementValue(element, "name");
            users.Province = GetElementValue(element, "province");
            users.City = GetElementValue(element, "city");
            users.Location = GetElementValue(element, "location");
            users.Description = GetElementValue(element, "description");
            users.BlogURL = GetElementValue(element, "url");
            users.CustomizeImageURL = GetElementValue(element, "profile_image_url");
            users.Domain = GetElementValue(element, "domain");
            users.Gender = GetElementValue(element, "gender");
            users.FollowerCount = Convert.ToInt32(GetElementValue(element, "followers_count"));
            users.FriendCount = Convert.ToInt32(GetElementValue(element, "friends_count"));
            users.StatusesCount = Convert.ToInt32(GetElementValue(element, "statuses_count"));
            users.FavouriteCount = Convert.ToInt32(GetElementValue(element, "favourites_count"));
            users.CreateTime = GetElementValue(element, "created_at");
            users.IsFollowing = Convert.ToBoolean(GetElementValue(element, "following"));
            users.IsVerified = Convert.ToBoolean(GetElementValue(element, "verified"));
            users.IsEnableGeo = Convert.ToBoolean(GetElementValue(element, "geo_enabled"));
            var tmp = (from status in element.Descendants("user")
                       select status.Element("name").Value);
            foreach (var p in tmp)
            {
                string a = p.ToString();
            }

            StatusList statusitem = new StatusList();
            foreach (XElement subelement in element.Descendants("status"))
            {
                statusitem.CreateTime = subelement.Element(XName.Get("created_at")).Value;
                statusitem.TwitterID = subelement.Element(XName.Get("id")).Value;
                statusitem.TwitterContent = subelement.Element(XName.Get("text")).Value;
                statusitem.Source = subelement.Element(XName.Get("source")).Value;
                statusitem.IsFavorited = Convert.ToBoolean(subelement.Element(XName.Get("favorited")).Value);
                statusitem.IsTruncated = Convert.ToBoolean(subelement.Element(XName.Get("truncated")).Value);
                statusitem.GeoRSS = subelement.Element(XName.Get("geo")).Value;
                statusitem.ReplyStatusID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_status_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_status_id")).Value);
                statusitem.ReplyUserID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_user_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_user_id")).Value);
                statusitem.ReplyTwitterName = subelement.Element(XName.Get("in_reply_to_screen_name")).Value;

                if (subelement.XPathSelectElement("thumbnail_pic") != null)
                    statusitem.ThumbnailImageURL = subelement.Element(XName.Get("thumbnail_pic")).Value;

                if (subelement.XPathSelectElement("bmiddle_pic") != null)
                    statusitem.MiddleImageURL = subelement.Element(XName.Get("bmiddle_pic")).Value;

                if (subelement.XPathSelectElement("original_pic") != null)
                    statusitem.OriginalImageURL = subelement.Element(XName.Get("original_pic")).Value;

            }

            users.StatusItems = statusitem;


            return users;
        }

        //public static UserList Parse(XDocument doc)
        //{
        //    var users = from results in doc.Descendants("user")
        //                select new UserList
        //                {
        //                    UserID = results.Element("id").Value.ToString(),
        //                    TwitterName = results.Element("screen_name").Value.ToString(),
        //                    FriendName  = results.Element("name").Value.ToString(),
        //                    Province = results.Element("province").Value.ToString(),
        //                    City=results.Element("city").Value.ToString(),
        //                    Location=results.Element("location").Value.ToString(),
        //                    Description=results.Element("description").Value.ToString(),
        //                    BlogURL = results.Element("url").Value.ToString(),
        //                    CustomizeImageURL = results.Element("profile_image_url").Value.ToString(),
        //                    Domain = results.Element("domain").Value.ToString(),
        //                    Gender = results.Element("gender").Value.ToString(),
        //                    FollowerCount= Int32.Parse(results.Element("followers_count").Value),
        //                    FriendCount = Int32.Parse(results.Element("friends_count").Value),
        //                    StatusesCount= Int32.Parse(results.Element("statuses_count").Value),
        //                    FavouriteCount=Int32.Parse(results.Element("favourites_count").Value),
        //                    CreateTime = results.Element("created_at").Value.ToString(),
        //                    IsFollowing = Convert.ToBoolean(results.Element("following").Value),
        //                    IsVerified = Convert.ToBoolean(results.Element("verified").Value),
        //                    IsEnableGeo = Convert.ToBoolean(results.Element("geo_enabled").Value)
        //                };


        //    var tmpTbImageURL = doc.XPathSelectElement("//thumbnail_pic");

        //    var status = from tmpstatus in doc.Descendants("status")
        //                 select new StatusList
        //                 {
        //                             CreateTime = tmpstatus.Element("created_at").Value,
        //                             TwitterID = tmpstatus.Element("id").Value,
        //                             TwitterContent = tmpstatus.Element("text").Value,
        //                             Source = tmpstatus.Element("source").Value,
        //                             IsFavorited = Convert.ToBoolean(tmpstatus.Element(XName.Get("favorited")).Value),
        //                             IsTruncated = Convert.ToBoolean(tmpstatus.Element(XName.Get("truncated")).Value),
        //                             GeoRSS = tmpstatus.Element(XName.Get("geo")).Value,
        //                             ReplyStatusID = Convert.ToInt32((tmpstatus.Element(XName.Get("in_reply_to_status_id")).Value == "") ? "0" : tmpstatus.Element(XName.Get("in_reply_to_status_id")).Value),
        //                             ReplyUserID = Convert.ToInt32((tmpstatus.Element(XName.Get("in_reply_to_user_id")).Value == "") ? "0" : tmpstatus.Element(XName.Get("in_reply_to_user_id")).Value),
        //                             ReplyTwitterName = tmpstatus.Element(XName.Get("in_reply_to_screen_name")).Value,
        //                             //ThumbnailImageURL = tmpstatus.Element("thumbnail_pic").Value == null ? "" : tmpstatus.Element("thumbnail_pic").Value,
        //                             //MiddleImageURL = tmpstatus.Element("bmiddle_pic").Value == null ? "" : tmpstatus.Element("bmiddle_pic").Value,
        //                             //OriginalImageURL = tmpstatus.Element("original_pic").Value == null ? "" : tmpstatus.Element("original_pic").Value
        //                             //ThumbnailImageURL = tmpstatus.Elements("thumbnail_pic").Any() ? "" : tmpstatus.Element("thumbnail_pic").Value,

        //                             //MiddleImageURL = tmpstatus.Element("bmiddle_pic").Value,
        //                             //OriginalImageURL = tmpstatus.Element("original_pic").Value
        //                 };

        //    List<UserList> tmp = users.ToList();
        //    List<StatusList> tmps = status.ToList();

        //    return null;

        //    //UserList users = new UserList();
        //    //users.UserID = GetElementValue(element, "id");
        //    //users.TwitterName = GetElementValue(element, "screen_name");
        //    //users.FriendName = GetElementValue(element, "name");
        //    //users.Province = GetElementValue(element, "province");
        //    //users.City = GetElementValue(element, "city");
        //    //users.Location = GetElementValue(element, "location");
        //    //users.Description = GetElementValue(element, "description");
        //    //users.BlogURL = GetElementValue(element, "url");
        //    //users.CustomizeImageURL = GetElementValue(element, "profile_image_url");
        //    //users.Domain = GetElementValue(element, "domain");
        //    //users.Gender = GetElementValue(element, "gender");
        //    //users.FollowerCount = Convert.ToInt32(GetElementValue(element, "followers_count"));
        //    //users.FriendCount = Convert.ToInt32(GetElementValue(element, "friends_count"));
        //    //users.StatusesCount = Convert.ToInt32(GetElementValue(element, "statuses_count"));
        //    //users.FavouriteCount = Convert.ToInt32(GetElementValue(element, "favourites_count"));
        //    //users.CreateTime = GetElementValue(element, "created_at");
        //    //users.IsFollowing = Convert.ToBoolean(GetElementValue(element, "following"));
        //    //users.IsVerified = Convert.ToBoolean(GetElementValue(element, "verified"));
        //    //users.IsEnableGeo = Convert.ToBoolean(GetElementValue(element, "geo_enabled"));
        //    //users.StatusItems = null;

        //    //var users = from results in doc.Descendants("user")
        //    //            select new
        //    //            {
        //    //                UserID = (string)results.Attribute("id"),
        //    //                TwitterName = (string)results.Attribute("screen_name"),
        //    //                FriendName = (string)results.Attribute("name")
        //    //            };

        //    //foreach (var user in users)
        //    //{
        //    //    //UserList usercollection;

        //    //    string a = user.FriendName;
        //    //}
        //    //return null;
        //}

        #endregion

    }
    #endregion

    #region Followers Collection
    [DataContract()]
    public class FollowerList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public List<UserList> Users { get; set; }
        [DataMember]
        public string NextCursor { get; set; }
        [DataMember]
        public string PreviewCursor { get; set; }
        #endregion

        #region Constructor
        FollowerList() { }

        public static List<FollowerList> Parse(XElement element)
        {
            List<FollowerList> followercollection = new List<FollowerList>();

            List<UserList> usercollection = new List<UserList>();
            FollowerList followeritem = new FollowerList();

            foreach (XElement followernode in element.Elements())
            {
                UserList useritem = new UserList();

                useritem.UserID = GetElementValue(followernode, "id");
                useritem.TwitterName = GetElementValue(followernode, "screen_name");
                useritem.FriendName = GetElementValue(followernode, "name");
                useritem.Province = GetElementValue(followernode, "province");
                useritem.City = GetElementValue(followernode, "city");
                useritem.Location = GetElementValue(followernode, "location");
                useritem.Description = GetElementValue(followernode, "description");
                useritem.BlogURL = GetElementValue(followernode, "url");
                useritem.CustomizeImageURL = GetElementValue(followernode, "profile_image_url");
                useritem.Domain = GetElementValue(followernode, "domain");
                useritem.Gender = GetElementValue(followernode, "gender");
                useritem.FollowerCount = Convert.ToInt32(GetElementValue(followernode, "followers_count"));
                useritem.FriendCount = Convert.ToInt32(GetElementValue(followernode, "friends_count"));
                useritem.StatusesCount = Convert.ToInt32(GetElementValue(followernode, "statuses_count"));
                useritem.FavouriteCount = Convert.ToInt32(GetElementValue(followernode, "favourites_count"));
                useritem.CreateTime = GetElementValue(followernode, "created_at");
                useritem.IsFollowing = Convert.ToBoolean(GetElementValue(followernode, "following"));
                useritem.IsVerified = Convert.ToBoolean(GetElementValue(followernode, "verified"));
                useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(followernode, "geo_enabled"));

                StatusList statusitem = new StatusList();
                foreach (XElement subelement in followernode.Descendants("status"))
                {
                    statusitem.CreateTime = subelement.Element(XName.Get("created_at")).Value;
                    statusitem.TwitterID = subelement.Element(XName.Get("id")).Value;
                    statusitem.TwitterContent = subelement.Element(XName.Get("text")).Value;
                    statusitem.Source = subelement.Element(XName.Get("source")).Value;
                    statusitem.IsFavorited = Convert.ToBoolean(subelement.Element(XName.Get("favorited")).Value);
                    statusitem.IsTruncated = Convert.ToBoolean(subelement.Element(XName.Get("truncated")).Value);
                    statusitem.GeoRSS = subelement.Element(XName.Get("geo")).Value;
                    statusitem.ReplyStatusID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_status_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_status_id")).Value);
                    statusitem.ReplyUserID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_user_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_user_id")).Value);
                    statusitem.ReplyTwitterName = subelement.Element(XName.Get("in_reply_to_screen_name")).Value;

                    if (subelement.XPathSelectElement("thumbnail_pic") != null)
                        statusitem.ThumbnailImageURL = subelement.Element(XName.Get("thumbnail_pic")).Value;

                    if (subelement.XPathSelectElement("bmiddle_pic") != null)
                        statusitem.MiddleImageURL = subelement.Element(XName.Get("bmiddle_pic")).Value;

                    if (subelement.XPathSelectElement("original_pic") != null)
                        statusitem.OriginalImageURL = subelement.Element(XName.Get("original_pic")).Value;
                }

                useritem.StatusItems = statusitem;
                usercollection.Add(useritem);
            }

            followeritem.Users = usercollection;
            followeritem.NextCursor = GetElementValue(element, "next_cursor");
            followeritem.PreviewCursor = GetElementValue(element, "previous_cursor");

            followercollection.Add(followeritem);

            return followercollection;
        }
        #endregion

    }
    #endregion

    #region Status Collection
    [DataContract()]
    public class StatusList
    {
        #region Public Data Members
        [DataMember]
        public string CreateTime { get; set; }
        [DataMember]
        public string TwitterID { get; set; }
        [DataMember]
        public string TwitterContent { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public bool IsFavorited { get; set; }
        [DataMember]
        public bool IsTruncated { get; set; }
        [DataMember]
        public string GeoRSS { get; set; }
        [DataMember]
        public int ReplyStatusID { get; set; }
        [DataMember]
        public int ReplyUserID { get; set; }
        [DataMember]
        public string ReplyTwitterName { get; set; }
        [DataMember]
        public string ThumbnailImageURL { get; set; }
        [DataMember]
        public string MiddleImageURL { get; set; }
        [DataMember]
        public string OriginalImageURL { get; set; }
        #endregion

    }
    #endregion

    #region Friend Timeline Collection
    [DataContract()]
    public class FriendTimelineList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public DateTime CreatedTime { get; set; }
        [DataMember]
        public string FriendTwitterID { get; set; }
        [DataMember]
        public string FriendTwitterContent { get; set; }
        [DataMember]
        public string SourceURL { get; set; }
        [DataMember]
        public bool IsFavorited { get; set; }
        [DataMember]
        public bool IsTruncated { get; set; }
        [DataMember]
        public string Geo { get; set; }
        [DataMember]
        public int ReplyToStatusID { get; set; }
        [DataMember]
        public int ReplyToUserID { get; set; }
        [DataMember]
        public string ReplyToScreenName { get; set; }
        [DataMember]
        public string ThumbnailPic { get; set; }
        [DataMember]
        public string MiddleSizePic { get; set; }
        [DataMember]
        public string OriginalSizePic { get; set; }
        [DataMember]
        public UserList UsersItem { get; set; }
        [DataMember]
        public RetweeterList RetweeterItem { get; set; }
        [DataMember]
        public bool IsShowThumbPic { get; set; }
        [DataMember]
        public bool IsShowRetweet { get; set; }
        #endregion

        #region Constructor
        private FriendTimelineList() { }

        public static List<FriendTimelineList> Parse(XElement element)
        {
            List<FriendTimelineList> friendtimecollection = new List<FriendTimelineList>();

            foreach (XElement friendtimelinenode in element.Elements())
            {
                FriendTimelineList friendtimelines = new FriendTimelineList();

                friendtimelines.CreatedTime = ParseDateTime(GetElementValue(friendtimelinenode, "created_at"));
                friendtimelines.FriendTwitterID = GetElementValue(friendtimelinenode, "id");
                friendtimelines.FriendTwitterContent = GetElementValue(friendtimelinenode, "text");
                friendtimelines.SourceURL = GetElementValue(friendtimelinenode, "source");
                friendtimelines.IsFavorited = Convert.ToBoolean(GetElementValue(friendtimelinenode, "favorited"));
                friendtimelines.IsTruncated = Convert.ToBoolean(GetElementValue(friendtimelinenode, "truncated"));
                friendtimelines.Geo = GetElementValue(friendtimelinenode, "geo");

                friendtimelines.ReplyToStatusID = (GetElementValue(friendtimelinenode, "in_reply_to_status_id") == "" ? 0 : Convert.ToInt32(GetElementValue(friendtimelinenode, "in_reply_to_status_id")));
                friendtimelines.ReplyToUserID = (GetElementValue(friendtimelinenode, "in_reply_to_user_id") == "" ? 0 : Convert.ToInt32(GetElementValue(friendtimelinenode, "in_reply_to_user_id")));
                friendtimelines.ReplyToScreenName = GetElementValue(friendtimelinenode, "in_reply_to_screen_name");

                //friendtimelines.IsShowThumbPic = false;
                if (friendtimelinenode.XPathSelectElement("thumbnail_pic") != null)
                {
                    friendtimelines.IsShowThumbPic = true;
                    friendtimelines.ThumbnailPic = GetElementValue(friendtimelinenode, "thumbnail_pic");
                }
                else
                    friendtimelines.IsShowThumbPic = false;


                if (friendtimelinenode.XPathSelectElement("bmiddle_pic") != null)
                    friendtimelines.MiddleSizePic = GetElementValue(friendtimelinenode, "bmiddle_pic");

                if (friendtimelinenode.XPathSelectElement("original_pic") != null)
                    friendtimelines.OriginalSizePic = GetElementValue(friendtimelinenode, "original_pic");

                //friendtimelines.ThumbnailPic = friendtimelinenode.XPathSelectElement("//thumbnail_pic") == null ? "" : GetElementValue(friendtimelinenode, "thumbnail_pic");
                //friendtimelines.MiddleSizePic = friendtimelinenode.XPathSelectElement("//bmiddle_pic") == null ? "" : GetElementValue(friendtimelinenode, "bmiddle_pic");
                //friendtimelines.OriginalSizePic = friendtimelinenode.XPathSelectElement("//original_pic") == null ? "" : GetElementValue(friendtimelinenode, "original_pic");

                UserList useritem = new UserList();
                useritem.UserID = GetElementValue(friendtimelinenode.Element("user"), "id");
                useritem.TwitterName = GetElementValue(friendtimelinenode.Element("user"), "screen_name");
                useritem.FriendName = GetElementValue(friendtimelinenode.Element("user"), "name");
                useritem.Province = GetElementValue(friendtimelinenode.Element("user"), "province");
                useritem.City = GetElementValue(friendtimelinenode.Element("user"), "city");
                useritem.Location = GetElementValue(friendtimelinenode.Element("user"), "location");
                useritem.Description = GetElementValue(friendtimelinenode.Element("user"), "description");
                useritem.BlogURL = GetElementValue(friendtimelinenode.Element("user"), "url");
                useritem.CustomizeImageURL = GetElementValue(friendtimelinenode.Element("user"), "profile_image_url");
                useritem.Domain = GetElementValue(friendtimelinenode.Element("user"), "domain");
                useritem.Gender = GetElementValue(friendtimelinenode.Element("user"), "gender");
                useritem.FollowerCount = Convert.ToInt32(GetElementValue(friendtimelinenode.Element("user"), "followers_count"));
                useritem.FriendCount = Convert.ToInt32(GetElementValue(friendtimelinenode.Element("user"), "friends_count"));
                useritem.StatusesCount = Convert.ToInt32(GetElementValue(friendtimelinenode.Element("user"), "statuses_count"));
                useritem.FavouriteCount = Convert.ToInt32(GetElementValue(friendtimelinenode.Element("user"), "favourites_count"));
                useritem.CreateTime = GetElementValue(friendtimelinenode.Element("user"), "created_at");
                useritem.IsFollowing = Convert.ToBoolean(GetElementValue(friendtimelinenode.Element("user"), "following"));
                useritem.IsVerified = Convert.ToBoolean(GetElementValue(friendtimelinenode.Element("user"), "verified"));
                useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(friendtimelinenode.Element("user"), "geo_enabled"));
                friendtimelines.UsersItem = useritem;

                RetweeterList retweeteritem = new RetweeterList();


                //retweeteritem.CreatedTime = GetElementValue(friendtimelinenode.Element("retweeted_status"), "created_at");
                //retweeteritem.RetweeterID = GetElementValue(friendtimelinenode.Element("retweeted_status"), "id");
                //retweeteritem.RetweeterContent = GetElementValue(friendtimelinenode.Element("retweeted_status"), "text");
                //retweeteritem.SourceURL = GetElementValue(friendtimelinenode.Element("retweeted_status"), "source");
                //retweeteritem.IsFavorited = Convert.ToBoolean(GetElementValue(friendtimelinenode.Element("retweeted_status"), "favorited"));
                //retweeteritem.IsTruncated = Convert.ToBoolean(GetElementValue(friendtimelinenode.Element("retweeted_status"), "truncated"));
                //retweeteritem.Geo = GetElementValue(friendtimelinenode.Element("retweeted_status"), "geo");
                //var tmpReplyStatusID = GetElementValue(friendtimelinenode.Element("retweeted_status"), "in_reply_to_status_id");
                //retweeteritem.ReplyStatusID = Convert.ToInt32((tmpReplyStatusID == "") ? "0" : tmpReplyStatusID); 

                //retweeteritem.ReplyUserID =Convert.ToInt32(GetElementValue(friendtimelinenode.Element("retweeted_status"), "in_reply_to_user_id"));
                //retweeteritem.ReplyToScreenName = GetElementValue(friendtimelinenode.Element("retweeted_status"), "in_reply_to_status_id");
                //retweeteritem.ThumbnailImageURL = GetElementValue(friendtimelinenode.Element("retweeted_status"), "thumbnail_pic");
                //retweeteritem.MiddleImageURL = GetElementValue(friendtimelinenode.Element("retweeted_status"), "bmiddle_pic");
                //retweeteritem.OriginalImageURL = GetElementValue(friendtimelinenode.Element("retweeted_status"), "original_pic");

                foreach (XElement subelement in friendtimelinenode.Descendants("retweeted_status"))
                {
                    retweeteritem.CreatedTime = subelement.Element(XName.Get("created_at")).Value;
                    retweeteritem.RetweeterID = subelement.Element(XName.Get("id")).Value;
                    retweeteritem.RetweeterContent = subelement.Element(XName.Get("text")).Value;
                    retweeteritem.SourceURL = subelement.Element(XName.Get("source")).Value;
                    retweeteritem.IsFavorited = Convert.ToBoolean(subelement.Element(XName.Get("favorited")).Value);
                    retweeteritem.IsTruncated = Convert.ToBoolean(subelement.Element(XName.Get("truncated")).Value);
                    retweeteritem.Geo = subelement.Element(XName.Get("geo")).Value;
                    retweeteritem.ReplyStatusID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_status_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_status_id")).Value);
                    retweeteritem.ReplyUserID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_user_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_user_id")).Value);
                    retweeteritem.ReplyToScreenName = subelement.Element(XName.Get("in_reply_to_screen_name")).Value;
                    if (subelement.XPathSelectElement("thumbnail_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.ThumbnailImageURL = subelement.Element(XName.Get("thumbnail_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    if (subelement.XPathSelectElement("bmiddle_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.MiddleImageURL = subelement.Element(XName.Get("bmiddle_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    if (subelement.XPathSelectElement("original_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.OriginalImageURL = subelement.Element(XName.Get("original_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    //retweeteritem.ThumbnailImageURL = subelement.XPathSelectElement("//thumbnail_pic") == null ? "" : subelement.Element(XName.Get("thumbnail_pic")).Value;
                    //retweeteritem.MiddleImageURL = subelement.XPathSelectElement("//bmiddle_pic") == null ? "" : subelement.Element(XName.Get("bmiddle_pic")).Value;
                    //retweeteritem.OriginalImageURL = subelement.XPathSelectElement("//original_pic") == null ? "" : subelement.Element(XName.Get("original_pic")).Value;

                    UserList subuseritem = new UserList();
                    subuseritem.UserID = GetElementValue(subelement.Element("user"), "id");
                    subuseritem.TwitterName = GetElementValue(subelement.Element("user"), "screen_name");
                    subuseritem.FriendName = GetElementValue(subelement.Element("user"), "name");
                    subuseritem.Province = GetElementValue(subelement.Element("user"), "province");
                    subuseritem.City = GetElementValue(subelement.Element("user"), "city");
                    subuseritem.Location = GetElementValue(subelement.Element("user"), "location");
                    subuseritem.Description = GetElementValue(subelement.Element("user"), "description");
                    subuseritem.BlogURL = GetElementValue(subelement.Element("user"), "url");
                    subuseritem.CustomizeImageURL = GetElementValue(subelement.Element("user"), "profile_image_url");
                    subuseritem.Domain = GetElementValue(subelement.Element("user"), "domain");
                    subuseritem.Gender = GetElementValue(subelement.Element("user"), "gender");
                    subuseritem.FollowerCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "followers_count"));
                    subuseritem.FriendCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "friends_count"));
                    subuseritem.StatusesCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "statuses_count"));
                    subuseritem.FavouriteCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "favourites_count"));
                    subuseritem.CreateTime = GetElementValue(subelement.Element("user"), "created_at");
                    subuseritem.IsFollowing = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "following"));
                    subuseritem.IsVerified = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "verified"));
                    subuseritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "geo_enabled"));
                    retweeteritem.UsersItem = subuseritem;

                    friendtimelines.RetweeterItem = retweeteritem;
                }



                if (friendtimelines.RetweeterItem == null)
                    friendtimelines.IsShowRetweet = false;
                else
                {
                    friendtimelines.IsShowRetweet = true;
                }



                //foreach (XElement usernode in friendtimelinenode.Element("user").Elements())
                //{
                //    UserList useritem = new UserList();
                //    useritem.UserID = GetElementValue(usernode, "id");
                //    //useritem.TwitterName = GetElementValue(usernode, "screen_name");
                //    //useritem.FriendName = GetElementValue(usernode, "name");
                //    //useritem.Province = GetElementValue(usernode, "province");
                //    //useritem.City = GetElementValue(usernode, "city");
                //    //useritem.Location = GetElementValue(usernode, "location");
                //    //useritem.Description = GetElementValue(usernode, "description");
                //    //useritem.BlogURL = GetElementValue(usernode, "url");
                //    //useritem.CustomizeImageURL = GetElementValue(usernode, "profile_image_url");
                //    //useritem.Domain = GetElementValue(usernode, "domain");
                //    //useritem.Gender = GetElementValue(usernode, "gender");
                //    //useritem.FollowerCount = Convert.ToInt32(GetElementValue(usernode, "followers_count"));
                //    //useritem.FriendCount = Convert.ToInt32(GetElementValue(usernode, "friends_count"));
                //    //useritem.StatusesCount = Convert.ToInt32(GetElementValue(usernode, "statuses_count"));
                //    //useritem.FavouriteCount = Convert.ToInt32(GetElementValue(usernode, "favourites_count"));
                //    //useritem.CreateTime = GetElementValue(usernode, "created_at");
                //    //useritem.IsFollowing = Convert.ToBoolean(GetElementValue(usernode, "following"));
                //    //useritem.IsVerified = Convert.ToBoolean(GetElementValue(usernode, "verified"));
                //    //useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(usernode, "geo_enabled"));
                //    friendtimelines.UsersItem = useritem;
                //}



                friendtimecollection.Add(friendtimelines);
            }

            //friendtimelines.CreatedTime = GetGroupElementValue(element, "created_at");
            ////friendtimelines.CreatedTime = GetElementValue(GetElementValue(element, "created_at"),"status");
            //friendtimelines.FriendTwitterID = GetGroupElementValue(element, "id");
            //friendtimelines.FriendTwitterContent = GetGroupElementValue(element, "text");
            //friendtimelines.SourceURL = GetGroupElementValue(element, "source");
            //friendtimelines.IsFavorited = Convert.ToBoolean(GetGroupElementValue(element, "favorited"));
            //friendtimelines.IsTruncated = Convert.ToBoolean(GetGroupElementValue(element, "truncated"));
            //friendtimelines.Geo = GetGroupElementValue(element, "geo");

            //friendtimelines.ReplyToStatusID = (GetGroupElementValue(element, "in_reply_to_status_id") == "" ? 0 : Convert.ToInt32(GetGroupElementValue(element, "in_reply_to_status_id")));
            //friendtimelines.ReplyToUserID = (GetGroupElementValue(element, "in_reply_to_user_id") == "" ? 0 : Convert.ToInt32(GetGroupElementValue(element, "in_reply_to_user_id")));
            //friendtimelines.ReplyToScreenName = GetGroupElementValue(element, "in_reply_to_screen_name");
            //friendtimelines.UsersItem

            //UserList userInfo = new UserList();


            //var tmpURL = (from status in element.Descendants("")

            return friendtimecollection;
        }
        #endregion

        static DateTime ParseDateTime(string date)
        {
            string dayOfWeek = date.Substring(0, 3).Trim();
            string month = date.Substring(4, 3).Trim();
            string dayInMonth = date.Substring(8, 2).Trim();
            string time = date.Substring(11, 9).Trim();
            string offset = date.Substring(20, 5).Trim();
            string year = date.Substring(25, 5).Trim();
            string dateTime = string.Format("{0}-{1}-{2}   {3}", dayInMonth, month, year, time);
            DateTime ret = DateTime.Parse(dateTime);
            return ret;
        }
    }
    #endregion

    #region Favorite Collection
    [DataContract()]
    public class FavoriteList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public DateTime CreatedTime { get; set; }
        [DataMember]
        public string FriendTwitterID { get; set; }
        [DataMember]
        public string FriendTwitterContent { get; set; }
        [DataMember]
        public string SourceURL { get; set; }
        [DataMember]
        public bool IsFavorited { get; set; }
        [DataMember]
        public bool IsTruncated { get; set; }
        [DataMember]
        public string Geo { get; set; }
        [DataMember]
        public int ReplyToStatusID { get; set; }
        [DataMember]
        public int ReplyToUserID { get; set; }
        [DataMember]
        public string ReplyToScreenName { get; set; }
        [DataMember]
        public string ThumbnailPic { get; set; }
        [DataMember]
        public string MiddleSizePic { get; set; }
        [DataMember]
        public string OriginalSizePic { get; set; }
        [DataMember]
        public UserList UsersItem { get; set; }
        [DataMember]
        public RetweeterList RetweeterItem { get; set; }
        [DataMember]
        public bool IsShowThumbPic { get; set; }
        [DataMember]
        public bool IsShowRetweet { get; set; }
        #endregion

        #region Constructor
        public FavoriteList() { }

        public static FavoriteList Parse(XElement element, int Status)
        {
            FavoriteList favorites = new FavoriteList();

            favorites.CreatedTime = ParseDateTime(GetElementValue(element, "created_at"));
            favorites.FriendTwitterID = GetElementValue(element, "id");
            favorites.FriendTwitterContent = GetElementValue(element, "text");
            favorites.SourceURL = GetElementValue(element, "source");
            favorites.IsFavorited = Convert.ToBoolean(GetElementValue(element, "favorited"));
            favorites.IsTruncated = Convert.ToBoolean(GetElementValue(element, "truncated"));
            favorites.Geo = GetElementValue(element, "geo");

            favorites.ReplyToStatusID = (GetElementValue(element, "in_reply_to_status_id") == "" ? 0 : Convert.ToInt32(GetElementValue(element, "in_reply_to_status_id")));
            favorites.ReplyToUserID = (GetElementValue(element, "in_reply_to_user_id") == "" ? 0 : Convert.ToInt32(GetElementValue(element, "in_reply_to_user_id")));
            favorites.ReplyToScreenName = GetElementValue(element, "in_reply_to_screen_name");

            if (element.XPathSelectElement("thumbnail_pic") != null)
            {
                favorites.IsShowThumbPic = true;
                favorites.ThumbnailPic = GetElementValue(element, "thumbnail_pic");
            }
            else
                favorites.IsShowThumbPic = false;


            if (element.XPathSelectElement("bmiddle_pic") != null)
                favorites.MiddleSizePic = GetElementValue(element, "bmiddle_pic");

            if (element.XPathSelectElement("original_pic") != null)
                favorites.OriginalSizePic = GetElementValue(element, "original_pic");

            UserList useritem = new UserList();
            useritem.UserID = GetElementValue(element.Element("user"), "id");
            useritem.TwitterName = GetElementValue(element.Element("user"), "screen_name");
            useritem.FriendName = GetElementValue(element.Element("user"), "name");
            useritem.Province = GetElementValue(element.Element("user"), "province");
            useritem.City = GetElementValue(element.Element("user"), "city");
            useritem.Location = GetElementValue(element.Element("user"), "location");
            useritem.Description = GetElementValue(element.Element("user"), "description");
            useritem.BlogURL = GetElementValue(element.Element("user"), "url");
            useritem.CustomizeImageURL = GetElementValue(element.Element("user"), "profile_image_url");
            useritem.Domain = GetElementValue(element.Element("user"), "domain");
            useritem.Gender = GetElementValue(element.Element("user"), "gender");
            useritem.FollowerCount = Convert.ToInt32(GetElementValue(element.Element("user"), "followers_count"));
            useritem.FriendCount = Convert.ToInt32(GetElementValue(element.Element("user"), "friends_count"));
            useritem.StatusesCount = Convert.ToInt32(GetElementValue(element.Element("user"), "statuses_count"));
            useritem.FavouriteCount = Convert.ToInt32(GetElementValue(element.Element("user"), "favourites_count"));
            useritem.CreateTime = GetElementValue(element.Element("user"), "created_at");
            useritem.IsFollowing = Convert.ToBoolean(GetElementValue(element.Element("user"), "following"));
            useritem.IsVerified = Convert.ToBoolean(GetElementValue(element.Element("user"), "verified"));
            useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(element.Element("user"), "geo_enabled"));
            favorites.UsersItem = useritem;

            RetweeterList retweeteritem = new RetweeterList();

            foreach (XElement subelement in element.Descendants("retweeted_status"))
            {
                retweeteritem.CreatedTime = subelement.Element(XName.Get("created_at")).Value;
                retweeteritem.RetweeterID = subelement.Element(XName.Get("id")).Value;
                retweeteritem.RetweeterContent = subelement.Element(XName.Get("text")).Value;
                retweeteritem.SourceURL = subelement.Element(XName.Get("source")).Value;
                retweeteritem.IsFavorited = Convert.ToBoolean(subelement.Element(XName.Get("favorited")).Value);
                retweeteritem.IsTruncated = Convert.ToBoolean(subelement.Element(XName.Get("truncated")).Value);
                retweeteritem.Geo = subelement.Element(XName.Get("geo")).Value;
                retweeteritem.ReplyStatusID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_status_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_status_id")).Value);
                retweeteritem.ReplyUserID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_user_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_user_id")).Value);
                retweeteritem.ReplyToScreenName = subelement.Element(XName.Get("in_reply_to_screen_name")).Value;
                if (subelement.XPathSelectElement("thumbnail_pic") != null)
                {
                    retweeteritem.IsShowThumbPic = true;
                    retweeteritem.ThumbnailImageURL = subelement.Element(XName.Get("thumbnail_pic")).Value;
                }
                else
                    retweeteritem.IsShowThumbPic = false;

                if (subelement.XPathSelectElement("bmiddle_pic") != null)
                {
                    retweeteritem.IsShowThumbPic = true;
                    retweeteritem.MiddleImageURL = subelement.Element(XName.Get("bmiddle_pic")).Value;
                }
                else
                    retweeteritem.IsShowThumbPic = false;

                if (subelement.XPathSelectElement("original_pic") != null)
                {
                    retweeteritem.IsShowThumbPic = true;
                    retweeteritem.OriginalImageURL = subelement.Element(XName.Get("original_pic")).Value;
                }
                else
                    retweeteritem.IsShowThumbPic = false;

                UserList subuseritem = new UserList();
                subuseritem.UserID = GetElementValue(subelement.Element("user"), "id");
                subuseritem.TwitterName = GetElementValue(subelement.Element("user"), "screen_name");
                subuseritem.FriendName = GetElementValue(subelement.Element("user"), "name");
                subuseritem.Province = GetElementValue(subelement.Element("user"), "province");
                subuseritem.City = GetElementValue(subelement.Element("user"), "city");
                subuseritem.Location = GetElementValue(subelement.Element("user"), "location");
                subuseritem.Description = GetElementValue(subelement.Element("user"), "description");
                subuseritem.BlogURL = GetElementValue(subelement.Element("user"), "url");
                subuseritem.CustomizeImageURL = GetElementValue(subelement.Element("user"), "profile_image_url");
                subuseritem.Domain = GetElementValue(subelement.Element("user"), "domain");
                subuseritem.Gender = GetElementValue(subelement.Element("user"), "gender");
                subuseritem.FollowerCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "followers_count"));
                subuseritem.FriendCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "friends_count"));
                subuseritem.StatusesCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "statuses_count"));
                subuseritem.FavouriteCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "favourites_count"));
                subuseritem.CreateTime = GetElementValue(subelement.Element("user"), "created_at");
                subuseritem.IsFollowing = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "following"));
                subuseritem.IsVerified = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "verified"));
                subuseritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "geo_enabled"));
                retweeteritem.UsersItem = subuseritem;

                favorites.RetweeterItem = retweeteritem;
            }



            if (favorites.RetweeterItem == null)
                favorites.IsShowRetweet = false;
            else
            {
                favorites.IsShowRetweet = true;
            }

            return favorites;
        }

        public static List<FavoriteList> Parse(XElement element)
        {
            List<FavoriteList> favoritecollection = new List<FavoriteList>();

            foreach (XElement favoritenode in element.Elements())
            {
                FavoriteList favorites = new FavoriteList();

                favorites.CreatedTime = ParseDateTime(GetElementValue(favoritenode, "created_at"));
                favorites.FriendTwitterID = GetElementValue(favoritenode, "id");
                favorites.FriendTwitterContent = GetElementValue(favoritenode, "text");
                favorites.SourceURL = GetElementValue(favoritenode, "source");
                favorites.IsFavorited = Convert.ToBoolean(GetElementValue(favoritenode, "favorited"));
                favorites.IsTruncated = Convert.ToBoolean(GetElementValue(favoritenode, "truncated"));
                favorites.Geo = GetElementValue(favoritenode, "geo");

                favorites.ReplyToStatusID = (GetElementValue(favoritenode, "in_reply_to_status_id") == "" ? 0 : Convert.ToInt32(GetElementValue(favoritenode, "in_reply_to_status_id")));
                favorites.ReplyToUserID = (GetElementValue(favoritenode, "in_reply_to_user_id") == "" ? 0 : Convert.ToInt32(GetElementValue(favoritenode, "in_reply_to_user_id")));
                favorites.ReplyToScreenName = GetElementValue(favoritenode, "in_reply_to_screen_name");
         
                if (favoritenode.XPathSelectElement("thumbnail_pic") != null)
                {
                    favorites.IsShowThumbPic = true;
                    favorites.ThumbnailPic = GetElementValue(favoritenode, "thumbnail_pic");
                }
                else
                    favorites.IsShowThumbPic = false;


                if (favoritenode.XPathSelectElement("bmiddle_pic") != null)
                    favorites.MiddleSizePic = GetElementValue(favoritenode, "bmiddle_pic");

                if (favoritenode.XPathSelectElement("original_pic") != null)
                    favorites.OriginalSizePic = GetElementValue(favoritenode, "original_pic");

                UserList useritem = new UserList();
                useritem.UserID = GetElementValue(favoritenode.Element("user"), "id");
                useritem.TwitterName = GetElementValue(favoritenode.Element("user"), "screen_name");
                useritem.FriendName = GetElementValue(favoritenode.Element("user"), "name");
                useritem.Province = GetElementValue(favoritenode.Element("user"), "province");
                useritem.City = GetElementValue(favoritenode.Element("user"), "city");
                useritem.Location = GetElementValue(favoritenode.Element("user"), "location");
                useritem.Description = GetElementValue(favoritenode.Element("user"), "description");
                useritem.BlogURL = GetElementValue(favoritenode.Element("user"), "url");
                useritem.CustomizeImageURL = GetElementValue(favoritenode.Element("user"), "profile_image_url");
                useritem.Domain = GetElementValue(favoritenode.Element("user"), "domain");
                useritem.Gender = GetElementValue(favoritenode.Element("user"), "gender");
                useritem.FollowerCount = Convert.ToInt32(GetElementValue(favoritenode.Element("user"), "followers_count"));
                useritem.FriendCount = Convert.ToInt32(GetElementValue(favoritenode.Element("user"), "friends_count"));
                useritem.StatusesCount = Convert.ToInt32(GetElementValue(favoritenode.Element("user"), "statuses_count"));
                useritem.FavouriteCount = Convert.ToInt32(GetElementValue(favoritenode.Element("user"), "favourites_count"));
                useritem.CreateTime = GetElementValue(favoritenode.Element("user"), "created_at");
                useritem.IsFollowing = Convert.ToBoolean(GetElementValue(favoritenode.Element("user"), "following"));
                useritem.IsVerified = Convert.ToBoolean(GetElementValue(favoritenode.Element("user"), "verified"));
                useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(favoritenode.Element("user"), "geo_enabled"));
                favorites.UsersItem = useritem;

                RetweeterList retweeteritem = new RetweeterList();

                foreach (XElement subelement in favoritenode.Descendants("retweeted_status"))
                {
                    retweeteritem.CreatedTime = subelement.Element(XName.Get("created_at")).Value;
                    retweeteritem.RetweeterID = subelement.Element(XName.Get("id")).Value;
                    retweeteritem.RetweeterContent = subelement.Element(XName.Get("text")).Value;
                    retweeteritem.SourceURL = subelement.Element(XName.Get("source")).Value;
                    retweeteritem.IsFavorited = Convert.ToBoolean(subelement.Element(XName.Get("favorited")).Value);
                    retweeteritem.IsTruncated = Convert.ToBoolean(subelement.Element(XName.Get("truncated")).Value);
                    retweeteritem.Geo = subelement.Element(XName.Get("geo")).Value;
                    retweeteritem.ReplyStatusID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_status_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_status_id")).Value);
                    retweeteritem.ReplyUserID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_user_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_user_id")).Value);
                    retweeteritem.ReplyToScreenName = subelement.Element(XName.Get("in_reply_to_screen_name")).Value;
                    if (subelement.XPathSelectElement("thumbnail_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.ThumbnailImageURL = subelement.Element(XName.Get("thumbnail_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    if (subelement.XPathSelectElement("bmiddle_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.MiddleImageURL = subelement.Element(XName.Get("bmiddle_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    if (subelement.XPathSelectElement("original_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.OriginalImageURL = subelement.Element(XName.Get("original_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    UserList subuseritem = new UserList();
                    subuseritem.UserID = GetElementValue(subelement.Element("user"), "id");
                    subuseritem.TwitterName = GetElementValue(subelement.Element("user"), "screen_name");
                    subuseritem.FriendName = GetElementValue(subelement.Element("user"), "name");
                    subuseritem.Province = GetElementValue(subelement.Element("user"), "province");
                    subuseritem.City = GetElementValue(subelement.Element("user"), "city");
                    subuseritem.Location = GetElementValue(subelement.Element("user"), "location");
                    subuseritem.Description = GetElementValue(subelement.Element("user"), "description");
                    subuseritem.BlogURL = GetElementValue(subelement.Element("user"), "url");
                    subuseritem.CustomizeImageURL = GetElementValue(subelement.Element("user"), "profile_image_url");
                    subuseritem.Domain = GetElementValue(subelement.Element("user"), "domain");
                    subuseritem.Gender = GetElementValue(subelement.Element("user"), "gender");
                    subuseritem.FollowerCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "followers_count"));
                    subuseritem.FriendCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "friends_count"));
                    subuseritem.StatusesCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "statuses_count"));
                    subuseritem.FavouriteCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "favourites_count"));
                    subuseritem.CreateTime = GetElementValue(subelement.Element("user"), "created_at");
                    subuseritem.IsFollowing = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "following"));
                    subuseritem.IsVerified = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "verified"));
                    subuseritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "geo_enabled"));
                    retweeteritem.UsersItem = subuseritem;

                    favorites.RetweeterItem = retweeteritem;
                }



                if (favorites.RetweeterItem == null)
                    favorites.IsShowRetweet = false;
                else
                {
                    favorites.IsShowRetweet = true;
                }

                favoritecollection.Add(favorites);
            }

            return favoritecollection;
        }
        #endregion

        static DateTime ParseDateTime(string date)
        {
                string dayOfWeek = date.Substring(0, 3).Trim();
                string month = date.Substring(4, 3).Trim();
                string dayInMonth = date.Substring(8, 2).Trim();
                string time = date.Substring(11, 9).Trim();
                string offset = date.Substring(20, 5).Trim();
                string year = date.Substring(25, 5).Trim();
                string dateTime = string.Format("{0}-{1}-{2}   {3}", dayInMonth, month, year, time);
                DateTime ret = DateTime.Parse(dateTime);
                return ret;
        }
    }
    
    #endregion

    #region Comment Timeline Collection
    [DataContract()]
    public class CommentTimelineList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public DateTime CreatedTime { get; set; }
        [DataMember]
        public string CommentID { get; set; }
        [DataMember]
        public string CommentContent { get; set; }
        [DataMember]
        public UserList UsersItem { get; set; }
        [DataMember]
        public StatusList StatusItem { get; set; }
        #endregion

        #region Constructor
        public CommentTimelineList() { }

        public static List<CommentTimelineList> Parse(XElement element)
        {
            List<CommentTimelineList> commentcollection = new List<CommentTimelineList>();

            foreach (XElement commentnode in element.Elements())
            {
                CommentTimelineList comments = new CommentTimelineList();

                comments.CreatedTime = ParseDateTime(GetElementValue(commentnode, "created_at"));
                comments.CommentID = GetElementValue(commentnode, "id");
                comments.CommentContent = GetElementValue(commentnode, "text");

                UserList useritem = new UserList();
                useritem.UserID = GetElementValue(commentnode.Element("user"), "id");
                useritem.TwitterName = GetElementValue(commentnode.Element("user"), "screen_name");
                useritem.FriendName = GetElementValue(commentnode.Element("user"), "name");
                useritem.Province = GetElementValue(commentnode.Element("user"), "province");
                useritem.City = GetElementValue(commentnode.Element("user"), "city");
                useritem.Location = GetElementValue(commentnode.Element("user"), "location");
                useritem.Description = GetElementValue(commentnode.Element("user"), "description");
                useritem.BlogURL = GetElementValue(commentnode.Element("user"), "url");
                useritem.CustomizeImageURL = GetElementValue(commentnode.Element("user"), "profile_image_url");
                useritem.Domain = GetElementValue(commentnode.Element("user"), "domain");
                useritem.Gender = GetElementValue(commentnode.Element("user"), "gender");
                useritem.FollowerCount = Convert.ToInt32(GetElementValue(commentnode.Element("user"), "followers_count"));
                useritem.FriendCount = Convert.ToInt32(GetElementValue(commentnode.Element("user"), "friends_count"));
                useritem.StatusesCount = Convert.ToInt32(GetElementValue(commentnode.Element("user"), "statuses_count"));
                useritem.FavouriteCount = Convert.ToInt32(GetElementValue(commentnode.Element("user"), "favourites_count"));
                useritem.CreateTime = GetElementValue(commentnode.Element("user"), "created_at");
                useritem.IsFollowing = Convert.ToBoolean(GetElementValue(commentnode.Element("user"), "following"));
                useritem.IsVerified = Convert.ToBoolean(GetElementValue(commentnode.Element("user"), "verified"));
                useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(commentnode.Element("user"), "geo_enabled"));
                comments.UsersItem = useritem;

                StatusList statusitem = new StatusList();
                statusitem.TwitterID = GetElementValue(commentnode.Element("status"), "id");
                comments.StatusItem = statusitem;

                commentcollection.Add(comments);
            }

            return commentcollection;
        }
        #endregion


        static DateTime ParseDateTime(string date)
        {
            string dayOfWeek = date.Substring(0, 3).Trim();
            string month = date.Substring(4, 3).Trim();
            string dayInMonth = date.Substring(8, 2).Trim();
            string time = date.Substring(11, 9).Trim();
            string offset = date.Substring(20, 5).Trim();
            string year = date.Substring(25, 5).Trim();
            string dateTime = string.Format("{0}-{1}-{2}   {3}", dayInMonth, month, year, time);
            DateTime ret = DateTime.Parse(dateTime);
            return ret;
        }
    }
    #endregion

    #region Retweeter Status
    [DataContract()]
    public class RetweeterList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public string CreatedTime { get; set; }
        [DataMember]
        public string RetweeterID { get; set; }
        [DataMember]
        public string RetweeterContent { get; set; }
        [DataMember]
        public string SourceURL { get; set; }
        [DataMember]
        public bool IsFavorited { get; set; }
        [DataMember]
        public bool IsTruncated { get; set; }
        [DataMember]
        public string Geo { get; set; }
        [DataMember]
        public int ReplyStatusID { get; set; }
        [DataMember]
        public int ReplyUserID { get; set; }
        [DataMember]
        public string ReplyToScreenName { get; set; }
        [DataMember]
        public string ThumbnailImageURL { get; set; }
        [DataMember]
        public string MiddleImageURL { get; set; }
        [DataMember]
        public string OriginalImageURL { get; set; }
        [DataMember]
        public UserList UsersItem { get; set; }
        [DataMember]
        public bool IsShowThumbPic { get; set; }
        #endregion

        #region Constructor
        public RetweeterList() { }

        //public static List<RetweeterList> Parse(XElement element)
        //{
        //    List<RetweeterList> retweetercollection = new List<RetweeterList>();

        //    foreach (XElement retweeternode in element.Elements())
        //    {
        //        RetweeterList retweeter = new RetweeterList();

        //        retweeter.CreatedTime = GetElementValue(retweeternode, "created_at");
        //        retweeter.RetweeterID = GetElementValue(retweeternode, "id");
        //        retweeter.RetweeterContent = GetElementValue(retweeternode, "text");
        //        retweeter.SourceURL = GetElementValue(retweeternode, "source");
        //        retweeter.IsFavorited = Convert.ToBoolean(GetElementValue(friendtimelinenode.Element("retweeted_status"), "favorited"));
        //        retweeter.IsTruncated = Convert.ToBoolean(GetElementValue(friendtimelinenode.Element("retweeted_status"), "truncated"));
        //        retweeter.Geo = GetElementValue(friendtimelinenode.Element("retweeted_status"), "geo");
        //        retweeter.ReplyStatusID = (GetElementValue(friendtimelinenode.Element("retweeted_status"),
        //            "in_reply_to_status_id") == "" ? 0 : Convert.ToInt32(GetElementValue(friendtimelinenode.Element("retweeted_status"),
        //            "in_reply_to_status_id")));
        //        retweeter.ReplyUserID = (GetElementValue(friendtimelinenode.Element("retweeted_status"),
        //            "in_reply_to_user_id") == "" ? 0 : Convert.ToInt32(GetElementValue(friendtimelinenode.Element("retweeted_status"),
        //            "in_reply_to_user_id")));
        //        retweeter.ReplyToScreenName = GetElementValue(friendtimelinenode.Element("retweeted_status"), "in_reply_to_screen_name");

        //        retweeter.ThumbnailImageURL = GetElementValue(friendtimelinenode.Element("retweeted_status"), "thumbnail_pic");
        //        retweeter.MiddleImageURL = GetElementValue(friendtimelinenode.Element("retweeted_status"), "bmiddle_pic");
        //        retweeter.OriginalImageURL = GetElementValue(friendtimelinenode.Element("retweeted_status"), "original_pic");

        //        retweeter.UsersItem = null;


        //    }
        //}
        #endregion
    }
    #endregion

    #region Mention Timeline Collection
    [DataContract()]
    public class MentionList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public DateTime CreatedTime { get; set; }
        [DataMember]
        public string FriendTwitterID { get; set; }
        [DataMember]
        public string FriendTwitterContent { get; set; }
        [DataMember]
        public string SourceURL { get; set; }
        [DataMember]
        public bool IsFavorited { get; set; }
        [DataMember]
        public bool IsTruncated { get; set; }
        [DataMember]
        public string Geo { get; set; }
        [DataMember]
        public int ReplyToStatusID { get; set; }
        [DataMember]
        public int ReplyToUserID { get; set; }
        [DataMember]
        public string ReplyToScreenName { get; set; }
        [DataMember]
        public string ThumbnailPic { get; set; }
        [DataMember]
        public string MiddleSizePic { get; set; }
        [DataMember]
        public string OriginalSizePic { get; set; }
        [DataMember]
        public UserList UsersItem { get; set; }
        [DataMember]
        public RetweeterList RetweeterItem { get; set; }
        [DataMember]
        public bool IsShowThumbPic { get; set; }
        [DataMember]
        public bool IsShowRetweet { get; set; }
        #endregion

        #region Constructor
        public MentionList() { }

        public static List<MentionList> Parse(XElement element)
        {
            List<MentionList> mentioncollection = new List<MentionList>();

            foreach (XElement mentionnode in element.Elements())
            {
                MentionList mentions = new MentionList();

                mentions.CreatedTime = ParseDateTime(GetElementValue(mentionnode, "created_at"));
                mentions.FriendTwitterID = GetElementValue(mentionnode, "id");
                mentions.FriendTwitterContent = GetElementValue(mentionnode, "text");
                mentions.SourceURL = GetElementValue(mentionnode, "source");
                mentions.IsFavorited = Convert.ToBoolean(GetElementValue(mentionnode, "favorited"));
                mentions.IsTruncated = Convert.ToBoolean(GetElementValue(mentionnode, "truncated"));
                mentions.Geo = GetElementValue(mentionnode, "geo");

                mentions.ReplyToStatusID = (GetElementValue(mentionnode, "in_reply_to_status_id") == "" ? 0 : Convert.ToInt32(GetElementValue(mentionnode, "in_reply_to_status_id")));
                mentions.ReplyToUserID = (GetElementValue(mentionnode, "in_reply_to_user_id") == "" ? 0 : Convert.ToInt32(GetElementValue(mentionnode, "in_reply_to_user_id")));
                mentions.ReplyToScreenName = GetElementValue(mentionnode, "in_reply_to_screen_name");

                if (mentionnode.XPathSelectElement("thumbnail_pic") != null)
                {
                    mentions.IsShowThumbPic = true;
                    mentions.ThumbnailPic = GetElementValue(mentionnode, "thumbnail_pic");
                }
                else
                    mentions.IsShowThumbPic = false;


                if (mentionnode.XPathSelectElement("bmiddle_pic") != null)
                    mentions.MiddleSizePic = GetElementValue(mentionnode, "bmiddle_pic");

                if (mentionnode.XPathSelectElement("original_pic") != null)
                    mentions.OriginalSizePic = GetElementValue(mentionnode, "original_pic");

                UserList useritem = new UserList();
                useritem.UserID = GetElementValue(mentionnode.Element("user"), "id");
                useritem.TwitterName = GetElementValue(mentionnode.Element("user"), "screen_name");
                useritem.FriendName = GetElementValue(mentionnode.Element("user"), "name");
                useritem.Province = GetElementValue(mentionnode.Element("user"), "province");
                useritem.City = GetElementValue(mentionnode.Element("user"), "city");
                useritem.Location = GetElementValue(mentionnode.Element("user"), "location");
                useritem.Description = GetElementValue(mentionnode.Element("user"), "description");
                useritem.BlogURL = GetElementValue(mentionnode.Element("user"), "url");
                useritem.CustomizeImageURL = GetElementValue(mentionnode.Element("user"), "profile_image_url");
                useritem.Domain = GetElementValue(mentionnode.Element("user"), "domain");
                useritem.Gender = GetElementValue(mentionnode.Element("user"), "gender");
                useritem.FollowerCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "followers_count"));
                useritem.FriendCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "friends_count"));
                useritem.StatusesCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "statuses_count"));
                useritem.FavouriteCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "favourites_count"));
                useritem.CreateTime = GetElementValue(mentionnode.Element("user"), "created_at");
                useritem.IsFollowing = Convert.ToBoolean(GetElementValue(mentionnode.Element("user"), "following"));
                useritem.IsVerified = Convert.ToBoolean(GetElementValue(mentionnode.Element("user"), "verified"));
                useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(mentionnode.Element("user"), "geo_enabled"));
                mentions.UsersItem = useritem;

                RetweeterList retweeteritem = new RetweeterList();

                foreach (XElement subelement in mentionnode.Descendants("retweeted_status"))
                {
                    retweeteritem.CreatedTime = subelement.Element(XName.Get("created_at")).Value;
                    retweeteritem.RetweeterID = subelement.Element(XName.Get("id")).Value;
                    retweeteritem.RetweeterContent = subelement.Element(XName.Get("text")).Value;
                    retweeteritem.SourceURL = subelement.Element(XName.Get("source")).Value;
                    retweeteritem.IsFavorited = Convert.ToBoolean(subelement.Element(XName.Get("favorited")).Value);
                    retweeteritem.IsTruncated = Convert.ToBoolean(subelement.Element(XName.Get("truncated")).Value);
                    retweeteritem.Geo = subelement.Element(XName.Get("geo")).Value;
                    retweeteritem.ReplyStatusID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_status_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_status_id")).Value);
                    retweeteritem.ReplyUserID = Convert.ToInt32((subelement.Element(XName.Get("in_reply_to_user_id")).Value == "") ? "0" : subelement.Element(XName.Get("in_reply_to_user_id")).Value);
                    retweeteritem.ReplyToScreenName = subelement.Element(XName.Get("in_reply_to_screen_name")).Value;
                    if (subelement.XPathSelectElement("thumbnail_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.ThumbnailImageURL = subelement.Element(XName.Get("thumbnail_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    if (subelement.XPathSelectElement("bmiddle_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.MiddleImageURL = subelement.Element(XName.Get("bmiddle_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    if (subelement.XPathSelectElement("original_pic") != null)
                    {
                        retweeteritem.IsShowThumbPic = true;
                        retweeteritem.OriginalImageURL = subelement.Element(XName.Get("original_pic")).Value;
                    }
                    else
                        retweeteritem.IsShowThumbPic = false;

                    UserList subuseritem = new UserList();
                    subuseritem.UserID = GetElementValue(subelement.Element("user"), "id");
                    subuseritem.TwitterName = GetElementValue(subelement.Element("user"), "screen_name");
                    subuseritem.FriendName = GetElementValue(subelement.Element("user"), "name");
                    subuseritem.Province = GetElementValue(subelement.Element("user"), "province");
                    subuseritem.City = GetElementValue(subelement.Element("user"), "city");
                    subuseritem.Location = GetElementValue(subelement.Element("user"), "location");
                    subuseritem.Description = GetElementValue(subelement.Element("user"), "description");
                    subuseritem.BlogURL = GetElementValue(subelement.Element("user"), "url");
                    subuseritem.CustomizeImageURL = GetElementValue(subelement.Element("user"), "profile_image_url");
                    subuseritem.Domain = GetElementValue(subelement.Element("user"), "domain");
                    subuseritem.Gender = GetElementValue(subelement.Element("user"), "gender");
                    subuseritem.FollowerCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "followers_count"));
                    subuseritem.FriendCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "friends_count"));
                    subuseritem.StatusesCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "statuses_count"));
                    subuseritem.FavouriteCount = Convert.ToInt32(GetElementValue(subelement.Element("user"), "favourites_count"));
                    subuseritem.CreateTime = GetElementValue(subelement.Element("user"), "created_at");
                    subuseritem.IsFollowing = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "following"));
                    subuseritem.IsVerified = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "verified"));
                    subuseritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(subelement.Element("user"), "geo_enabled"));
                    retweeteritem.UsersItem = subuseritem;

                    mentions.RetweeterItem = retweeteritem;
                }



                if (mentions.RetweeterItem == null)
                    mentions.IsShowRetweet = false;
                else
                {
                    mentions.IsShowRetweet = true;
                }

                mentioncollection.Add(mentions);
            }

            return mentioncollection;
        }

        static DateTime ParseDateTime(string date)
        {
            string dayOfWeek = date.Substring(0, 3).Trim();
            string month = date.Substring(4, 3).Trim();
            string dayInMonth = date.Substring(8, 2).Trim();
            string time = date.Substring(11, 9).Trim();
            string offset = date.Substring(20, 5).Trim();
            string year = date.Substring(25, 5).Trim();
            string dateTime = string.Format("{0}-{1}-{2}   {3}", dayInMonth, month, year, time);
            DateTime ret = DateTime.Parse(dateTime);
            return ret;
        }
        #endregion
    }
    #endregion

    #region Direct Message Collection
    [DataContract()]
    public class DirectMessageList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public string DirectMessageID { get; set; }
        [DataMember]
        public string DirectMessageContent { get; set; }
        [DataMember]
        public string SenderID { get; set; }
        [DataMember]
        public string RecipientID { get; set; }
        [DataMember]
        public DateTime CreatedTime { get; set; }
        [DataMember]
        public string SenderScreenName { get; set; }
        [DataMember]
        public string RecipientScreenName { get; set; }
        [DataMember]
        public UserList SenderUsersItem { get; set; }
        [DataMember]
        public UserList RecipientUsersItem { get; set; }

        #endregion

        #region Constructor
        public DirectMessageList() {}

        public static List<DirectMessageList> Parse(XElement element)
        {
            List<DirectMessageList> directmessagecollection = new List<DirectMessageList>();

            foreach (XElement directmessagenode in element.Elements())
            {
                DirectMessageList directmessages = new DirectMessageList();

                directmessages.DirectMessageID = GetElementValue(directmessagenode, "id");
                directmessages.DirectMessageContent = GetElementValue(directmessagenode, "text");
                directmessages.SenderID = GetElementValue(directmessagenode, "sender_id");
                directmessages.RecipientID = GetElementValue(directmessagenode, "recipient_id");
                directmessages.CreatedTime = ParseDateTime(GetElementValue(directmessagenode, "created_at"));
                directmessages.SenderScreenName = GetElementValue(directmessagenode, "sender_screen_name");
                directmessages.RecipientScreenName = GetElementValue(directmessagenode, "recipient_screen_name");
                

                //UserList useritem = new UserList();
                //useritem.UserID = GetElementValue(mentionnode.Element("user"), "id");
                //useritem.TwitterName = GetElementValue(mentionnode.Element("user"), "screen_name");
                //useritem.FriendName = GetElementValue(mentionnode.Element("user"), "name");
                //useritem.Province = GetElementValue(mentionnode.Element("user"), "province");
                //useritem.City = GetElementValue(mentionnode.Element("user"), "city");
                //useritem.Location = GetElementValue(mentionnode.Element("user"), "location");
                //useritem.Description = GetElementValue(mentionnode.Element("user"), "description");
                //useritem.BlogURL = GetElementValue(mentionnode.Element("user"), "url");
                //useritem.CustomizeImageURL = GetElementValue(mentionnode.Element("user"), "profile_image_url");
                //useritem.Domain = GetElementValue(mentionnode.Element("user"), "domain");
                //useritem.Gender = GetElementValue(mentionnode.Element("user"), "gender");
                //useritem.FollowerCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "followers_count"));
                //useritem.FriendCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "friends_count"));
                //useritem.StatusesCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "statuses_count"));
                //useritem.FavouriteCount = Convert.ToInt32(GetElementValue(mentionnode.Element("user"), "favourites_count"));
                //useritem.CreateTime = GetElementValue(mentionnode.Element("user"), "created_at");
                //useritem.IsFollowing = Convert.ToBoolean(GetElementValue(mentionnode.Element("user"), "following"));
                //useritem.IsVerified = Convert.ToBoolean(GetElementValue(mentionnode.Element("user"), "verified"));
                //useritem.IsEnableGeo = Convert.ToBoolean(GetElementValue(mentionnode.Element("user"), "geo_enabled"));
                //mentions.UsersItem = useritem;


                directmessagecollection.Add(directmessages);
            }

            return directmessagecollection;
        }

        static DateTime ParseDateTime(string date)
        {
            string dayOfWeek = date.Substring(0, 3).Trim();
            string month = date.Substring(4, 3).Trim();
            string dayInMonth = date.Substring(8, 2).Trim();
            string time = date.Substring(11, 9).Trim();
            string offset = date.Substring(20, 5).Trim();
            string year = date.Substring(25, 5).Trim();
            string dateTime = string.Format("{0}-{1}-{2}   {3}", dayInMonth, month, year, time);
            DateTime ret = DateTime.Parse(dateTime);
            return ret;
        }
        #endregion

    }

    #endregion

    #region Unread Message Collection
    [DataContract()]
    public class UnreadMessageList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public bool IsNewStatus { get; set; }
        [DataMember]
        public string CommentCount { get; set; }
        [DataMember]
        public string MentionCount { get; set; }
        [DataMember]
        public string DirectMessageCount { get; set; }
        [DataMember]
        public string FollowerCount { get; set; }
        #endregion

        #region Constructor
        public UnreadMessageList() { }

        public static UnreadMessageList Parse(XElement element)
        {
            //List<UnreadMessageList> unreadmessagecollection = new List<UnreadMessageList>();
            //foreach (XElement unreadmessagenode in element.Elements())
            //{
            //    UnreadMessageList unreadmessages = new UnreadMessageList();
                
            //    unreadmessages.IsNewStatus = ((GetElementValue(unreadmessagenode, "new_status") == "1") ? true:false);
            //    unreadmessages.CommentCount = GetElementValue(unreadmessagenode, "comments");
            //    unreadmessages.FollowerCount = GetElementValue(unreadmessagenode, "followers");
            //    unreadmessages.DirectMessageCount = GetElementValue(unreadmessagenode, "dm");
            //    unreadmessages.MentionCount = GetElementValue(unreadmessagenode, "mentions");
            //    unreadmessagecollection.Add(unreadmessages);
            //}

            //return unreadmessagecollection;
            UnreadMessageList unreadmessages = new UnreadMessageList();
            unreadmessages.IsNewStatus = ((GetElementValue(element, "new_status") == "1") ? true : false);
            unreadmessages.CommentCount = GetElementValue(element, "comments");
            unreadmessages.FollowerCount = GetElementValue(element, "followers");
            unreadmessages.DirectMessageCount = GetElementValue(element, "dm");
            unreadmessages.MentionCount = GetElementValue(element, "mentions");

            return unreadmessages;
        }
        #endregion
    }
    #endregion

    #region Emotion Collection
    [DataContract()]
    public class EmotionList : DataMemberObjectBase
    {
        #region Public Data Members
        [DataMember]
        public string Phrase { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string EmotionURL { get; set; }
        [DataMember]
        public string IsHot { get; set; }
        [DataMember]
        public string IsCommon { get; set; }
        [DataMember]
        public string OrderNumber { get; set; }
        [DataMember]
        public string Category { get; set; }
        #endregion

        #region Constructor
        public EmotionList() { }

        public static List<EmotionList> Parse(XElement element)
        {
            List<EmotionList> emotioncollection = new List<EmotionList>();

            foreach (XElement emotionnode in element.Elements())
            {
                EmotionList emotions = new EmotionList();
                emotions.Phrase = GetElementValue(emotionnode, "phrase");
                emotions.Type = GetElementValue(emotionnode, "type");
                emotions.EmotionURL = GetElementValue(emotionnode, "url");
                emotions.IsHot = GetElementValue(emotionnode, "is_hot");
                emotions.IsCommon = GetElementValue(emotionnode, "is_common");
                emotions.OrderNumber = GetElementValue(emotionnode, "order_number");
                emotions.Category = GetElementValue(emotionnode, "category");

                emotioncollection.Add(emotions);
            }

            return emotioncollection;
        }
        #endregion
    }
    #endregion

}