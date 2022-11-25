using Microsoft.EntityFrameworkCore;

namespace VesselWebCenter.Services.ViewModels
{
	public class PagingList<T> : List<T>
	{
		public int	PageIndex { get;private set; }
		public int	TotalPages { get; set; }
		public PagingList(List<T> items, int count, int pageIndex, int pageSize)
		{
			this.PageIndex = pageIndex;
			this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			this.AddRange(items);
		}
		public bool PreviousPage 
		{ 
			get 
			{
				return (this.PageIndex > 1); 
			}
		}
		public bool NextPage 
		{
			get 
			{ 
				return (this.PageIndex < this.TotalPages); 
			}
		}

		public static async Task<PagingList<T>> CreatePagesAsync(IQueryable<T> source, int pageIndex, int pageSize) 
		{
			var count = await source.CountAsync();
			var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PagingList<T>(items, count, pageIndex, pageSize);
		}
	}
}
