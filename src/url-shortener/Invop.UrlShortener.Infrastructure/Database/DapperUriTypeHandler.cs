using System.Data;
using Dapper;

namespace Invop.UrlShortener.Infrastructure.Database;

public class DapperUriTypeHandler : SqlMapper.TypeHandler<Uri>
{
    public override Uri? Parse(object value)
    {
        if (value is null or DBNull)
        {
            return null;
        }

        var uriString = value as string;
        return string.IsNullOrEmpty(uriString) ? null : new Uri(uriString);
    }

    public override void SetValue(IDbDataParameter parameter, Uri? value)
    {
        parameter.Value = value?.ToString();
    }
}