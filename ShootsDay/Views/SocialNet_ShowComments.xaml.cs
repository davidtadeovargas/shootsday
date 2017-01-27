using ShootsDay.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ShootsDay.Views
{
    public partial class SocialNet_ShowComments : ContentPage
    {
        string host = Application.Current.Properties["host"].ToString();
        string id_image;
        List<Comment> comentarios = null;
        public SocialNet_ShowComments(RequestComments request, string _id_image)
        {
            this.id_image = _id_image;
            if (request.data != null)
            {
                comentarios = request.data.Comments;
                foreach (var comment in comentarios)
                {
                    comment.User.url_image = this.host + "" + comment.User.url_image;
                }
            }
            InitializeComponent();
            Debug.WriteLine(comentarios);
            listComments.ItemsSource = comentarios;
        }

        private void btnAdd_comment(object sender, EventArgs e)
        {
            var boton = (Button)sender;
            Debug.WriteLine("Mostrar vista para agregar comentario");
            if (!string.IsNullOrEmpty(addComment.Text))
            {
                add_comment(addComment.Text);
            }
        }

        private async void add_comment(string comentario)
        {
            string username = Application.Current.Properties["username"].ToString();
            string password = Application.Current.Properties["password"].ToString();
            try
            {
                var client = new HttpClient();
                var userData = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Comment = new { picture_id = this.id_image, comment = comentario },
                    Login = new { password = password, username = username }
                });
                //var userData = Newtonsoft.Json.JsonConvert.SerializeObject( new { User = auxUser, Login = auxLogin } );
                var content = new StringContent(userData, Encoding.UTF8, "application/json");

                var uri = new Uri("http://www.js-project.com.mx/ws-jsproject/comments/add.json");

                var result = await client.PostAsync(uri, content).ConfigureAwait(true);
                if (result.IsSuccessStatusCode)
                {
                    var tokenJson = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(tokenJson);

                    if (jsonSystem.status.type != "error")
                    {
                        // Se obtuvieron imagenes con éxito, se muestran en la vista
                        await DisplayAlert(jsonSystem.status.type, jsonSystem.status.message, "Aceptar");
                    }
                    else
                    {
                        await DisplayAlert(jsonSystem.status.type, jsonSystem.status.message, "Aceptar");
                    }
                    this.Navigation.PopAsync();
                }
                else
                {
                    var respuesta = await result.Content.ReadAsStringAsync();
                    var jsonSystem = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMyPictures>(respuesta);
                    await DisplayAlert("Error", jsonSystem.status.message, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error, Excepcion: " + ex.Message);
            }
        }

    }
}
