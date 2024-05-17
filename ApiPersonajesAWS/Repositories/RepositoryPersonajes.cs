using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
#region PROCEDURES
//DELIMITER $$
//DROP PROCEDURE IF EXISTS UpdatePersonaje;
//create procedure UpdatePersonaje
//(
//IN param_idpersonaje INT,
//IN param_nombre NVARCHAR(60),
//IN param_imagen NVARCHAR(250)
//)
//begin
//	update PERSONAJES
//    set PERSONAJE = param_nombre, IMAGEN = param_imagen
//    where IDPERSONAJE = param_idpersonaje;
//end $$
//DELIMITER ;
#endregion
namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;

        public RepositoryPersonajes(PersonajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync(int id)
        {
            return await this.context.Personajes.FirstOrDefaultAsync(x => x.IdPersonaje == id);
        }

        private async Task<int> GetMaxIdAsync()
        {
            return await this.context.Personajes.MaxAsync(x => x.IdPersonaje) + 1;
        }

        public async Task<Personaje> InsertPersonajeAsync(string nombre,string imagen) 
        {
            Personaje personaje = new Personaje
            {
                IdPersonaje = await this.GetMaxIdAsync(),
                Nombre=nombre,
                Imagen=imagen
            };
            this.context.Personajes.Add(personaje);
            await this.context.SaveChangesAsync();
            return personaje;
        }

        public async Task UpdatePersonajeAsync(int idpersonaje,string nombre, string imagen)
        {
            string sql = "CALL UpdatePersonaje(@param_idpersonaje, @param_nombre, @param_imagen);";
            MySqlParameter paramidpersonaje = new MySqlParameter("@param_idpersonaje", idpersonaje);
            MySqlParameter paramnombre = new MySqlParameter("@param_nombre", nombre);
            MySqlParameter paramimagen = new MySqlParameter("@param_imagen", imagen);
            await context.Database.ExecuteSqlRawAsync(sql, paramidpersonaje, paramnombre, paramimagen);
        }
    }
}
