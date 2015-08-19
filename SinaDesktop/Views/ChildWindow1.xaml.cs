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
using System.ServiceModel.Channels;
using System.ServiceModel;
using SinaDesktop.SinaDataService;
using SinaDesktop.Utilities;

namespace SinaDesktop
{
    public partial class ChildWindow1 : ChildWindow
    {
        public string StatusID { get; set; }

        private void UpdateCommentByID(string statusID, string commentcontent)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.UpdateCommentByIDCompleted += client_UpdateCommentByIDCompleted;
            svc.UpdateCommentByIDAsync(statusID, "xml",commentcontent, ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_UpdateCommentByIDCompleted(object sender, UpdateCommentByIDCompletedEventArgs e)
        {
            this.DialogResult = true;
        }

        #region Constructor
        public ChildWindow1()
        {
            InitializeComponent();
        }

        public ChildWindow1(string statusID)
        {
            InitializeComponent();
            this.StatusID = statusID;
            
        }
        #endregion


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCommentByID(StatusID,txtCommentContent.Text.Trim());
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

