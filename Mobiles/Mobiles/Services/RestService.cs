using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;

namespace Mobiles.Services
{

    internal class NameValueCollection
    {
        internal void Add(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }


    public class RestService : IDisposable
    {

        public HttpClient ClientContext { get; set; }
        public Uri BaseAddress { get; }
        string _server = "http://192.168.1.6";

        public RestService()
        {
            // this.MaxResponseContentBufferSize = 256000;
            //var a = ConfigurationManager.AppSettings["IP"];
            this.BaseAddress = new Uri(_server);
            ClientContext = new HttpClient();
            ClientContext.BaseAddress = BaseAddress;
            ClientContext.DefaultRequestHeaders.Accept.Clear();
            this.ClientContext.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //key api = 57557c4f25f436213fe34a2090a266e2

        }

        public RestService(string controller)
        {
            string BaseUri = _server+ "/api/" + controller + "/";
            ClientContext = new HttpClient();
            ClientContext.BaseAddress = new Uri(BaseUri);
            ClientContext.DefaultRequestHeaders.Accept.Clear();
            this.ClientContext.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            CekTokenAsync();
        }
        

        public async void CekTokenAsync()
        {
            var main = await Helpers.GetBaseApp();
            if (main != null && main.Token != null)
            {
                this.ClientContext.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue(main.Token.token_type, main.Token.access_token);
            }
        }


        public void SetToken(AuthenticationToken token)
        {
            if (token != null)
            {
                this.ClientContext.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue(token.token_type, token.access_token);
            }
        }

        public async Task<AuthenticationToken> GenerateTokenAsync(string user, string password)
        {
            try
            {
                var str = string.Format("grant_type=password&username={0}&password={1}", user, password);
                var result = await ClientContext.PostAsync("Token", new StringContent(str, Encoding.UTF8));
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var Token = JsonConvert.DeserializeObject<AuthenticationToken>(content);

                    if (Token != null)
                    {
                        Token.Email = user;
                    }

                    return Token;
                }
                else
                {
                    throw new System.Exception(result.StatusCode.ToString());
                }

            }
            catch (Exception ex)
            {
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = ex.Message,
                    Cancel = "OK"
                }, "message");

                return null;
            }

        }

        internal async Task<T> GetAsync<T>(string uri, int id)
        {
            var response = await ClientContext.GetAsync(uri + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return ConvertResponseToObject<T>(response);
            }
            else
                return default(T);
        }

        internal async Task<T> GetAsync<T>(string uri)
        {
            var response = await ClientContext.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                return ConvertResponseToObject<T>(response);
            }
            else
                return default(T);
        }

        internal async Task<T> PostAsync<T>(string uri, T item)
        {
            var obj = JsonConvert.SerializeObject(item);
            var response = await ClientContext.PostAsJsonAsync<T>(uri, item);
            if (response.IsSuccessStatusCode)
            {
                return ConvertResponseToObject<T>(response);
            }
            AnotherResponse(response);
            return default(T);
        }

        private void AnotherResponse(HttpResponseMessage response)
        {
            string message = response.Content.ReadAsStringAsync().Result;
            switch (response.StatusCode)
            {

                //case HttpStatusCode.Continue:
                //    break;
                //case HttpStatusCode.SwitchingProtocols:
                //    break;
                //case HttpStatusCode.OK:
                //    break;
                //case HttpStatusCode.Created:
                //    break;
                //case HttpStatusCode.Accepted:
                //    break;
                //case HttpStatusCode.NonAuthoritativeInformation:
                //    break;
                //case HttpStatusCode.NoContent:
                //    break;
                //case HttpStatusCode.ResetContent:
                //    break;
                //case HttpStatusCode.PartialContent:
                //    break;
                //case HttpStatusCode.MultipleChoices:
                //    break;

                //case HttpStatusCode.MovedPermanently:
                //    break;

                case HttpStatusCode.Found:
                    Helpers.ShowMessageError("Data Tidak Ditemukan");
                    break;

                case HttpStatusCode.SeeOther:
                    break;

                case HttpStatusCode.NotModified:
                    Helpers.ShowMessageError("Data Tidak Tersimpan");
                    break;
                //case HttpStatusCode.UseProxy:
                //    break;
                //case HttpStatusCode.Unused:
                //    break;
                //case HttpStatusCode.TemporaryRedirect:
                //    break;
                case HttpStatusCode.BadRequest:

                    Helpers.ShowMessageError(message);

                    break;
                case HttpStatusCode.Unauthorized:
                    Helpers.ShowMessageError("Anda Tidak Memiliki Access");
                    break;
                //case HttpStatusCode.PaymentRequired:
                //    break;
                //case HttpStatusCode.Forbidden:
                //    break;
                case HttpStatusCode.NotFound:
                    Helpers.ShowMessageError("Alamat Request Tidak Ditemukan");
                    break;
                //case HttpStatusCode.MethodNotAllowed:
                //    break;
                //case HttpStatusCode.NotAcceptable:
                //    break;
                //case HttpStatusCode.ProxyAuthenticationRequired:
                //    break;
                case HttpStatusCode.RequestTimeout:
                    Helpers.ShowMessageError("Waktu Request Terlalu Lama");
                    break;
                //case HttpStatusCode.Conflict:
                //    break;
                //case HttpStatusCode.Gone:
                //    break;
                //case HttpStatusCode.LengthRequired:
                //    break;
                //case HttpStatusCode.PreconditionFailed:
                //    break;
                //case HttpStatusCode.RequestEntityTooLarge:
                //    break;
                //case HttpStatusCode.RequestUriTooLong:
                //    break;
                //case HttpStatusCode.UnsupportedMediaType:
                //    break;
                //case HttpStatusCode.RequestedRangeNotSatisfiable:
                //    break;
                //case HttpStatusCode.ExpectationFailed:
                //    break;
                //case HttpStatusCode.UpgradeRequired:
                //    break;
                //case HttpStatusCode.InternalServerError:
                //    break;
                //case HttpStatusCode.NotImplemented:
                //    break;
                //case HttpStatusCode.BadGateway:
                //    break;
                //case HttpStatusCode.ServiceUnavailable:
                //    break;
                //case HttpStatusCode.GatewayTimeout:
                //    break;
                //case HttpStatusCode.HttpVersionNotSupported:
                //    break;
                default:
                    message = response.Content.ReadAsStringAsync().Result;
                    Helpers.ShowMessageError(message);
                    break;
            }
        }

        internal async Task<T> Delete<T>(string uri, int id)
        {
            var response = await ClientContext.DeleteAsync(uri + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return ConvertResponseToObject<T>(response);
            }
            AnotherResponse(response);
            return default(T);
        }

        internal async Task<T> PutAsync<T>(string uri, object id, T content)
        {
            var response = await ClientContext.PutAsJsonAsync<T>(uri + "/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                return ConvertResponseToObject<T>(response);
            }
            else
                return default(T);
        }

        public T ConvertResponseToObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        public void Dispose()
        {
        }
    }


}
