using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainWs.Excepcions
{
    public class HttpResponseException : Exception
    {
        /// <summary>
        /// Excepction status (404 not found 400 bad request 500 internal error)
        /// </summary>
        public int Status { get; set; } = 500;

        public object Value { get; set; }

        public HttpResponseException(int status, string value)
        {
            this.Status = status;
            this.Value = value;
        }
    }
}
