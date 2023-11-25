namespace LibraryApi.ViewModel
{
    public class CustomerIndexViewModel
    {
        public required List<CustomerViewModel> Customer { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }
    }
}