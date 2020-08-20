using System.Collections.Generic;

namespace BusinessLogicLayer.Interfaces
{
    /// <typeparam name="T1">model</typeparam>
    /// <typeparam name="T2">DTO</typeparam>
    public interface IMapper<T1, T2>
    {
        T2 GetDTO(T1 model);
        T1 GetNewModel(T2 dto);
        IEnumerable<T2> GetDTOs(IEnumerable<T1> models);
        IEnumerable<T1> GetNewModels(IEnumerable<T2> dtos);
    }
}