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
using SinaDesktop.Utilities;
using SinaDesktop.SinaDataService;

namespace SinaDesktop.Controls
{
    public partial class UserMentionChildWindow : ChildWindow
    {
        #region Public Property
        public string StatusID { get; set; }
        #endregion

        #region Data Service
        private void RepostStatus(string statusid, string commentcontent, int isComment)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.RepostStatusCompleted += client_RepostStatusCompleted;
            svc.RepostStatusAsync(statusid, commentcontent, isComment, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_RepostStatusCompleted(object sender, RepostStatusCompletedEventArgs e)
        {
            this.DialogResult = true;
        }
        #endregion

        #region Constructor
        public UserMentionChildWindow()
        {
            InitializeComponent();
        }

        public UserMentionChildWindow(string strStatusID)
        {
            InitializeComponent();
            this.StatusID = strStatusID;
        }
        #endregion

        #region Private Events
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int tmpIsComment = (bool)chkComment.IsChecked ? 1 : 0;
            RepostStatus(this.StatusID, txtMentionContent.Text, tmpIsComment);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
    }
}

