using System.Net;

namespace Employee_Management_System.Model
{
    public class APIResponse
    {
            public APIResponse()
            {
                ErrorMessages = new List<String>();
            }
            public HttpStatusCode StatusCode { get; set; }
            public bool IsSuccess { get; set; } = true;
            public List<string> ErrorMessages { get; set; }
            public object Result { get; set; }
        }

    }

