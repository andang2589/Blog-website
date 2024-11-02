using BlogWebsite.DTO.Common;
using Newtonsoft.Json;
using System.Text;

namespace BlogWebsite.Areas.Admin.Helper
{
    public class HttpClientHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public HttpClientHelper(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public HttpClient CreateConfigClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];



            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            }

            return client;
        }


        public async Task<ClassResult<T>> HandleHttpRespone<T>(HttpResponseMessage response) where T : class 
        {
            var result = await response.Content.ReadAsStringAsync();

            //if(response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    return ClassResult<T>.SuccessResult();
            //}

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return ClassResult<T>.UnauthorizedResult();
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return ClassResult<T>.ForbiddenResult();
            }

            return JsonConvert.DeserializeObject<ClassResult<T>>(result);
        }

        //Phuong thuc tien loi de gui yeu cau GET
        public async Task<ClassResult<T>> SendGetRequest<T>(string endpoint) where T : class
        {
            var client = CreateConfigClient();
            var response = await client.GetAsync(endpoint);
            return await HandleHttpRespone<T>(response);
        }

        //Phuong thuc tien loi de gui yeu cau POST
        public async Task<ClassResult<T>> SendPostRequest<T>(string endpoint, object data) where T : class
        {
            var client = CreateConfigClient();
            var json = JsonConvert.SerializeObject(data);
            var httpClient = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, httpClient);
            return await HandleHttpRespone<T>(response);
        }

        //Phuong thuc tien loi de gui yeu cau PUT
        public async Task<ClassResult<T>> SendPutRequest<T>(string endpoint, object data) where T : class
        {
            var client = CreateConfigClient();
            var json = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endpoint, httpContent);
            return await HandleHttpRespone<T>(response);
        }


        //Phuong thuc tien loi de gui yeu cau DELETE
        public async Task<ClassResult<T>> SendDeleteRequest<T>(string endpoint) where T : class
        {
            var client = CreateConfigClient();
            var response = await client.DeleteAsync(endpoint);
            return await HandleHttpRespone<T> (response);
        }
    }
}
