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
using SinaDesktop.SinaDataService;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SinaDesktop.Utilities;

namespace SinaDesktop.Controls
{
    public partial class UserCommentChildWindow : ChildWindow
    {
        #region Public Property
        public string StatusID { get; set; }
        #endregion

        #region Data Service
        private void GetCommentsByID(string id)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetCommentsTimelineByIDCompleted += client_GetCommentsByIDCompleted;
            svc.GetCommentsTimelineByIDAsync(id,"xml",ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetCommentsByIDCompleted(object sender, GetCommentsTimelineByIDCompletedEventArgs e)
        {
            CommentListPanel.DataContext = e.Result;
        }

        private void UpdateCommentByID(string statusID, string commentcontent)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.UpdateCommentByIDCompleted += client_UpdateCommentByIDCompleted;
            svc.UpdateCommentByIDAsync(statusID, "xml", commentcontent, ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_UpdateCommentByIDCompleted(object sender, UpdateCommentByIDCompletedEventArgs e)
        {
            this.DialogResult = true;
        }
        #endregion

        #region Constructor
        public UserCommentChildWindow()
        {
            InitializeComponent();
        }

        public UserCommentChildWindow(string id)
        {
            InitializeComponent();
            this.StatusID = id;
            GetCommentsByID(id);
        }
        #endregion

        #region Private Events
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCommentByID(StatusID, txtCommentContent.Text.Trim());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
    }
}

