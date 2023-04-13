using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Extensions.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public string ContentType { get; }

        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var csv = new StringBuilder();

            var type = context.Object.GetType();

            if (type.GetGenericTypeDefinition() == typeof(PagedResults<>))
            {
                var dataObjects = type.GetProperty("Data").GetValue(context.Object);

                var dataObjectsList = (IEnumerable<object>)dataObjects;

                csv.AppendLine(string.Join(",", dataObjectsList.FirstOrDefault().GetType().GetProperties().Select(x => x.Name)));

                foreach (var obj in (IEnumerable<object>)dataObjects)
                {
                    var properties = obj.GetType().GetProperties().Select(
                                pi => new
                                {
                                    Name = pi.Name,
                                    Value = pi.GetValue(obj, null)
                                }
                            );

                    var values = new List<string>();

                    foreach (var property in properties)
                    {
                        if (property.Value is not null)
                        {
                            var value = property.Value.ToString();

                            if (property.Value.GetType() != typeof(string))
                            {
                                value = JsonConvert.SerializeObject(property.Value);
                            }

                            if (value.Contains(","))
                            {
                                value = string.Concat("\"", value, "\"");
                            }

                            value = value.Replace("\r", " ", StringComparison.InvariantCultureIgnoreCase);
                            value = value.Replace("\n", " ", StringComparison.InvariantCultureIgnoreCase);

                            values.Add(value);
                        }
                        else
                        {
                            values.Add(string.Empty);
                        }
                    }

                    csv.AppendLine(string.Join(",", values));
                }
            }
            else
            {
                csv.AppendLine(string.Join(",", type.GetProperties().Select(x => x.Name)));

                foreach (var obj in (IEnumerable<object>)context.Object)
                {
                    var properties = obj.GetType().GetProperties().Select(
                        pi => new
                        {
                            Name = pi.Name,
                            Value = pi.GetValue(obj, null)
                        }
                    );

                    var values = new List<string>();

                    foreach (var property in properties)
                    {
                        if (property.Value is not null)
                        {
                            var value = property.Value.ToString();

                            if (property.Value.GetType() != typeof(string))
                            {
                                value = JsonConvert.SerializeObject(property.Value);
                            }

                            if (value.Contains(","))
                            {
                                value = string.Concat("\"", value, "\"");
                            }

                            value = value.Replace("\r", " ", StringComparison.InvariantCultureIgnoreCase);
                            value = value.Replace("\n", " ", StringComparison.InvariantCultureIgnoreCase);

                            values.Add(value);
                        }
                        else
                        {
                            values.Add(string.Empty);
                        }
                    }

                    csv.AppendLine(string.Join(",", values));
                }
            }
           
            return context.HttpContext.Response.WriteAsync(csv.ToString(), selectedEncoding);
        }

        protected override bool CanWriteType(Type type)
        {
            return type.IsGenericType;
        }
    }
}
