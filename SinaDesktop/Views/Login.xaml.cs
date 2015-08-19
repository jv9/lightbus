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
using System.IO.IsolatedStorage;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SinaDesktop.SinaDataService;
using SinaDesktop.Utilities;
using SinaDesktop.Controls;
using System.Windows.Data;

namespace SinaDesktop.Views
{
    public partial class Login : Page
    {
        private MessageChildWindow mMessageChildWindow = null;
        
        public Login()
        {
            InitializeComponent();

            chkAutoLogin.IsChecked = ConfigurationSettings.AutoLogin;
            chkKeepPWD.IsChecked = ConfigurationSettings.KeepPassword;
            btLogin.IsEnabled = true;

            if ((bool)chkAutoLogin.IsChecked)
            {
                chkKeepPWD.IsChecked = true;

                txtUserName.Text = ConfigurationSettings.UserName;
                txtPassword.Password = ConfigurationSettings.Password;
   
                VerifyCredentialsLogin("xml");
            }


            if ((bool)chkKeepPWD.IsChecked)
            {

                txtUserName.Text = ConfigurationSettings.UserName;
                txtPassword.Password = ConfigurationSettings.Password;

            }
        }

        #region Data Service
        private void GetToken(string userID, string passwd)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetTokenCompleted += client_GetTokenCompleted;
            svc.GetTokenAsync(userID, passwd);
        }

        private void client_GetTokenCompleted(object sender, GetTokenCompletedEventArgs e)
        {

            try
            {
                if (e.Result != null)
                {
                    foreach (TokenKeyCollection resultKey in e.Result)
                    {
                        ConfigurationSettings.TokenKey = resultKey.TokenKeyString;
                        ConfigurationSettings.TokenKeySecret = resultKey.TokenKeySecretString;
                        ConfigurationSettings.UserName = txtUserName.Text;
                        ConfigurationSettings.Password = txtPassword.Password;
                    }
                    VerifyCredentialsLogin("xml");
                }
                else
                {
                    mMessageChildWindow = new MessageChildWindow((int)Globals.Message.VerifyError);
                    mMessageChildWindow.Show();
                }
            }
            catch (Exception)
            {
                busyIndicator.IsBusy = false;
                btLogin.IsEnabled = true;
            }

  
        }

        private void VerifyCredentialsLogin(string format)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.VerifyCredentialsCompleted += client_VerifyCredentialsLoginCompleted;
            svc.VerifyCredentialsAsync(format, ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_VerifyCredentialsLoginCompleted(object sender, VerifyCredentialsCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    ConfigurationSettings.UserName = txtUserName.Text;
                    ConfigurationSettings.Password = txtPassword.Password;

                    Globals.UserInformation = e.Result;
                    NavigationService.Navigate(new Uri(String.Format("/Home"), UriKind.Relative));
                }
                else
                {
                    mMessageChildWindow = new MessageChildWindow((int)Globals.Message.VerifyError);
                    mMessageChildWindow.Show();
                }
            }
            catch (Exception)
            {
                busyIndicator.IsBusy = false;
                btLogin.IsEnabled = true;
            }
            
        }
        #endregion

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == "" || txtPassword.Password == "")
            {
                //BindingExpression beUserName = txtUserName.GetBindingExpression(TextBox.TextProperty);
                //beUserName.UpdateSource();

                mMessageChildWindow = new MessageChildWindow((int)Globals.Message.LoginError);
                mMessageChildWindow.Show();
            }
            else
            {
                btLogin.IsEnabled = false;

                Globals.UserName = txtUserName.Text;
                Globals.Password = txtPassword.Password;

                ConfigurationSettings.KeepPassword = (bool)chkKeepPWD.IsChecked;
                ConfigurationSettings.AutoLogin = (bool)chkAutoLogin.IsChecked;

                busyIndicator.IsBusy = true;

                if (ConfigurationSettings.UserName == "")
                {
                    GetToken(txtUserName.Text, txtPassword.Password);
                }
                else
                {
                    if (ConfigurationSettings.UserName == txtUserName.Text && ConfigurationSettings.Password == txtPassword.Password)
                    {
                        VerifyCredentialsLogin("xml");
                    }
                    else
                    {
                        GetToken(txtUserName.Text, txtPassword.Password);
                    }
                }
            }
        }
    }
}
