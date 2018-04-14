using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data;

namespace TodoApp.Web.Auth
{
    public interface IOptionsMapper<TOptions> where TOptions : class, new()
    {
        TOptions CreateOptions(string scheme, Tenant tenant);
    }
}
