using Blazored.LocalStorage;
using Newtonsoft.Json;
using LoteriaNacionalDominio;
using static LoteriaNacionalDominio.SeguridadDTO;


namespace IntranetLoteriaNacional.Shared.Constants
{
    public class ProjectHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _session;

        public ProjectHttpClient(HttpClient httpClient, ILocalStorageService session) 
        {
            _httpClient = httpClient;
            _session = session;
        }

        public async Task<T[]> PostAsJsonAsync<T>(string endpoint, object body)
        {
            try
            {
                if (body != null)
                {
                    var response = await _httpClient.PostAsJsonAsync(endpoint, body);

                    if (response.IsSuccessStatusCode)
                    {
                        var returnType = await response.Content.ReadFromJsonAsync<RespuestaDTO>()!;
                        if (returnType is not null && returnType.CodigoError == 0 && returnType.Body != "[]")
                        {
                            var ret = JsonConvert.DeserializeObject<T[]>(returnType.Body)!;
                            return ret;
                        }
                        return JsonConvert.DeserializeObject<T[]>(string.Empty)!;
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T[]>(string.Empty)!;
                    }
                }
                else
                {
                    return JsonConvert.DeserializeObject<T[]>(string.Empty)!;
                }
            }
            catch (Exception ex)
            {

                //throw;
                return JsonConvert.DeserializeObject<T[]>(string.Empty)!;
            }

        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest)
        {
            var postResponse = await _httpClient.SendAsync(httpRequest);
            return postResponse;
        }

        public async Task<string> validaPermiso(PermisosXPagina permisosXPagina)
        {
            try
            {
                permisosXPagina.codigoUsuario = await _session.GetItemAsync<string>("user");
                var postRequest = new HttpRequestMessage(HttpMethod.Post, EndPoints.verificarPermisos)
                {
                    Content = JsonContent.Create(permisosXPagina)
                };

                var postResponse = await _httpClient.SendAsync(postRequest);
                if (postResponse.IsSuccessStatusCode)
                {
                    var returnType = await postResponse.Content.ReadFromJsonAsync<RespuestaDTO>()!;
                    if (returnType.CodigoError == 0)
                    {
                        return "";
                    }
                    else
                    {
                        return returnType.MensajeError;
                    }
                }
                else
                {
                    return "Error en el servicio de validar los permisos";
                }
            }
            catch
            {

                return "Error de servidor";
            }

        }

    }
}
