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
using SinaDesktop.Controls;
using System.Windows.Threading;

namespace SinaDesktop.Views
{
    public partial class UserMention : Page
    {
        #region Private Members
        private bool IsFirstLoad = true;
        private ImageViewerChildWindow imageViewerChildWindow = null;
        private UserCommentChildWindow userCommentChildWindow = null;
        private UserMentionChildWindow userMentionChildWindow = null;
        #endregion

        #region Constructor
        public UserMention()
        {
            InitializeComponent();

            if (Globals.MentionCollection != null)
            {
                if (Globals.MentionCollection.Count > 0 && Globals.MentionCollection.Count <= 40)
                {
                    IsFirstLoad = false;
                }
                else
                    IsFirstLoad = true;
            }

            GetMention();

            //DispatcherTimer everyFiveSeconds = new DispatcherTimer();
            //everyFiveSeconds.Interval = TimeSpan.FromSeconds(ConfigurationSettings.UpdateTimer * 2);
            //everyFiveSeconds.Tick += new EventHandler(everyFiveSeconds_Tick);
            //everyFiveSeconds.Start();

            //Globals.GlobalTimer.Interval = TimeSpan.FromSeconds(ConfigurationSettings.UpdateTimer * 2);
            //Globals.GlobalTimer.Tick += new EventHandler(everyFiveSeconds_Tick);
            //Globals.GlobalTimer.Start();
        }
        #endregion

        #region Private Events
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void hbImageViewer_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpLinkButton = (HyperlinkButton)sender;
            MentionList MentionItem = (MentionList)tmpLinkButton.DataContext;

            if (MentionItem != null)
            {
                imageViewerChildWindow = new ImageViewerChildWindow(MentionItem.UsersItem.FriendName, MentionItem.MiddleSizePic, MentionItem.OriginalSizePic);

                imageViewerChildWindow.Show();
            }
        }

        private void hbretweetImageViewer_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpLinkButton = (HyperlinkButton)sender;
            MentionList MentionItem = (MentionList)tmpLinkButton.DataContext;

            if (MentionItem != null)
            {
                imageViewerChildWindow = new ImageViewerChildWindow(MentionItem.UsersItem.FriendName, MentionItem.RetweeterItem.MiddleImageURL, MentionItem.RetweeterItem.OriginalImageURL);

                imageViewerChildWindow.Show();
            }
        }

        private void everyFiveSeconds_Tick(object sender, EventArgs e)
        {
            if (Globals.MentionCount != "0")
            {
                GetMention();
            }
        }

        private void btComment_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            MentionList MentionItem = (MentionList)tmpButton.DataContext;

            if (MentionItem != null)
            {
                userCommentChildWindow = new UserCommentChildWindow(MentionItem.FriendTwitterID);
                userCommentChildWindow.Show();
            }
        }

        private void btRepost_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            MentionList MentionItem = (MentionList)tmpButton.DataContext;

            if (MentionItem != null)
            {
                userMentionChildWindow = new UserMentionChildWindow(MentionItem.FriendTwitterID);
                userMentionChildWindow.Show();
            }
        }

        private void btFavorite_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            MentionList MentionItem = (MentionList)tmpButton.DataContext;

            if (MentionItem != null)
            {
                MentionItem.IsFavorited = true;
                CreateFavorite(MentionItem.FriendTwitterID);
            }
        }

        private void btUnFavorite_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            MentionList MentionItem = (MentionList)tmpButton.DataContext;
            if (MentionItem != null)
            {
                MentionItem.IsFavorited = false;

                foreach (FavoriteList item in Globals.FavoriteCollection)
                {
                    if (item.FriendTwitterID == MentionItem.FriendTwitterID)
                    {
                        Globals.FavoriteCollection.Remove(item);
                        DelFavorite(MentionItem.FriendTwitterID);
                        break;
                    }
                }
            }
        }
        #endregion
        
        #region Data Service
        private void GetMention()
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetMentionsCompleted += client_GetMentionCompleted;
            svc.GetMentionsAsync("xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetMentionCompleted(object sender, GetMentionsCompletedEventArgs e)
        {
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
                Globals.MentionCollection = e.Result;
                LayoutRoot.DataContext = Globals.MentionCollection;
            }
            else
            {
                int count = 0;

                if (Globals.MentionCollection.Count > 0)
                {
                    string FirstID = Globals.MentionCollection[0].FriendTwitterID;
                    foreach (MentionList newitem in e.Result)
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
                        Globals.MentionCollection.Insert(i, e.Result[i]);
                    }

                    LayoutRoot.DataContext = Globals.MentionCollection;
                }

                if (Globals.MentionCount != "0")
                    ResetCount("2");
                //ResetCount("2");
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
            Globals.MentionCount = "0";
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
                Globals.FavoriteCollection.Insert(0, e.Result);
            }
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
    }
}
