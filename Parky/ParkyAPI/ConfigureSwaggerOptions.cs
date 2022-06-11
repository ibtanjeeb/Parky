using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;
       

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var dec in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    dec.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"Parky API{dec.ApiVersion}",
                        Version = dec.ApiVersion.ToString()

                    }

                    ) ;
            }
            var xmlCommentfile = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlfullCommentpath = Path.Combine(AppContext.BaseDirectory, xmlCommentfile);
               options.IncludeXmlComments(xmlfullCommentpath);
        }
    }
}
