namespace ETicaret.Domain.Utilities.Concretes
{
    public class ErrorResult : Result
    {
        public ErrorResult() : base(false) { }

        public ErrorResult(string messages) : base(false, messages) { }
    }
}
