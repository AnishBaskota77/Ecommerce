using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Library
{
    public static class General
    {
        public static DynamicParameters PrepareDynamicParameters<T>(this T item)
        {
            var properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var model = new DynamicParameters();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetCustomAttribute<NotMappedAttribute>() != null)
                    continue;
                var val = prop.GetValue(item); model.Add(prop.Name, val);
            }
            return model;
        }
    }
}
