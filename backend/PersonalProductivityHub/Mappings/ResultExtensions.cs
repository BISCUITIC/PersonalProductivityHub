using Application.Common.Result;
using Azure;

namespace PersonalProductivityHub.Mappings;

public static class ResultExtensions
{    
    public static IResult ToHttpResult<T, TResult>(
        this Result<T> result,
        Func<T, TResult> map,
        Func<TResult, string>? locationFactory = null) 
    {
        if (result.IsSuccess)
        {                        
            TResult response = map(result.Value!);

            if (locationFactory is not null)
            {
                return MapSuccessT(response, locationFactory(response), result.ResultStatus);
            }
            else
            {
                return MapSuccessT(response, null, result.ResultStatus);
            }            
        }

        return MapError(result.Error, result.ResultStatus);
    }        
    
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return MapSuccess(result.ResultStatus);
        }

        return MapError(result.Error, result.ResultStatus);
    }
  
    private static IResult MapError(string? error, ResultStatus status)
    {
        return status switch
        {
            ResultStatus.BadRequest => Results.Problem(title: error, statusCode: StatusCodes.Status400BadRequest),
            ResultStatus.Unauthorized => Results.Problem(statusCode: StatusCodes.Status401Unauthorized),
            ResultStatus.Forbidden => Results.Problem(statusCode: StatusCodes.Status403Forbidden),
            ResultStatus.NotFound => Results.Problem(title: error, statusCode: StatusCodes.Status404NotFound),
            ResultStatus.Conflict => Results.Problem(title: error, statusCode: StatusCodes.Status409Conflict),
            _ => Results.Problem(title: error, statusCode: StatusCodes.Status500InternalServerError)
        };
    }

    private static IResult MapSuccessT<T>(T value, string? location, ResultStatus status)
    {
        return status switch
        {
            ResultStatus.Success => Results.Ok(value),

            ResultStatus.Created => location is null ? throw new InvalidOperationException("For Created need to pass locationFactory") 
                                                     : Results.Created(location, value),

            ResultStatus.NoContent => Results.NoContent(),

            _ => Results.Ok(value)
        };
    }

    private static IResult MapSuccess(ResultStatus status)
    {
        return status switch
        {
            ResultStatus.Success => Results.Ok(),
            ResultStatus.NoContent => Results.NoContent(),
            _ => Results.Ok()
        };
    }
}