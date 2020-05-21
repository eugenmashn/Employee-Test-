using Employee_Test_.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Employee_Test_.Services
{
    public class Reader
    {
        public static async   Task<List<Employee>> ReaderAsList(IFormFile formFile, string appEnvironment)
        {  
           
            if (formFile.Length > 0)
            { 
                List<Employee> employees = new List<Employee>();

                string uploads = appEnvironment + "/Files/" + formFile.FileName;
                

                //var filePath = Path.Combine(uploads, formFile.FileName).ToString();
              
                using (var fileStream = new FileStream( uploads, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                    fileStream.Dispose();
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(uploads);
                    XmlElement xRoot = xDoc.DocumentElement;
                    foreach (XmlElement xnode in xRoot)
                    {
                        string TeamName = "";
                        XmlNode attr = xnode.Attributes.GetNamedItem("Title");
                        if (attr != null)
                             TeamName = attr.Value;

                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            Employee employee = new Employee();
                            employee.TeamName = TeamName;
                            employee.Name = childnode.Attributes["Name"].Value;
                            employee.Position = childnode.Attributes["Position"].Value;
                            employee.HireDate = childnode.Attributes["HireDate"].Value; 
                            employees.Add(employee);
                        }
                       
                    }

                }
                return employees;
            }
            return null;
        }
    }
}
