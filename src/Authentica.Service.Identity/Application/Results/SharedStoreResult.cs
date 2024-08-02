namespace Application.Results;

public class SharedStoreResult : BaseResult<SharedStoreResult>
{
    public static new SharedStoreResult Success()
    {
        return new SharedStoreResult
        {
            Succeeded = true
        };
    }
}
