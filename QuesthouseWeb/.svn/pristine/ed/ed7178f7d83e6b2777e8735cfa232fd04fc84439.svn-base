namespace QuesthouseWeb.Models.ViewModels
{
	public class ProductListViewModel
	{
		// 상품 목록
		public List<Product> Items { get; set; }

		// 현재 페이지 번호
		public int Page { get; set; }

		// 한 페이지에 보여줄 항목 수
		public int PageSize { get; set; }

		// 전체 아이템 수
		public int TotalItems { get; set; }

		// 전체 페이지 수 계산 (읽기 전용)
		public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

		// 이전 페이지 존재 여부
		public bool HasPreviousPage => Page > 1;

		// 다음 페이지 존재 여부
		public bool HasNextPage => Page < TotalPages;

		public int? SelectedCategoryId { get; set; }

	}
}
