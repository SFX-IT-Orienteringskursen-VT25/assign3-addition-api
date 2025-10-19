namespace AdditionApi
{
    public class Number
    {
        public List<int> Numbers { get; set; } = new List<int>();
        public int Sum => Numbers.Sum();
    }
}
