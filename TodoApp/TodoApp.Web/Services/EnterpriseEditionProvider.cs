using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Web.Services
{
    public class EnterpriseEditionProvider : IEditionProvider
    {
        public string GetEdition()
        {
            return "Enterprise";
        }
    }
}
