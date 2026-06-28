using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Query;

namespace BuildingBlocks.Application.Queries;

public record PageQuery<T> : IQuery<PageDto<T>>
{
    private int _pageIndex;
    private int _pageSize;

    public int PageIndex
    {
        get => _pageIndex;
        init
        {
            if (value < 1)
                throw new ArgumentException("A página deve ser maior ou igual a 1.", nameof(PageIndex));
            _pageIndex = value;
        }
    }

    public int PageSize
    {
        get => _pageSize;
        init
        {
            if (value > 100)
                throw new ArgumentException("O número de itens por página não pode ser maior que 100.", nameof(PageSize));
            _pageSize = value;
        }
    }

    public PageQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}