using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SinaDesktop.SinaDataService;
using SinaDesktop.Utilities;
using SinaDesktop.Controls;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace SinaDesktop.Views
{
    public partial class FriendTimeline : Page
    {
        #region Private Members
        private bool IsFirstLoad = true;
        private ImageViewerChildWindow imageViewerChildWindow = null;
        //private ObservableCollection<FriendTimelineList> FriendTimelineCollection = new ObservableCollection<FriendTimelineList>();
        private NotificationWindow toastWindow = null;
        private UserCommentChildWindow userCommentChildWindow = null;
        private UserMentionChildWindow userMentionChildWindow = null;
        Queue<NotificationWindow> _notifyQueue;
        #endregion

        #region Constructors
        public FriendTimeline()
        {
            InitializeComponent();

            _notifyQueue = new Queue<NotificationWindow>();

            if (Globals.FriendTimeline != null)
            {
                if (Globals.FriendTimeline.Count > 0 && Globals.FriendTimeline.Count <= 80)
                {
                    IsFirstLoad = false;
                }
                else
                    IsFirstLoad = true;
            }

            GetFriendTimeline();

            DispatcherTimer everyFiveSeconds = new DispatcherTimer();
            everyFiveSeconds.Interval = TimeSpan.FromSeconds(ConfigurationSettings.UpdateTimer);
            everyFiveSeconds.Tick += new EventHandler(everyFiveSeconds_Tick);
            everyFiveSeconds.Start();
        }
        #endregion
        
        #region Data Service
        private void GetFriendTimeline()
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetFriendTimelineCompleted += client_GetFriendTimeLineCompleted;
            svc.GetFriendTimelineAsync("xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetFriendTimeLineCompleted(object sender, GetFriendTimelineCompletedEventArgs e)
        {

            if (IsFirstLoad)
            {
                IsFirstLoad = false;
                Globals.FriendTimeline = e.Result;
                LayoutRoot.DataContext = Globals.FriendTimeline;
            }
            else
            {
                int count = 0;

                if (Globals.FriendTimeline.Count > 0)
                {
                    string FirstID = Globals.FriendTimeline[0].FriendTwitterID;
                    foreach (FriendTimelineList newitem in e.Result)
                    {
                        if (FirstID != newitem.FriendTwitterID)
                        {
                            count++;
                        }
                        else
                            break;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        Globals.FriendTimeline.Insert(i, e.Result[i]);
                        
                        //if (toastWindow != null && toastWindow.Visibility == Visibility.Visible)
                        //    toastWindow.Close();

                        if (ConfigurationSettings.NotificationDisable)
                        {
                            NotificationWindow nw = new NotificationWindow();
                            nw.Width = 400;
                            nw.Height = 100;
                            nw.Closed += new EventHandler(OnNotificationClosed);
                            nw.Content = new NotificationControl(nw,
                                e.Result[i].UsersItem.TwitterName,
                                e.Result[i].FriendTwitterContent,
                                e.Result[i].UsersItem.CustomizeImageURL,
                                e.Result[i].UsersItem.IsVerified);
                            AddNotificationToQueue(nw);
                        }
                        
                        //toastWindow = new NotificationWindow();
                        //toastWindow.Width = 400;
                        //toastWindow.Height = 100;
                        //toastWindow.Closed += new EventHandler(OnNotificationClosed);
                        //toastWindow.Content = new NotificationControl(toastWindow,
                        //    e.Result[i].UsersItem.TwitterName, 
                        //    e.Result[i].FriendTwitterContent,
                        //    e.Result[i].UsersItem.CustomizeImageURL);
                        ////toastWindow.Show((int)ConfigurationSettings.NotificationTimer);
                        //AddNotificationToQueue(toastWindow);
                    }

                    LayoutRoot.DataContext = Globals.FriendTimeline;
                
                }
            }
        }

        private void CreateNewNotificationControl(string NotificationText)
        {
        }

        private void GetUnreadMessage(string sinceid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetUnreaderMessageCompleted += client_GetUnreadMessageCompleted;
            svc.GetUnreaderMessageAsync(sinceid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetUnreadMessageCompleted(object sender, GetUnreaderMessageCompletedEventArgs e)
        {
            if (e.Result.CommentCount != "0")
            {
                Globals.CommentCount = e.Result.CommentCount;
            }

            if (e.Result.MentionCount != "0")
            {
                Globals.MentionCount = e.Result.MentionCount;
            }

            if (e.Result.FollowerCount != "0")
                Globals.FollowerCount = e.Result.FollowerCount;

            if (e.Result.IsNewStatus)
            {
                if (Globals.FriendTimeline != null)
                {
                    if (Globals.FriendTimeline.Count > 0 && Globals.FriendTimeline.Count <= 80)
                    {
                        IsFirstLoad = false;
                    }
                    else
                        IsFirstLoad = true;
                }

                GetFriendTimeline();
            }
        }

        private void CreateFavorite(string statusid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.CreateFavoriteCompleted += client_CreateFavoriteCompleted;
            svc.CreateFavoriteAsync(statusid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_CreateFavoriteCompleted(object sender, CreateFavoriteCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Globals.FavoriteCollection.Insert(0,e.Result);
            }
        }

        private void CreateFriendship(string userid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.CreateFriendshipsCompleted += client_CreateFriendshipCompleted;
            svc.CreateFriendshipsAsync(userid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_CreateFriendshipCompleted(object sender, CreateFriendshipsCompletedEventArgs e)
        {
        }

        private void DelFavorite(string statusid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.DeleteFavoriteCompleted += client_DelFavoriteCompleted;
            svc.DeleteFavoriteAsync(statusid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_DelFavoriteCompleted(object sender, DeleteFavoriteCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                
            }
        }
        #endregion

        #region Private Events
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (NavigationContext.QueryString.ContainsKey("isFirstLoad"))
            //{
            //    if (NavigationContext.QueryString["isFirstLoad"] == "")
            //    {
            //        this.IsFirstLoad = "T";
            //    }
            //    else
            //        this.IsFirstLoad = NavigationContext.QueryString["isFirstLoad"];
            //}
            //else
            //{

            //}
        }

        private void hbImageViewer_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpLinkButton = (HyperlinkButton)sender;
            FriendTimelineList FriendTimelineItem = (FriendTimelineList)tmpLinkButton.DataContext;

            if (FriendTimelineItem != null)
            {
                imageViewerChildWindow = new ImageViewerChildWindow(FriendTimelineItem.UsersItem.FriendName, FriendTimelineItem.MiddleSizePic, FriendTimelineItem.OriginalSizePic);
                imageViewerChildWindow.Show();
            }
        }

        private void retweetImageViewer_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpLinkButton = (HyperlinkButton)sender;
            FriendTimelineList FriendTimelineItem = (FriendTimelineList)tmpLinkButton.DataContext;

            if (FriendTimelineItem != null)
            {
                imageViewerChildWindow = new ImageViewerChildWindow(FriendTimelineItem.UsersItem.FriendName, FriendTimelineItem.RetweeterItem.MiddleImageURL, FriendTimelineItem.RetweeterItem.OriginalImageURL);
                imageViewerChildWindow.Show();
            }

        }

        private void everyFiveSeconds_Tick(object sender, EventArgs e)
        {
            if (Globals.FriendTimeline != null)
            {
                if (Globals.FriendTimeline.Count > 0)
                {
                    GetUnreadMessage(Globals.FriendTimeline[0].FriendTwitterID);
                }
            }
        }

        private void btComment_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            FriendTimelineList FriendTimelineItem = (FriendTimelineList)tmpButton.DataContext;

            if (FriendTimelineItem != null)
            {
                userCommentChildWindow = new UserCommentChildWindow(FriendTimelineItem.FriendTwitterID);
                userCommentChildWindow.Show();
            }
        }

        private void btRepost_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            FriendTimelineList FriendTimelineItem = (FriendTimelineList)tmpButton.DataContext;

            if (FriendTimelineItem != null)
            {
                userMentionChildWindow = new UserMentionChildWindow(FriendTimelineItem.FriendTwitterID);
                userMentionChildWindow.Closed += UserMentionChildWindow_Close;
                userMentionChildWindow.Show();
                //RepostStatus(FriendTimelineItem.FriendTwitterID, "", 0);
            }
        }

        private void btFavorite_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            FriendTimelineList FriendTimelineItem = (FriendTimelineList)tmpButton.DataContext;

            if (FriendTimelineItem != null)
            {
                FriendTimelineItem.IsFavorited = true;
                CreateFavorite(FriendTimelineItem.FriendTwitterID);
            }
        }

        private void btUnFavorite_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            FriendTimelineList FriendTimelineItem = (FriendTimelineList)tmpButton.DataContext;
            if (FriendTimelineItem != null)
            {
                FriendTimelineItem.IsFavorited = false;

                foreach (FavoriteList item in Globals.FavoriteCollection)
                {
                    if (item.FriendTwitterID == FriendTimelineItem.FriendTwitterID)
                    {
                        Globals.FavoriteCollection.Remove(item);
                        DelFavorite(FriendTimelineItem.FriendTwitterID);
                        break;
                    }
                }
            }
        }

        private void AddNotificationToQueue(NotificationWindow notification)
        {
            if (toastWindow == null)
            {
                toastWindow = notification;
                notification.Show((int)ConfigurationSettings.NotificationTimer);
            }
            else
            {
                _notifyQueue.Enqueue(notification);
            }
        }

        void OnNotificationClosed(object sender, EventArgs e)
        {
            toastWindow = null;
            if (_notifyQueue.Count > 0)
            {
                NotificationWindow notifyWindow = _notifyQueue.Dequeue();
                toastWindow = notifyWindow;
                notifyWindow.Show((int)ConfigurationSettings.NotificationTimer);
            }
        }

        void UserMentionChildWindow_Close(object sender, EventArgs e)
        {
            Boolean? returnValue = userMentionChildWindow.DialogResult;

            if (returnValue.HasValue && returnValue.Value)
            {
                tbMessage.Text = "成功转发微博";
                MessageImage.Source = new BitmapImage(new Uri("/SinaDesktop;component/Images/success.png", UriKind.Relative));
                this.showMessagePanel.Begin();
            }
        }
        #endregion
    }
}
