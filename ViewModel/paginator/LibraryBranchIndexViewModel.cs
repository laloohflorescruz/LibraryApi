namespace LibraryApi.ViewModel
{
    public class LibraryBranchIndexViewModel
    {
        public required List<LibraryBranchViewModel> LibraryBranches { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }
    }
}