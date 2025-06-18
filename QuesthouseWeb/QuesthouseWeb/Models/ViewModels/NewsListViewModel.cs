using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuesthouseWeb.Models.ViewModels
{
	public class NewsListViewModel
	{
		public List<News> NewsItems { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalItems { get; set; }

		public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

		// 이전 페이지 존재 여부
		public bool HasPreviousPage => CurrentPage > 1;

		// 다음 페이지 존재 여부
		public bool HasNextPage => CurrentPage < TotalPages;
	}
}
