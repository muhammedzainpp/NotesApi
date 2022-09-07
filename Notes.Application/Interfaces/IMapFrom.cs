using AutoMapper;

namespace Notes.Application.Interfaces;

public interface IMapFrom<T>
{
    void MapFrom(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
