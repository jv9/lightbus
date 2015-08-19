using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SinaDesktop.SinaDataService;
using SinaDesktop.Utilities;
using System.Windows.Threading;
using SinaDesktop.Controls;

namespace SinaDesktop.Views
{
    public partial class UserComment : Page
    {
        #region Private Member
        private bool IsFirstLoad = true;
        private ReplyCommentChildWindow cReplyCommentChildWindow = null;
        #endregion

        #region Data Service
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

        private void GetCommentTimeline()
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetCommentTimelineCompleted += client_GetCommentTimelineCompleted;
            svc.GetCommentTimelineAsync("xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetCommentTimelineCompleted(object sender, GetCommentTimelineCompletedEventArgs e)
        {
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
                Globals.CommentTimeline = e.Result;
                LayoutRoot.DataContext = Globals.CommentTimeline;
            }
            else
            {
                int count = 0;

                if (Globals.CommentTimeline.Count > 0)
                {
                    string FirstID = Globals.CommentTimeline[0].CommentID;
                    foreach (CommentTimelineList newitem in e.Result)
                    {
                        if (FirstID != newitem.CommentContent)
                        {
                            count++;
                        }
                        else
                            break;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        Globals.CommentTimeline.Insert(i, e.Result[i]);
                    }

                    LayoutRoot.DataContext = Globals.CommentTimeline;
                }
            }

            if (Globals.CommentCount != "0")
            {
                ResetCount("1");
            }
            
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
            Globals.CommentCount = "0";
        }
        #endregion

        #region Constructor
        public UserComment()
        {
            InitializeComponent();

            if (Globals.CommentTimeline != null)
            {
                if (Globals.CommentTimeline.Count > 0 && Globals.CommentTimeline.Count <= 40)
                {
                    IsFirstLoad = false;
                }
                else
                    IsFirstLoad = true;
            }

            GetCommentTimeline();

            //DispatcherTimer everyFiveSeconds = new DispatcherTimer();
            //everyFiveSeconds.Interval = TimeSpan.FromSeconds(ConfigurationSettings.UpdateTimer * 1.5);
            //everyFiveSeconds.Tick += new EventHandler(everyFiveSeconds_Tick);
            //everyFiveSeconds.Start();
        }

        //public UserComment(int index)
        //{
        //    InitializeComponent();

        //    GetCommentTimeline();
        //}
        #endregion

        #region Private Event
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        

        private void btCreateFriendship_Click(object sender, RoutedEventArgs e)
        {
            Button tmpButton = (Button)sender;
            CommentTimelineList CommentTimelineItem = (CommentTimelineList)tmpButton.DataContext;
            if (CommentTimelineItem != null)
            {
                CreateFriendship(CommentTimelineItem.UsersItem.UserID);
            }
        }

        private void everyFiveSeconds_Tick(object sender, EventArgs e)
        {
            if (Globals.CommentCount != "0")
            {
                GetCommentTimeline();
            }
        }

        private void btReplyComment_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpHyperButton = (HyperlinkButton)sender;
            CommentTimelineList CommentTimelineItem = (CommentTimelineList)tmpHyperButton.DataContext;
            
            if (CommentTimelineItem != null)
            {
                cReplyCommentChildWindow = new ReplyCommentChildWindow(CommentTimelineItem.CommentID, CommentTimelineItem.StatusItem.TwitterID);
                cReplyCommentChildWindow.Show();
            }
        }
        #endregion
    }
}
