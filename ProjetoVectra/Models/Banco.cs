namespace ProjetoVectra.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Banco : DbContext
    {
        public Banco()
            : base("name=Banco")
        {
        }

        public virtual DbSet<Pessoa> pessoa { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>()
                .Property(e => e.Cpf)
                .IsUnicode(false);

            modelBuilder.Entity<Pessoa>()
                .Property(e => e.Nome)
                .IsUnicode(false);
        }
    }
}
