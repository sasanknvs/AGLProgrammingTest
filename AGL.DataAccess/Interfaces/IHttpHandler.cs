using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace AGL.DataAccess.Interfaces
{
    public interface IHttpHandler : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(string url);

        HttpResponseMessage Get(string url);
    }

}
