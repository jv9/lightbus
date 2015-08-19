using System;
using System.IO.IsolatedStorage;
using SinaDesktop.SinaDataService;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace SinaDesktop.Utilities
{
    #region Global Settings
    public class Globals
    {
        public static string UserName = "";
        public static string Password = "";
        public static UserList UserInformation = null;
        public static ObservableCollection<FriendTimelineList> FriendTimeline = null;
        public static ObservableCollection<FavoriteList> FavoriteCollection = null;
        public static ObservableCollection<CommentTimelineList> CommentTimeline = null;
        public static ObservableCollection<MentionList> MentionCollection = null;
        public static string CommentCount = "0";
        public static string MentionCount = "0";
        public static string FollowerCount = "0";
        public static bool MiniMode = false;


        #region Enum
        //public Enum CommentStatus 
        //{ 
        //    NoComment, 
        //    CurrentComment, 
        //    OriginalComment, 
        //    BothComment 
        //}

        public enum Panels
        {
            FriendTimelinePanel = 0,
            CommentPanel,
            FavoritePanel,
            MentionPanel,
            UserProfilePanel,
            FollowerPanel
        }

        public enum Message
        {
            LoginError = 0,
            VerifyError = 1,
            Submit
        }

        public enum StatusType
        {
            Comment = 1,
            Mention,
            DirectMessage,
            Fans
        }
        #endregion
    }
    #endregion

    #region Configuration Setting
    public abstract class ConfigurationSettings
    {
        private const string TOKEN_KEY = "TokenKey";
        private const string TOKEN_KEY_SECRET = "TokenKeySecret";
        private const string USER_NAME = "UserName";
        private const string USER_PASSWORD = "Password";
        private const string UPDATE_TIMER = "UpdateTimer";
        private const string NOTIFICATION_TIMER = "NotificationTimer";
        private const string AUTO_UPDATE = "AutoUpdate";
        private const string KEEP_PASSWORD = "KeepPassword";
        private const string AUTO_LOGIN = "AutoLogin";
        private const string FRIENDTIMELINE_CLOSE = "FriendTimelineClose";
        private const string USERCOMMENT_CLOSE = "UserCommentClose";
        private const string USERMENTION_CLOSE = "UserMentionClose";
        private const string USERFAVORITE_CLOSE = "UserFavoriteClose";
        private const string USERFOLLOWER_CLOSE = "UserFollowerClose";
        private const string USERPROFILE_CLOSE = "UserProfileClose";
        private const string NOTIFICATION_DISABLE = "NotificationDisable";
        //private const string MINIMODE = "MiniMode";

        #region Declare Properties
        public static string TokenKey
        {
            get
            {
                string value = "";
                if (IsolatedStorageSettings.ApplicationSettings.Contains(TOKEN_KEY))
                {
                    value = IsolatedStorageSettings.ApplicationSettings[TOKEN_KEY].ToString();
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[TOKEN_KEY] = value;
            }
        }

        public static string TokenKeySecret
        {
            get
            {
                string value = "";
                if (IsolatedStorageSettings.ApplicationSettings.Contains(TOKEN_KEY_SECRET))
                {
                    value = IsolatedStorageSettings.ApplicationSettings[TOKEN_KEY_SECRET].ToString();
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[TOKEN_KEY_SECRET] = value;
            }
        }

        public static string UserName
        {
            get
            {
                string value = "";
                if (IsolatedStorageSettings.ApplicationSettings.Contains(USER_NAME))
                {
                    value = IsolatedStorageSettings.ApplicationSettings[USER_NAME].ToString();
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[USER_NAME] = value;
            }
        }

        public static string Password
        {
            get
            {
                string value = "";
                if (IsolatedStorageSettings.ApplicationSettings.Contains(USER_PASSWORD))
                {
                    value = IsolatedStorageSettings.ApplicationSettings[USER_PASSWORD].ToString();
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[USER_PASSWORD] = value;
            }
        }

        public static double UpdateTimer
        {
            get
            {
                double value = 10;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(UPDATE_TIMER))
                {
                    value = (double)IsolatedStorageSettings.ApplicationSettings[UPDATE_TIMER];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[UPDATE_TIMER] = value;
            }
        }

        public static double NotificationTimer
        {
            get
            {
                double value = 6000;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(NOTIFICATION_TIMER))
                {
                    value = (double)IsolatedStorageSettings.ApplicationSettings[NOTIFICATION_TIMER];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[NOTIFICATION_TIMER] = value;
            }
        }

        public static bool AutoUpdate
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(AUTO_UPDATE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[AUTO_UPDATE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[AUTO_UPDATE] = value;
            }
        }

        public static bool KeepPassword
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(KEEP_PASSWORD))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[KEEP_PASSWORD];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[KEEP_PASSWORD] = value;
            }
        }

        public static bool AutoLogin
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(AUTO_LOGIN))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[AUTO_LOGIN];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[AUTO_LOGIN] = value;
            }
        }

        public static bool FriendTimelinClose
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(FRIENDTIMELINE_CLOSE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[FRIENDTIMELINE_CLOSE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[FRIENDTIMELINE_CLOSE] = value;
            }
        }

        public static bool UserCommentClose
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(USERCOMMENT_CLOSE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[USERCOMMENT_CLOSE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[USERCOMMENT_CLOSE] = value;
            }
        }

        public static bool UserMentionClose
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(USERMENTION_CLOSE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[USERMENTION_CLOSE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[USERMENTION_CLOSE] = value;
            }
        }

        public static bool UserFavoriteClose
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(USERFAVORITE_CLOSE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[USERFAVORITE_CLOSE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[USERFAVORITE_CLOSE] = value;
            }
        }

        public static bool UserFollowerClose
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(USERFOLLOWER_CLOSE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[USERFOLLOWER_CLOSE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[USERFOLLOWER_CLOSE] = value;
            }
        }

        public static bool UserProfileClose
        {
            get
            {
                bool value = false;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(USERPROFILE_CLOSE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[USERPROFILE_CLOSE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[USERPROFILE_CLOSE] = value;
            }
        }

        public static bool NotificationDisable
        {
            get
            {
                bool value = true;
                if (IsolatedStorageSettings.ApplicationSettings.Contains(NOTIFICATION_DISABLE))
                {
                    value = (bool)IsolatedStorageSettings.ApplicationSettings[NOTIFICATION_DISABLE];
                }

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[NOTIFICATION_DISABLE] = value;
            }
        }

        //public static bool MiniMode
        //{
        //    get
        //    {
        //        bool value = false;
        //        if (IsolatedStorageSettings.ApplicationSettings.Contains(MINIMODE))
        //        {
        //            value = (bool)IsolatedStorageSettings.ApplicationSettings[MINIMODE];
        //        }

        //        return value;
        //    }
        //    set
        //    {
        //        IsolatedStorageSettings.ApplicationSettings[MINIMODE] = value;
        //    }
        //}
        #endregion
        
    }
    #endregion
    
}
