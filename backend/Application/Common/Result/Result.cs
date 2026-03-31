namespace Application.Common.Result;

public class Result
{
    private readonly bool _success;    
    private readonly string? _error;
    private readonly ResultStatus _resultStatus;

    public bool IsSuccess { get => _success; }
    public bool IsFailure { get => !_success; }
    
    public string? Error { get => _error; }
    public ResultStatus ResultStatus { get => _resultStatus; }

    protected Result(ResultStatus resultStatus)
    {
        _success = true;
        _error = default;
        _resultStatus = resultStatus;
    }

    protected Result(string error, ResultStatus resultStatus)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Error message is required", nameof(error));

        _success = false;
        _error = error;
        _resultStatus = resultStatus;
    }

    public static Result Success(ResultStatus resultStatus)
    {
        return new Result(resultStatus);
    }

    public static Result Failure(string error, ResultStatus resultStatus)
    {
        return new Result(error, resultStatus);
    }
}
