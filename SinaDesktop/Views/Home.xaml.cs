using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SinaDesktop.SinaDataService;
using SinaDesktop.Utilities;
using System.IO;
using System.Windows.Media.Imaging;
using SilverlightChina.Controls;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Jillzhang.GifUtility;
using Silverlight.Samples;
using SinaDesktop.Controls;

namespace SinaDesktop
{
    public partial class Home : Page
    {
        #region Private Members
        //private bool IsFirstLoad = true;
        private byte[] tmpImageStream;
        private string tmpImageName;
        private ObservableCollection<DragDockPanel> panels = new ObservableCollection<DragDockPanel>();

        //private NotificationWindow toastWindow = null;
        //Queue<NotificationWindow> _notifyQueue;

        //private int newMessageCount = 0;
        #endregion

        #region Constructor
        public Home()
        {
            InitializeComponent();

            //_notifyQueue = new Queue<NotificationWindow>();
            

            foreach (DragDockPanel panel in this.dragDockPanelHost.Items)
            {
                panels.Add(panel);
            }

            InitPanel();
            LayoutRoot.DataContext = Globals.UserInformation;
            
            DispatcherTimer everyFiveSeconds = new DispatcherTimer();
            everyFiveSeconds.Interval = TimeSpan.FromSeconds(ConfigurationSettings.UpdateTimer * 1.2);
            everyFiveSeconds.Tick += new EventHandler(everyFiveSeconds_Tick);
            everyFiveSeconds.Start();
        }
        #endregion
        
        #region Data Services
        private void UpdateStatus(string status, string tokenKey, string tokenSecretKey)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);

            svc.UpdateStatusCompleted += client_UpdateStatusCompleted;
            svc.UpdateStatusAsync("xml", status, tokenKey, tokenSecretKey);
        }

        private void client_UpdateStatusCompleted(object sender, UpdateStatusCompletedEventArgs e)
        {
            //txtResult.Text = e.Result;

            txtStatus.Text = "";
            //NestFrame.Navigate(new Uri(String.Format("/FriendTimeline/1"), UriKind.Relative));
            //showMessagePanel.Begin();
        }

        #region Upload Image

        private void UploadHttpWebRequestByoAuth(string username, string passwd, string status, string tokenKey, string tokenSecretKey)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.UploadHttpWebRequestByoAuthCompleted += client_UploadHttpWebRequestByoAuthCompleted;
            svc.UploadHttpWebRequestByoAuthAsync(username, passwd, status, tmpImageName, tmpImageStream, tokenKey, tokenSecretKey);
        }

        private void client_UploadHttpWebRequestByoAuthCompleted(object sender, UploadHttpWebRequestByoAuthCompletedEventArgs e)
        {
            //txtResult.Text = e.Result;
            txtStatus.Text = "";
            //NestFrame.Navigate(new Uri(String.Format("/FriendTimeline/1"), UriKind.Relative));
            //showMessagePanel.Begin();
        }

        #endregion

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
                commentPanel.MessageCount = e.Result.CommentCount;
            }
            else
                commentPanel.MessageCount = "";

            if (e.Result.MentionCount != "0")
            {
                mentionPanel.MessageCount = e.Result.MentionCount;
            }
            else
                mentionPanel.MessageCount = "";

            if (e.Result.FollowerCount != "0")
            {
                followerPanel.MessageCount = e.Result.FollowerCount;
            }
            else
                followerPanel.MessageCount = "";

            if (e.Result.IsNewStatus)
            {
                //if (newMessageCount.ToString() == "0")
                //{
                //    homePanel.MessageCount = "+";
                //}
                //else
                //    homePanel.MessageCount = newMessageCount.ToString();
                
                //GetFriendTimeline();
            }
            else
                homePanel.MessageCount = "";
        }

        private void GetEmotions()
        {
            var httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
            httpBindingElement.MaxBufferSize = 2147483647;
            httpBindingElement.MaxReceivedMessageSize = 2147483647;
            

            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), httpBindingElement);
            
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
           
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetEmotionsCompleted += client_GetEmotions;
            svc.GetEmotionsAsync("image", "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetEmotions(object sender, GetEmotionsCompletedEventArgs e)
        {

        }

        private void ResetCount(string type)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.ResetCountCompleted += client_ResetCountCompleted;
            svc.ResetCountAsync(type, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_ResetCountCompleted(object sender, ResetCountCompletedEventArgs e)
        {
            
        }

        //private void GetFriendTimeline()
        //{
        //    CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
        //    EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
        //    SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
        //    svc.GetFriendTimelineCompleted += client_GetFriendTimeLineCompleted;
        //    svc.GetFriendTimelineAsync("xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        //}

        //private void client_GetFriendTimeLineCompleted(object sender, GetFriendTimelineCompletedEventArgs e)
        //{

            
        //        newMessageCount = 0;

        //        if (Globals.FriendTimeline.Count > 0)
        //        {
        //            string FirstID = Globals.FriendTimeline[0].FriendTwitterID;
        //            foreach (FriendTimelineList newitem in e.Result)
        //            {
        //                if (FirstID != newitem.FriendTwitterID)
        //                {
        //                    newMessageCount++;
        //                }
        //                else
        //                    break;
        //            }

        //            for (int i = 0; i < newMessageCount; i++)
        //            {
        //                Globals.FriendTimeline.Insert(i, e.Result[i]);

        //                //if (toastWindow != null && toastWindow.Visibility == Visibility.Visible)
        //                //    toastWindow.Close();

        //                NotificationWindow nw = new NotificationWindow();

        //                nw.Width = 400;
        //                nw.Height = 100;
        //                nw.Closed += new EventHandler(OnNotificationClosed);
        //                nw.Content = new NotificationControl(toastWindow,
        //                    e.Result[i].UsersItem.TwitterName,
        //                    e.Result[i].FriendTwitterContent,
        //                    e.Result[i].UsersItem.CustomizeImageURL);
        //                //toastWindow.Show((int)ConfigurationSettings.NotificationTimer);
        //                AddNotificationToQueue(nw);

        //            }
        //        }
            
        //}
        #endregion

        #region Private Events
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            imgLoad.Visibility = Visibility.Collapsed;
            if (tmpImageName != null)
            {
                if (tmpImageName.Length > 0)
                {
                    //with image
                    UploadHttpWebRequestByoAuth(Globals.UserName, Globals.Password, txtStatus.Text, ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
                }
                else
                {
                    //single status
                    UpdateStatus(txtStatus.Text, ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
                }
            }
            else
                UpdateStatus(txtStatus.Text, ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void btLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {

                using (Stream stream = openDialog.File.OpenRead())
                {


                    // Don't allow big files.
                    if (stream.Length > 1500000)
                    {
                        MessageBox.Show("Files must be less than 1.5 Megabytes.");
                    }
                    else
                    {
                        BitmapImage image = new BitmapImage();
                        byte[] data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);

                        tmpImageName = openDialog.File.Name.ToString();
                        tmpImageStream = data;
                        image.SetSource(stream);
                        //imgLoad.Source = image;
                        //imgLoad.Visibility = Visibility.Visible;
                        stream.Close();

                        //string fileExtension = System.IO.Path.GetExtension(openDialog.File.Name.ToString());
                        //BitmapImage image = new BitmapImage();

                        //byte[] data = new byte[stream.Length];
                        //stream.Read(data, 0, (int)stream.Length);

                        //tmpImageName = openDialog.File.Name.ToString();
                        //tmpImageStream = data;
                        ////image.SetSource(stream);
                        ////imgOwner.Source = image;
                        //stream.Close();

                        //GifImage tmpgif = null;

                        //if (fileExtension == ".gif")
                        //{
                        //    tmpgif = GIFDecoder.Decode(stream);
                        //    tmpImageName = openDialog.File.Name.ToString();

                        //    for (int count = 0; count < tmpgif.Frames.Count; count++)
                        //    {
                        //        imgOwner.Source = tmpgif.Frames[count].Image;
                        //        //image.SetSource(imgOwner.Source);

                        //    }

                        //    byte[] data = new byte[stream.Length];
                        //    stream.Read(data, 0, (int)stream.Length);
                        //    tmpImageStream = data;
                        //    //image.SetSource(stream);
                        //    stream.Close();
                        //}
                        //else
                        //{

                        //    byte[] data = new byte[stream.Length];
                        //    stream.Read(data, 0, (int)stream.Length);

                        //    tmpImageName = openDialog.File.Name.ToString();
                        //    tmpImageStream = data;
                        //    image.SetSource(stream);
                        //    //imgLoad.Source = image;
                        //    imgOwner.Source = image;
                        //    //imgLoad.Visibility = Visibility.Visible;
                        //    stream.Close();
                        //}
                    }
                }
            }
        }

        private void homePanel_PanelClosed(object sender, EventArgs e)
        {
            DragDockPanel currentPanel = sender as DragDockPanel;

            foreach (DragDockPanel panel in this.dragDockPanelHost.Items)
            {
                if (panel.PanelIndex == currentPanel.PanelIndex)
                {
                    this.dragDockPanelHost.Width -= 360;
                    this.dragDockPanelHost.Items.Remove(currentPanel);

                    switch (panel.PanelIndex)
                    {
                        case 0:
                            ConfigurationSettings.FriendTimelinClose = true;
                            break;
                        case 1:
                            ConfigurationSettings.UserCommentClose = true;
                            break;
                        case 2:
                            ConfigurationSettings.UserFavoriteClose = true;
                            break;
                        case 3:
                            ConfigurationSettings.UserMentionClose = true;
                            break;
                        case 4:
                            ConfigurationSettings.UserProfileClose = true;
                            break;
                        case 5:
                            ConfigurationSettings.UserFollowerClose = true;
                            break;
                    }

                    break;
                }
            }
        }

        private void btSubmit_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Opacity = 1;
        }

        private void btSubmit_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Opacity = 0.5;
        }

        private void hbWriteBlog_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.MiniMode)
            {
                txtStatus.Width = 290;
            }
            else
                txtStatus.Width = 400;

            if (BrandingBorder.Height == 0)
            {
                this.showMenu.Begin();
                txtStatus.Focus();
            }
            else
                this.hideMenu.Begin();
        }

        private void hbRefresh_Click(object sender, RoutedEventArgs e)
        {
            FriendTimelineFrame.Refresh();
        }

        private void everyFiveSeconds_Tick(object sender, EventArgs e)
        {
            if (Globals.CommentCount != "0")
            {
                btCommentCount.Visibility = Visibility.Visible;
                btCommentCount.Content = Globals.CommentCount;
                if (ConfigurationSettings.UserCommentClose == false)
                {
                    commentPanel.MessageCount = Globals.CommentCount;
                    CommentFrame.Refresh();
                }
            }
            else
            {
                btCommentCount.Visibility = Visibility.Collapsed;
                btCommentCount.Content = "";
                commentPanel.MessageCount = "";
            }


            if (Globals.MentionCount != "0")
            {
                btMentionCount.Content = Globals.MentionCount;
                btMentionCount.Visibility = Visibility.Visible;
                if (ConfigurationSettings.UserMentionClose == false)
                {
                    mentionPanel.MessageCount = Globals.MentionCount;
                    MentionFrame.Refresh();
                }
            }
            else
            {
                btMentionCount.Content = "";
                btMentionCount.Visibility = Visibility.Collapsed;
                mentionPanel.MessageCount = "";
            }

            if (Globals.FollowerCount != "0")
            {
                btFollowerCount.Visibility = Visibility.Visible;
                btFollowerCount.Content = Globals.FollowerCount;
                if (ConfigurationSettings.UserFollowerClose == false)
                {
                    followerPanel.MessageCount = Globals.FollowerCount;
                    UserFollowerFrame.Refresh();
                }
            }
            else
            {
                btFollowerCount.Visibility = Visibility.Collapsed;
                btFollowerCount.Content = "";
                followerPanel.MessageCount = "";
            }
        }

        private void AddPanel(int panelIndex)
        {
            bool isExist = false;

            foreach (DragDockPanel panel in this.dragDockPanelHost.Items)
            {
                if (panel.PanelIndex == panelIndex)
                {
                    isExist = true;
                    break;
                }
                else
                {
                    isExist = false;
                }
            }

            if (isExist == false)
            {
                foreach (DragDockPanel panel in panels)
                {
                    if (panel.PanelIndex == panelIndex)
                    {
                        this.dragDockPanelHost.Width += 360;

                        dragDockPanelHost.Items.Add(panel);
                        break;
                    }
                }
            }
        }

        private void hbFriendline_Click(object sender, RoutedEventArgs e)
        {
            MiniModeWindows();
            AddPanel((int)Globals.Panels.FriendTimelinePanel);
            ConfigurationSettings.FriendTimelinClose = false;
            
        }

        private void hbAtme_Click(object sender, RoutedEventArgs e)
        {
            MiniModeWindows();
            AddPanel((int)Globals.Panels.MentionPanel);
            ConfigurationSettings.UserMentionClose = false;
            
            if (Globals.MentionCount != "0")
            {
                if (MentionFrame.CurrentSource == null)
                {
                    MentionFrame.Navigate(new Uri(String.Format("/UserMention"), UriKind.Relative));
                }
                else
                {
                    MentionFrame.Refresh();
                }
            }  
        }

        private void hbComment_Click(object sender, RoutedEventArgs e)
        {
            MiniModeWindows();
            AddPanel((int)Globals.Panels.CommentPanel);
            ConfigurationSettings.UserCommentClose = false;

            if (Globals.CommentCount != "0")
            {
                //btCommentCount.Content = "0";
                //commentPanel.MessageCount = "0";

                if (CommentFrame.CurrentSource == null)
                {
                    CommentFrame.Navigate(new Uri(String.Format("/UserComment"), UriKind.Relative));
                }
                else
                {
                    CommentFrame.Refresh();
                }
            }
            

        }

        private void hbFavorite_Click(object sender, RoutedEventArgs e)
        {
            MiniModeWindows();

            AddPanel((int)Globals.Panels.FavoritePanel);
            ConfigurationSettings.UserFavoriteClose = false;
        }

        private void hbSearch_Click(object sender, RoutedEventArgs e)
        {
            //GetEmotions();
        }

        private void hbUserPhoto_Click(object sender, RoutedEventArgs e)
        {
            MiniModeWindows();

            HyperlinkButton tmpHyperlinkButton = (HyperlinkButton)sender;
            UserList tmpUserList = (UserList)tmpHyperlinkButton.DataContext;
            UserProfileFrame.Navigate(new Uri(String.Format("/UserProfile/" + tmpUserList.UserID.ToString()), UriKind.Relative));
            AddPanel((int)Globals.Panels.UserProfilePanel);
            ConfigurationSettings.UserProfileClose = false;
        }

        private void hbAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {

                using (Stream stream = openDialog.File.OpenRead())
                {


                    // Don't allow big files.
                    if (stream.Length > 5000000)
                    {
                        MessageBox.Show("图片尺寸必须小于5MB.");
                    }
                    else
                    {

                        ////BitmapImage image = new BitmapImage();
                        ////byte[] data = new byte[stream.Length];
                        ////stream.Read(data, 0, (int)stream.Length);

                        ////tmpImageName = openDialog.File.Name.ToString();
                        ////tmpImageStream = data;
                        ////image.SetSource(stream);
                        ////imgLoad.Source = image;
                        ////imgLoad.Visibility = Visibility.Visible;
                        ////stream.Close();

                        string fileExtension = System.IO.Path.GetExtension(openDialog.File.Name.ToString());
                        BitmapImage image = new BitmapImage();

                        byte[] data = new byte[stream.Length];
                        //stream.Read(data, 0, (int)stream.Length);

                        //tmpImageName = openDialog.File.Name.ToString();
                        //tmpImageStream = data;
                        ////image.SetSource(stream);
                        ////imgOwner.Source = image;
                        //stream.Close();

                        GifImage tmpgif = null;

                        if (fileExtension == ".gif")
                        {
                            //stream.Read(data, 0, (int)stream.Length);
                            //ExtendedImage extendedImage = new ExtendedImage();
                            //extendedImage.SetSource(stream);
                            //cusImage.Source = extendedImage;

                            tmpgif = GIFDecoder.Decode(stream);
                            tmpImageName = openDialog.File.Name.ToString();

                            for (int count = 0; count < tmpgif.Frames.Count; count++)
                            {
                                imgLoad.Source = tmpgif.Frames[count].Image;
                                imgLoad.Visibility = Visibility.Visible;
                                //imgOwner.Source = tmpgif.Frames[count].Image;
                                //image.SetSource(imgOwner.Source);

                            }

                            //byte[] data = new byte[stream.Length];
                            stream.Read(tmpgif.GlobalColorTable, 0, (int)tmpgif.GlobalColorTable.Length);
                            tmpImageStream = tmpgif.GlobalColorTable;
                            //image.SetSource(stream);
                        }
                        else
                        {
                            stream.Read(data, 0, (int)stream.Length);
                            tmpImageName = openDialog.File.Name.ToString();
                            tmpImageStream = data;
                            image.SetSource(stream);
                            imgLoad.Source = image;
                            imgLoad.Visibility = Visibility.Visible;  
                        }

                        stream.Close();
                    }
                }
            }
        }

        private void hbAddTopic_Click(object sender, RoutedEventArgs e)
        {
            txtStatus.Text = "#请在这里输入自定义话题#";
            txtStatus.SelectionStart = 1;
            txtStatus.SelectionLength = 11;
            txtStatus.Focus();
        }

        private void hbFans_Click(object sender, RoutedEventArgs e)
        {
            MiniModeWindows();
            AddPanel((int)Globals.Panels.FollowerPanel);
            ConfigurationSettings.UserFollowerClose = false;

            if (Globals.FollowerCount != "0")
            {
                if (UserFollowerFrame.CurrentSource == null)
                {
                    UserFollowerFrame.Navigate(new Uri(String.Format("/UserFollower"), UriKind.Relative));
                }
                else
                {
                    UserFollowerFrame.Refresh();
                }
            }
        }
        #endregion

        #region Private Methods
        private void InitPanel()
        {

                this.dragDockPanelHost.Width = 360 * this.dragDockPanelHost.Items.Count;

                if (ConfigurationSettings.FriendTimelinClose)
                {
                    this.dragDockPanelHost.Width -= 360;
                    this.dragDockPanelHost.Items.Remove(homePanel);
                }

                if (ConfigurationSettings.UserCommentClose)
                {
                    this.dragDockPanelHost.Width -= 360;
                    this.dragDockPanelHost.Items.Remove(commentPanel);
                }

                if (ConfigurationSettings.UserFavoriteClose)
                {
                    this.dragDockPanelHost.Width -= 360;
                    this.dragDockPanelHost.Items.Remove(favoritePanel);
                }

                if (ConfigurationSettings.UserMentionClose)
                {
                    this.dragDockPanelHost.Width -= 360;
                    this.dragDockPanelHost.Items.Remove(mentionPanel);
                }

                if (ConfigurationSettings.UserProfileClose)
                {
                    this.dragDockPanelHost.Width -= 360;
                    this.dragDockPanelHost.Items.Remove(userprofilePanel);
                }

                if (ConfigurationSettings.UserFollowerClose)
                {
                    this.dragDockPanelHost.Width -= 360;
                    this.dragDockPanelHost.Items.Remove(followerPanel);
                }
            
        }
        #endregion

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Globals.MiniMode)
            {
                ConfigurationSettings.FriendTimelinClose = false;
                ConfigurationSettings.UserCommentClose = true;
                ConfigurationSettings.UserFavoriteClose = true;
                ConfigurationSettings.UserMentionClose = true;
                ConfigurationSettings.UserProfileClose = true;
                ConfigurationSettings.UserFollowerClose = true;

                if (ConfigurationSettings.FriendTimelinClose)
                {
                    this.dragDockPanelHost.Items.Remove(homePanel);
                }

                if (ConfigurationSettings.UserCommentClose)
                {

                    this.dragDockPanelHost.Items.Remove(commentPanel);
                }

                if (ConfigurationSettings.UserFavoriteClose)
                {

                    this.dragDockPanelHost.Items.Remove(favoritePanel);
                }

                if (ConfigurationSettings.UserMentionClose)
                {

                    this.dragDockPanelHost.Items.Remove(mentionPanel);
                }

                if (ConfigurationSettings.UserProfileClose)
                {

                    this.dragDockPanelHost.Items.Remove(userprofilePanel);
                }

                if (ConfigurationSettings.UserFollowerClose)
                {

                    this.dragDockPanelHost.Items.Remove(followerPanel);
                }

                this.dragDockPanelHost.Width = 360 * this.dragDockPanelHost.Items.Count;
            }
        }

        private void MiniModeWindows()
        {
            if (Globals.MiniMode)
            {
                if (this.dragDockPanelHost.Items.Count > 0)
                {
                    this.dragDockPanelHost.Items.RemoveAt(0);
                    this.dragDockPanelHost.Width = 0;
                }
            }
        }

        //private void AddNotificationToQueue(NotificationWindow notification)
        //{
        //    if (toastWindow == null)
        //    {
        //        toastWindow = notification;
        //        notification.Show((int)ConfigurationSettings.NotificationTimer);
        //    }
        //    else
        //    {
        //        _notifyQueue.Enqueue(notification);
        //    }
        //}

        //void OnNotificationClosed(object sender, EventArgs e)
        //{
        //    toastWindow = null;
        //    if (_notifyQueue.Count > 0)
        //    {
        //        NotificationWindow notifyWindow = _notifyQueue.Dequeue();
        //        toastWindow = notifyWindow;
        //        notifyWindow.Show((int)ConfigurationSettings.NotificationTimer);
        //    }
        //}
    }
}
