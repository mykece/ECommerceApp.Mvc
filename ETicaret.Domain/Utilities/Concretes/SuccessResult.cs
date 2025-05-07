namespace ETicaret.Domain.Utilities.Concretes
{
    public class SuccessResult : Result
    {
        public SuccessResult() : base(true) { }

        public SuccessResult(string messages) : base(true, messages) { }
    }
}
