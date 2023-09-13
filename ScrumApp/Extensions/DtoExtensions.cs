using Scrum.Shared;

namespace ScrumApp.Extensions;

public static class DtoExtensions
{
    public static string Url(this ProductListShortDto dto) => $"/product/view/{dto.Id}";
    public static string Url(this ScrumApi.ProductShort dto) => $"/product/view/{dto.Id}";
    public static string Url(this ScrumApi.ProductBacklogItem dto) => $"/productbacklogitem/view/{dto.Id}";
    public static string Url(this ProductListDto dto) => $"/product/view/{dto.Id}";
    public static string Url(this ProductBacklogItemFullDto dto) => $"/product/view/{dto.Id}";
    public static string Url(this SprintListDto dto) => $"/sprint/view/{dto.Id}";
    public static string Url(this ScrumApi.SprintShort dto) => $"/sprint/view/{dto.Id}";
    public static string Url(this ProductBacklogItemListDto dto) => $"/productbacklogitem/view/{dto.Id}";
    public static string SprintUrl(this ProductBacklogItemFullDto dto) => $"/sprint/view/{dto.SprintId}";
    public static string ProductUrl(this ProductBacklogItemFullDto dto) => $"/product/view/{dto.ProductId}";

    public static string SprintUrl(this SprintBacklogItemFullDto dto) => $"/sprint/view/{dto.SprintId}";
    //public static string ProductUrl(this SprintBacklogItemFullDto dto) => $"/product/view/{dto.ProductId}";
    public static string ProductBacklogItemUrl(this SprintBacklogItemFullDto dto) => $"/productbacklogitem/view/{dto.ProductBacklogItemId}";
    public static string Url(this ProductBacklogItemDependentDto dto) => $"/productbacklogitem/view/{dto.Id}";
}
