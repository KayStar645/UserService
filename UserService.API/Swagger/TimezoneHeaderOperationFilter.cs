using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UserService.API.Swagger;

public class TimezoneHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Timezone-Offset",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("+07:00")
            },
            Description = "Timezone offset format ±hh:mm (e.g. +07:00, -05:00)"
        });
    }
}

