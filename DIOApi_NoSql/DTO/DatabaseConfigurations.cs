using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public interface IDatabaseConfigurations
    {
        string DatabaseName { get; set; }
        string TableName { get; set; }
    }

    public class DatabaseConfigurations: IDatabaseConfigurations
    {
        public DatabaseConfigurations(string d_name, string t_name)
        {
            DatabaseName = d_name;
            TableName = t_name;
        }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
    }
}
