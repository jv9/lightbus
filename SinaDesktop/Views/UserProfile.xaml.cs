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
using SinaDesktop.Utilities;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SinaDesktop.SinaDataService;
using System.Collections.ObjectModel;

namespace SinaDesktop.Views
{
    public partial class UserProfile : Page
    {
        private string UserID = "";
        ObservableCollection<FriendTimelineList> friendtimelinecollection = new ObservableCollection<FriendTimelineList>();

        #region Data Service
        private void GetUserTimeline(string userid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetUserTimelineCompleted += client_GetUserTimelineCompleted;
            svc.GetUserTimelineAsync(userid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetUserTimelineCompleted(object sender, GetUserTimelineCompletedEventArgs e)
        {
            gridUserTimeline.DataContext = e.Result;
            friendtimelinecollection = e.Result;
        }

        private void GetUserProfile(string userid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetUserProfileCompleted += client_GetUserProfileCompleted;
            svc.GetUserProfileAsync(userid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetUserProfileCompleted(object sender, GetUserProfileCompletedEventArgs e)
        {
            LayoutRoot.DataContext = e.Result;
            GetUserTimeline(this.UserID);
        }

        private void StatusDeleteByID(string statusid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.Status_DeleteCompleted += client_StatusDeleteByIDCompleted;
            svc.Status_DeleteAsync(statusid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_StatusDeleteByIDCompleted(object sender, Status_DeleteCompletedEventArgs e)
        {
        }
        #endregion

        #region Constructors
        public UserProfile()
        {
            InitializeComponent();
            //LayoutRoot.DataContext = Globals.UserInformation;

            
        }

        public UserProfile(string userid)
        {
            InitializeComponent();

        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("userid"))
            {
                if (NavigationContext.QueryString["userid"] == "")
                {
                    this.UserID = "";
                }
                else
                {
                    this.UserID = NavigationContext.QueryString["userid"];
                    GetUserProfile(this.UserID);

                }


            }
            else
            {

            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            FriendTimelineList FriendTimelineItem = (FriendTimelineList)tmpButton.DataContext;

            if (FriendTimelineItem != null)
            {
                StatusDeleteByID(FriendTimelineItem.FriendTwitterID);
                this.friendtimelinecollection.Remove(FriendTimelineItem);
            }
            
        }

    }
}