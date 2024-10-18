namespace Shared.Core.Commons;
public class Result<T> //: IResult<T>
{
    public Result()
    {
        Succeeded = true;
        Message = "Thành công!";
    }
    public Result(bool? success = true)
    {
        Succeeded = success.Value;
        if (success == true)
            Message = "Thành công!";
        else
            Message = "Có lỗi xảy ra!";
    }
    public Result(T data, string message = "", bool? success = true)
    {
        Succeeded = success.Value;
        Message = message == null ? "" : message;
        Data = data;
    }
    public Result(string message, bool? success = true)
    {
        Succeeded = success.Value;
        Message = message;
    }
    public Result(Exception ex)
    {
        Succeeded = false;
        Exception = ex;
        Message = ex.Message;
        Errors.Add(ex.Message);
        Code = -1;
    }
    public int Code { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; } = "";
    public List<string> Messages { get; set; }
    public T Data { get; set; }
    public Exception? Exception { get; set; }

    List<string> errors { get; set; }
    public List<string> Errors { get { if (errors == null) errors = new List<string>(); return errors; } set { errors = value; } }
}
