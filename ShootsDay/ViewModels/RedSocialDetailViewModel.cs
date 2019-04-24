using DLToolkit.Forms.Controls;
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
        public FlowListView FlowListView { get; set; }        

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




        public RedSocialDetailViewModel(Page context, Picture Picture_, Label lblNoItems_, FlowListView FlowListView_) : base(context)
        {
            comments = new ObservableCollection<Comment>();

            Picture = Picture_;
            lblNoItems = lblNoItems_;
            FlowListView = FlowListView_;

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
                                FlowListView.IsVisible = false;
                                lblNoItems.IsVisible = true;
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                FlowListView.IsVisible = true;
                                lblNoItems.IsVisible = false;
                            });                            
                        }

                        if (comments_.Count() == 0 || comments_.Count() < Constants.LIMIT)
                        {
                            endOfRecords = true;
                        }

                        foreach (var Photo in comments_)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                comments.Add(Photo);
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
