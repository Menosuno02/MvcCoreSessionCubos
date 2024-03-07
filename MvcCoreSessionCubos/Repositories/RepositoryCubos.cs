using Microsoft.EntityFrameworkCore;
using MvcCoreSessionCubos.Context;
using MvcCoreSessionCubos.Models;
using System.ComponentModel;

namespace MvcCoreSessionCubos.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;

        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        public async Task<int> GetMaxIdCompra()
        {
            if (await this.context.Compras.CountAsync() == 0) return 1;
            return await this.context.Compras
                .MaxAsync(c => c.IdCompra) + 1;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<List<Cubo>> GetCubosSessionAsync(List<int> cubos)
        {
            return await this.context.Cubos
                .Where(c => cubos.Contains(c.IdCubo))
                .ToListAsync();
        }

        public async Task<List<Compra>> CreateCompraAsync(List<int> cubos)
        {
            List<Cubo> listCubos = await GetCubosSessionAsync(cubos);
            int idCompra = await GetMaxIdCompra();
            List<Compra> compras = new List<Compra>();
            foreach(Cubo cubo in listCubos)
            {
                Compra compra = new Compra
                {
                    IdCompra = idCompra,
                    NombreCubo = cubo.Nombre,
                    Precio = cubo.Precio,
                    fechapedido = DateTime.Now
                };
                compras.Add(compra);
                this.context.Compras.AddAsync(compra);
            }
            await this.context.SaveChangesAsync();
            return compras;
        }
    }
}
