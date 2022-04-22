using Havan.CQRS.Core.Util;
using Newtonsoft.Json;
using RestSharp;

namespace TemplateRestCommunication
{
    public class RequestRest 
    {
        
        public RequestRest()
        {
            
        }
        NotificarErro notify = new NotificarErro();
        public T Post<T>(string host, string url, object objeto = null, List<Tuple<string, string>> parametros = null) where T : new()
        {
            try
            {
                var client = new RestClient(host);
                var request = new RestRequest(url, Method.POST);

                if (parametros != null)
                {
                    foreach (var item in parametros)
                    {
                        request.AddParameter(item.Item1, item.Item2);
                    }
                }

                if (objeto == null)
                {
                    return client.Execute<T>(request).Data;
                }

                request.AddJsonBody(JsonConvert.SerializeObject(objeto));

                var requestDto = client.Execute<T>(request);

                var retorno = requestDto.Data;

                return RetornoApi<T>(retorno);
            }
            catch (Exception e)
            {
                notify.AdicionarErro($"Erro ao realizar chamada de api Host: {host} e url: {url}  " + e.Message);
                notify.AdicionarErro(e);
                throw;
            }
        }
        public T Get<T>(string host, string url, List<Tuple<string, string>> parametros = null) where T : new()
        {

            var client = new RestClient(host);
            var request = new RestRequest(url, Method.GET);

            if (parametros == null)
            {
                var retorno = client.Execute<T>(request);
                if (retorno.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return JsonConvert.DeserializeObject<T>(retorno.Content);

                return retorno.Data;
            }

            foreach (var item in parametros)
            {
                request.AddParameter(item.Item1, item.Item2);
            }
            return client.Execute<T>(request).Data;
        }
        private T RetornoApi<T>(T retorno)
        {
            
            try
            {
                if (!typeof(T).GetInterfaces().Contains(typeof(IApiResponse)))
                    return retorno;

                var retornoResponse = (IApiResponse)retorno;
                if (!retornoResponse.Success)
                {
                    retornoResponse.Errors?.ForEach(x => notify.AdicionarErro(x));
                }

                return retorno;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }
    }
}
