using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections;
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

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;

            //var buffer = new StringBuilder();

            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                await writer.WriteAsync("Example-1");
                /*await writer.WriteAsync("Example");

                await writer.FlushAsync();*/
          /*      Type type = context.Object.GetType();
                Type itemType = type.GetGenericArguments()[0];

                if (type.GetGenericArguments().Length > 0)
                {
                    itemType = type.GetGenericArguments()[0];
                }
                else
                {
                    itemType = type.GetElementType();
                }*/

               /* foreach (var obj in (IEnumerable<object>)context.Object)
                {
                    Console.WriteLine(obj.ToString());*/
                    /*var vals = obj.GetType().GetProperties().Select(pi => new
                    {
                        Value = pi.GetValue(obj, null)
                    });

                    string valueLine = string.Empty;

                    foreach (var val in vals)
                    {
                        if (val.Value is not null)
                        {
                            var _val = val.Value.ToString();
                             
                            if (_val.Contains(","))
                                _val = string.Concat("\"", _val, "\"");

                            if (_val.Contains("\r"))
                                _val = _val.Replace("\r", " ");
                            if (_val.Contains("\n"))
                                _val = _val.Replace("\n", " ");

                            valueLine = string.Concat(valueLine, _val, ";");
                        }
                        else
                        {
                            valueLine = string.Concat(valueLine, string.Empty, ";");
                        }
                    }

                    await writer.WriteLineAsync(valueLine.TrimEnd(";".ToCharArray()));*/
                //}

                await writer.FlushAsync();
                writer.Close();

            }
        }

        /*protected override bool CanWriteType(Type type)
        {
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }

            return false;
        }*/
    }
}
