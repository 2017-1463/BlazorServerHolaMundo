using ERP.Web.Data;
using ERP.Web.Domain.Dto;
using ERP.Web.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Web.Services;

public class EmpleadoService : IEmpleadoService
{
    private readonly AppDbContext _context;

    public EmpleadoService(AppDbContext context)
    {
        _context = context;
    }

    /// Consultar los empleados existentes
    public async Task<List<EmpleadoDto>> Consultar(string filtro)
    {
        var empleados = await _context.Empleados
            .Include(e => e.DatosPersonales)  // Asegúrate de incluir los datos de Persona
            .Where(e => e.DatosPersonales.Nombre.Contains(filtro))  // Filtra por Nombre de Persona
            .Select(e => new EmpleadoDto()
            {
                Id = e.Id,
                PersonaId = e.PersonaId,
                Sueldo = e.Sueldo,
                DatosPersonales = new PersonaDto() // Mapea Persona a PersonaDto
                {
                    Id = e.DatosPersonales.Id,
                    Nombre = e.DatosPersonales.Nombre,
                    FechaDeNacimiento = e.DatosPersonales.FechaDeNacimiento
                }
            })
            .ToListAsync();
        return empleados;
    }

    public async Task<bool> Crear(EmpleadoDto request)
    {
        // Crea la entidad Empleado y asigna los datos de Persona
        var empleado = Empleado.Create(
            request.DatosPersonales.Nombre,
            request.DatosPersonales.FechaDeNacimiento,
            request.Sueldo
        );

        // Agrega el nuevo Empleado
        _context.Empleados.Add(empleado);
        return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task<bool> Modificar(EmpleadoDto request)
    {
        var empleado = await _context.Empleados
            .Include(e => e.DatosPersonales)  // Incluye la entidad relacionada Persona
            .FirstOrDefaultAsync(e => e.Id == request.Id);

        if (empleado == null) return false;

        // Modifica los datos de la entidad Empleado
        empleado.DatosPersonales.Nombre = request.DatosPersonales.Nombre;
        empleado.DatosPersonales.FechaDeNacimiento = request.DatosPersonales.FechaDeNacimiento;
        empleado.Sueldo = request.Sueldo;

        return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task<bool> Eliminar(int Id)
    {
        var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Id == Id);
        if (empleado == null) return false;

        // Elimina el empleado de la base de datos
        _context.Empleados.Remove(empleado);
        return (await _context.SaveChangesAsync()) > 0;
    }
}

