/*
 * @Author: Jun
 * @Description:
 */

using System.Reflection;
using MinimalApis.Minimal;

namespace MinimalApis.EndPoints;

public static class EndPointExtensions
{
    public static WebApplication RegisterEndPoints(this WebApplication app)
    {
        var type = Assembly.GetEntryAssembly()?.GetTypes()?.Where(type => type.IsAssignableTo(typeof(IEndPoint))
                                                                          && type is
                                                                          {
                                                                              IsAbstract: false, IsInterface: false
                                                                          });
        if (type?.Count() > 0)
        {
            foreach (var item in type)
            {
                var ep = ActivatorUtilities.CreateInstance(app.Services, item);
                (ep as IEndPoint)!.MapEndPoints(app);
            }
        }

        return app;
    }
}