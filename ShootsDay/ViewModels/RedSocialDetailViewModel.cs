using DLToolkit.Forms.Controls;
using ImageCircle.Forms.Plugin.Abstractions;
using ShootsDay.Managers;
using ShootsDay.RequestModels;
using ShootsDay.ViewModels;
using ShootsDay.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Models.Share
{
    class RedSocialDetailViewModel : BaseViewModel
    {
        public ImageSource Source { get; set; }
        public Picture Picture { get; set; }
        public Label lblNoItems { get; set; }
        public StackLayout StackLayoutListview { get; set; }
        
        private ObservableCollection<Comment> comments_;
        private bool endOfRecords = false;

        public ObservableCollection<Comment> comments
        {
            get
            {
                return comments_;
            }
            set
            {
                comments_ = value;
                RaisePropertyChanged();
            }
        }

        public Command ItemAppearingCommand
        {
            get;
            set;
        }




        public RedSocialDetailViewModel(Page context, 
                                        Picture Picture_, 
                                        Label lblNoItems_, 
                                        StackLayout StackLayout_) : base(context)
        {
            comments = new ObservableCollection<Comment>();

            Picture = Picture_;
            lblNoItems = lblNoItems_;
            this.StackLayoutListview = StackLayout_;

            getComments();

            ItemAppearingCommand = new Command((args) => OnItemAppearing(args));
        }


        private async Task OnItemAppearing(object args)
        {
            Comment Comment = (Comment)args;

            var commentsCount = comments_.Count;

            if (commentsCount == 0)
            {
                return;
            }

            //The server returned empty or lesser thant the limit
            if (endOfRecords)
            {
                return;
            }

            //If this is the end of the list get more records            
            var lastCommentId = comments_[commentsCount - 1].id;
            if (Comment.id == lastCommentId)
            {
                getComments();
            }
        }


        public async void getComments()
        {
            string username = SettingsManager.Instance.getUserName();
            string password = SettingsManager.Instance.getPassword();
            int id_event = SettingsManager.Instance.getIdEvent();

            try
            {
                LoadingManager.Instance.showLoading();

                var offset = 0;
                if (comments_!= null && comments_.Count() > 0)
                {
                    offset = comments_.Count();
                }

                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new { Picture = new { picture_id = Picture.id }, Login = new { password = password, username = username }, limit = Constants.LIMIT, offset = offset });
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri(Constants.GET_COMMENTS);

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);

                LoadingManager.Instance.closeLoading();

                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestComments>(tokenJson);
                    if (jsonSystem.status.type != "error")
                    {
                        //Get the comments list
                        List<Comment> comments_ = jsonSystem.data==null? new List<Comment>() : jsonSystem.data.Comments;

                        if (jsonSystem.data == null)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                lblNoItems.IsVisible = true;
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                lblNoItems.IsVisible = false;
                            });                            
                        }

                        if (comments_.Count() == 0 || comments_.Count() < Constants.LIMIT)
                        {
                            endOfRecords = true;
                        }

                        var rowCount = 0;
                        foreach (var Comment in comments_)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                comments.Add(Comment);

                                var gridImageUser = new Grid();
                                gridImageUser.RowSpacing = 1;
                                gridImageUser.BackgroundColor = Color.FromHex("#EEEAE9");
                                gridImageUser.Padding = 10;
                                gridImageUser.HorizontalOptions = LayoutOptions.FillAndExpand;
                                gridImageUser.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                gridImageUser.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                                gridImageUser.ColumnDefinitions.Add(new ColumnDefinition { Width = 25 });
                                gridImageUser.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                                var circuleImage = new CircleImage();
                                circuleImage.Source = Comment.User.url_image;
                                circuleImage.Aspect = Aspect.AspectFit;
                                circuleImage.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                circuleImage.VerticalOptions = LayoutOptions.CenterAndExpand;
                                circuleImage.WidthRequest = 35;
                                Grid.SetRow(circuleImage, 0);
                                Grid.SetColumn(circuleImage, 0);
                                gridImageUser.Children.Add(circuleImage);

                                var comment = new Label();
                                comment.Text = Comment.comment;
                                comment.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
                                comment.TextColor = Color.Gray;
                                comment.HorizontalOptions = Xamarin.Forms.LayoutOptions.StartAndExpand;
                                comment.VerticalOptions = Xamarin.Forms.LayoutOptions.CenterAndExpand;
                                Grid.SetRow(comment, 0);
                                Grid.SetColumn(comment, 1);
                                gridImageUser.Children.Add(comment);

                                var publiDate = new Label();
                                publiDate.Text = Comment.createdString;
                                publiDate.FontSize = 7;
                                publiDate.TextColor = Color.Black;
                                publiDate.HorizontalOptions = Xamarin.Forms.LayoutOptions.StartAndExpand;
                                publiDate.VerticalOptions = Xamarin.Forms.LayoutOptions.CenterAndExpand;
                                Grid.SetRow(publiDate, 1);
                                Grid.SetColumn(publiDate, 1);
                                gridImageUser.Children.Add(publiDate);

                                StackLayoutListview.Children.Add(gridImageUser);

                                ++rowCount;
                            });
                        }
                    }
                    else
                    {
                        Alert.DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                    }
                }
                else
                {
                    var respuesta = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestUser>(respuesta);
                    Alert.DisplayAlert("Error", respuesta, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                LoadingManager.Instance.closeLoading();
                Alert.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
