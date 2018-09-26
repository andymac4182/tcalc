namespace tcalc.Evaluation
{
    public class BooleanResult : Result
    {
        public bool Result { get; }

        public BooleanResult(bool result)
        {
            Result = result;
        }

        public override string ToString()
        {
            return Result.ToString();
        }
    }
}