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

namespace SinaDesktop.Views
{
    public partial class UserDirectMessage : Page
    {
        public UserDirectMessage()
        {
            InitializeComponent();
            if (Globals.UserInformation != null)
            {
                //GetFollower(Globals.UserInformation.UserID);
            }
            //GetDirectMessage();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        //private void GetFollower(string statusid)
        //{
        //    CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
        //    EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
        //    SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
        //    svc.GetFollowerCompleted += client_GetFollowerCompleted;
        //    svc.GetFollowerAsync(statusid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        //}

        //private void client_GetFollowerCompleted(object sender, GetFollowerCompletedEventArgs e)
        //{

        //}

        private void GetDirectMessage()
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetDirectMessagesCompleted += client_GetDirectMessageCompleted;
            svc.GetDirectMessagesAsync("xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetDirectMessageCompleted(object sender, GetDirectMessagesCompletedEventArgs e)
        {
            LayoutRoot.DataContext = e.Result;
        }

    }
}
