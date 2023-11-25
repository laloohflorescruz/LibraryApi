namespace LibraryApi.ViewModel
{
    public class AuthorIndexViewModel
    {
        public required List<AuthorViewModel> Author { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }
    }
}