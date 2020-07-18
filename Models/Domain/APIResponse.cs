namespace Models.Domain
{
    public class APIResponse<T>
    {
        public APIResponse()
        {
        }

        public APIResponse(T val)
        {
            Value = val;
        }

        public T Value { get; set; } = default;

        public string ErrorMessage { get; set; } = null;
    }
}
