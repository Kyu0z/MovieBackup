namespace XemPhim.Interfaces.CRUD
{
    public class Paging
    {
        public int Size { get; set; }
        public int Page { get; set; }

        public static Paging From(int size, int page)
        {
            return new Paging()
            {
                Size = size,
                Page = page,
            };
        }
    }
}