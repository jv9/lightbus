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
using System.Collections.ObjectModel;

namespace SinaDesktop.Views
{
    public partial class UserFollower : Page
    {
        private void GetFollower(string statusid, int count)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetFollowerCompleted += client_GetFollowerCompleted;
            svc.GetFollowerAsync(statusid,count, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetFollowerCompleted(object sender, GetFollowerCompletedEventArgs e)
        {
            ObservableCollection<FollowerList> tmpCollection = new ObservableCollection<FollowerList>();
            tmpCollection = e.Result;

            LayoutRoot.DataContext = tmpCollection[0].Users;

            ResetCount("4");
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
        #region Constructor
        public UserFollower()
        {
            InitializeComponent();
            GetFollower(Globals.UserInformation.UserID,20);
        }
        #endregion

        #region Events
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        #endregion

        private void btCreateFriendship_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            UserList UserItem = (UserList)tmpButton.DataContext;
            if (UserItem != null)
            {
                //CreateFriendship(UserItem.UserID);
            }
            //FollowerList FriendTimelineItem = (FollowerList)tmpButton.DataContext;

            //FollowerList tmpFollowerCollection = new FollowerList();
            //tmpFollowerCollection = sender.DataContext;

        }
        
    }
}
