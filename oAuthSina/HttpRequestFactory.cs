namespace LeoShi.Soft.OpenSinaAPI
{
    public class HttpRequestFactory
    {
        private HttpRequestFactory(){}

        public static BaseHttpRequest CreateHttpRequest(Method method)
        {
            if(method == Method.GET)
            {
                return new HttpGet();
            }
            return new HttpPost();
        }
    }
}