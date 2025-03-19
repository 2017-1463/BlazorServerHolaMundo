using ERP.Web.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Web.Domain.Dto
{
    public interface IEmpleadoService
    {
        Task<List<EmpleadoDto>> Consultar(string filtro);
        Task<bool> Crear(EmpleadoDto request);
        Task<bool> Modificar(EmpleadoDto request);
        Task<bool> Eliminar(int Id);
    }
}
