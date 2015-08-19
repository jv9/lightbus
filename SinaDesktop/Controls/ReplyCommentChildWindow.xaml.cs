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

namespace SinaDesktop.Controls
{
    public partial class ReplyCommentChildWindow : ChildWindow
    {
        #region Public Property
        public string StatusID { get; set; }
        public string CommentID { get; set; }
        public string CommentContent { get; set; }
        #endregion

        private void ReplyCommentByID(string statusid, string commentcontent,string commentid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.Status_ReplyCommentCompleted += client_ReplyCommentByIDCompleted;
            svc.Status_ReplyCommentAsync(statusid, commentcontent, commentid, "xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_ReplyCommentByIDCompleted(object sender, Status_ReplyCommentCompletedEventArgs e)
        {
            this.DialogResult = true;
        }

        #region Constructor
        public ReplyCommentChildWindow()
        {
            InitializeComponent();
        }

        public ReplyCommentChildWindow(string commentid,string statusid)
        {
            InitializeComponent();
            this.StatusID = statusid;
            this.CommentID = commentid;
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ReplyCommentByID(this.StatusID, txtCommentContent.Text ,this.CommentID );
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

