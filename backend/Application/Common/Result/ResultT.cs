namespace Application.Common.Result;

public class Result<T> : Result
{        
    private readonly T? _value;     
    
    public T? Value { get => _value; }        

    private Result(T value, ResultStatus resultStatus) : base(resultStatus)
    {           
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    private Result(string error, ResultStatus resultStatus) : base(error, resultStatus) 
    {
        _value = default;
    }

    public static Result<T> Success(T result, ResultStatus resultStatus)
    {
        return new Result<T>(result, resultStatus);
    }

    new public static Result<T> Failure(string error, ResultStatus resultStatus)
    {
        return new Result<T>(error, resultStatus);
    }
}
