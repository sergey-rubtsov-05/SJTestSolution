namespace DataModel.Common
{
    public class PageSettings
    {
        public PageSettings(int number, int size)
        {
            Number = number;
            Size = size;
        }

        public int Size { get; set; }
        public int Number { get; set; }
        public int Skip => Size * (Number - 1);
    }
}