using System.Net;

namespace Sales.Web.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; }

        public T? Response { get; }

        public HttpResponseMessage HttpResponseMessage { get; }

        public async Task<string?> GetErrorMessage()
        {
            if (!Error) return null;

            var codigoStatus = HttpResponseMessage.StatusCode;
            if (codigoStatus == HttpStatusCode.NotFound)
            {
                return "Recurso no Encontrado.";

            }
            else if (codigoStatus == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (codigoStatus == HttpStatusCode.Unauthorized)
            {
                return "Tiene que logearte para hacer esta operación";
            }
            else if (codigoStatus == HttpStatusCode.Forbidden)
            {
                return "NO tienes permiso para hacer esta operación";
            }


            return "Ha ocurrido un error inesperados.";
        }
    }
}
