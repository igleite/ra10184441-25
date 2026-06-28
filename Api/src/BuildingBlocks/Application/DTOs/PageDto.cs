namespace BuildingBlocks.Application.DTOs;

public record PageDto<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItemCount { get; set; }
    public IEnumerable<T>? Items { get; set; }
}