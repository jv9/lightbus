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
using System.Collections.ObjectModel;

namespace SinaDesktop.Views
{
    public partial class UserFavorite : Page
    {
        private ImageViewerChildWindow imageViewerChildWindow = null;

        #region Constructor
        public UserFavorite()
        {
            InitializeComponent();
            GetFavorites();
        }
        #endregion

        #region Private Data Service
        private void GetFavorites()
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.GetFavoritesCompleted += client_GetFavoritesCompleted;
            svc.GetFavoritesAsync("xml", ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_GetFavoritesCompleted(object sender, GetFavoritesCompletedEventArgs e)
        {
            //dgFavorites.ItemsSource = e.Result;
            Globals.FavoriteCollection = e.Result;
            LayoutRoot.DataContext = Globals.FavoriteCollection;
        }

        private void DelFavorite(string statusid)
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new HttpTransportBindingElement());
            EndpointAddress address = new EndpointAddress(new Uri(Application.Current.Host.Source, "/SinaDesktop.Web/SinaService.svc"));
            SinaDataService.DataServiceClient svc = new SinaDataService.DataServiceClient(binding, address);
            svc.DeleteFavoriteCompleted += client_DelFavoriteCompleted;
            svc.DeleteFavoriteAsync(statusid, "xml",ConfigurationSettings.TokenKey, ConfigurationSettings.TokenKeySecret);
        }

        private void client_DelFavoriteCompleted(object sender, DeleteFavoriteCompletedEventArgs e)
        {
            //ObservableCollection<FavoriteList> tmpList = (ObservableCollection<FavoriteList>)LayoutRoot.DataContext;
            //ObservableCollection<FavoriteList> tmpResult = e.Result;

            //if (tmpResult.Count > 0)
            //{
                
                    //for (int count = 0; count < e.Result.Count; count++)
                    //{
                    //    foreach (FavoriteList item in tmpList)
                    //    {
                    //        if (item.FriendTwitterID == tmpResult[count].FriendTwitterID)
                    //        {
                    //            tmpList.Remove(tmpResult[count]);
                    //            break;
                    //        }
                    //    }
                    //}
                
            //}
        }
        #endregion

        #region Private Events
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void hbImageViewer_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpLinkButton = (HyperlinkButton)sender;
            FavoriteList FavoriteItem = (FavoriteList)tmpLinkButton.DataContext;

            if (FavoriteItem != null)
            {
                imageViewerChildWindow = new ImageViewerChildWindow(FavoriteItem.UsersItem.FriendName, FavoriteItem.MiddleSizePic, FavoriteItem.OriginalSizePic);

                imageViewerChildWindow.Show();
            }
        }

        private void retweetImageViewer_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpLinkButton = (HyperlinkButton)sender;
            FavoriteList FavoriteItem = (FavoriteList)tmpLinkButton.DataContext;

            if (FavoriteItem != null)
            {
                imageViewerChildWindow = new ImageViewerChildWindow(FavoriteItem.UsersItem.FriendName, FavoriteItem.RetweeterItem.MiddleImageURL, FavoriteItem.RetweeterItem.OriginalImageURL);

                imageViewerChildWindow.Show();
            }
        }

        private void btDelFavorite_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton tmpButton = (HyperlinkButton)sender;
            FavoriteList FavoriteItem = (FavoriteList)tmpButton.DataContext;

            if (FavoriteItem != null)
            {
                DelFavorite(FavoriteItem.FriendTwitterID);
                ObservableCollection<FavoriteList> tmpList = (ObservableCollection<FavoriteList>)LayoutRoot.DataContext;
                foreach (FavoriteList item in tmpList)
                {
                    if (item.FriendTwitterID == FavoriteItem.FriendTwitterID)
                    {
                        tmpList.Remove(FavoriteItem);
                        break;
                    }
                }
            }
        }
        #endregion
        

    }
}
