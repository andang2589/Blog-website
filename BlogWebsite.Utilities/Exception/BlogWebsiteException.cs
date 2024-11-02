using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Utilities.Exception
{
    public class BlogWebsiteException : System.Exception
    {
        public BlogWebsiteException() { }

        public BlogWebsiteException(string message):base(message) { }

    }
}
