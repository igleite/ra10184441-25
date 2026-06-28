using Tickets.Application.DTOs;
using Tickets.Domain.Entities;

namespace Tickets.Application.Mappings;

public static class ChatMappings
{
    public static ChatDto ToDto(this Chat entity)
    {
        var dto = new ChatDto();
        dto.Id = entity.Id;
        dto.CreatedAt = entity.CreatedAt;
        dto.UpdatedAt = entity.UpdatedAt;
        dto.RowVersion = entity.RowVersion;
        dto.DeletedAt = entity.DeletedAt;
        dto.TicketId = entity.TicketId;
        dto.UserId = entity.UserId;
        dto.Message = entity.Message;

        return dto;
    }

    public static Chat ToEntity(this ChatDto dto)
    {
        var dateNow = dto.UpdatedAt;
        var entity = new Chat(dto.Id, dateNow);
        entity.SetDeletedAt(dto.DeletedAt, dateNow);
        entity.SetTicketId(dto.TicketId, dateNow);
        entity.SetUserId(dto.UserId, dateNow);
        entity.SetMessage(dto.Message, dateNow);

        return entity;
    }
}