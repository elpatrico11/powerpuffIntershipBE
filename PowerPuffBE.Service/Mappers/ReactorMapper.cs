﻿namespace PowerPuffBE.Service.Mappers;

using Data.Entities;
using Helpers;
using Model;
using Model.Enums;

public interface IReactorMapper
{
    ReactorDTO MapToDTO(ReactorEntity entity);
    IEnumerable<ReactorDTO> MapListToDTO(List<ReactorEntity> entityList);
    ReactorDTO MapToDTOWithDetails(ReactorEntity entity);
    ReactorDTO MapToDTOWithImage(ReactorEntity reactor,ImageEntity image);
}

public class ReactorMapper : IReactorMapper
{
    public ReactorDTO MapToDTO(ReactorEntity entity)
    {
        return new ReactorDTO()
        {
            Id = entity.Id,
            Description = entity.Description,
            Name = entity.Name,
            Status = ((ReactorStatusEnum)entity.Status).ToString().ToLower()
        };
    }

    public IEnumerable<ReactorDTO> MapListToDTO(List<ReactorEntity> entityList)
    {
        return entityList.Select(MapToDTOWithDetails);
    }


    public ReactorDTO MapToDTOWithDetails(ReactorEntity entity)
    {
        return new ReactorDTO()
        {
            Id = entity.Id,
            Description = entity.Description,
            Name = entity.Name,
            Status = ((ReactorStatusEnum)entity.Status).GetDescription(),
            ReactorCoreTemperature = entity.ProductionChecks?.Select(pc =>
            {
                return new ReactorChartDTO()
                {
                    Time = pc.MeasureTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    Value = pc.Temperature,
                    Status = GetStatus(pc.PowerProduction) // TODO - przekazac liczenie statusu dla studentow
                };
            }) ?? Array.Empty<ReactorChartDTO>(),
            ReactorPowerProduction = entity.ProductionChecks?.Select(pc =>
            {
                return new ReactorChartDTO()
                {
                    Time = pc.MeasureTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    Value = pc.PowerProduction,
                    Status = GetStatus(pc.PowerProduction)
                };
            }) ?? Array.Empty<ReactorChartDTO>(),
            //Links = generowac linki , poki co hardcoded 
        };
    }

    public ReactorDTO MapToDTOWithImage(ReactorEntity reactor, ImageEntity image)
    {
        return new ReactorDTO()
        {
            Id = reactor.Id,
            Name = reactor.Name,
            Description = reactor.Description,
            ImageContent = image == null ? "No image found" : "data:image/png;base64," + Convert.ToBase64String(image.Image)
        };
    }

    private string GetStatus(int value)
    {
        switch (value)
        {
            case <= 50 :
                return ReactorStatusEnum.InRange.GetDescription();
            case <= 75:
                return ReactorStatusEnum.OutOfRange.GetDescription();
            case > 75:
                return ReactorStatusEnum.Critical.GetDescription();
                
        }
    }
}