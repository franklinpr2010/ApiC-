﻿using System.Data.Entity.ModelConfiguration;
using XGame.Domain.Entities;

namespace XGame.Infra.Persistence.Map
{
    //Mapeamento de plataforma
    public class MapPlataforma : EntityTypeConfiguration<Plataforma>
    {
        public MapPlataforma()
        {
            ToTable("Plataforma");

            Property(p => p.Nome).HasMaxLength(50).IsRequired();
        }
    }
}
