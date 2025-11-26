using System;
using System.Collections.Generic;
using AurumLab.Models;
using Microsoft.EntityFrameworkCore;

namespace AurumLab.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dispositivo> Dispositivos { get; set; }

    public virtual DbSet<LocalDispositivo> LocalDispositivos { get; set; }

    public virtual DbSet<Manutencao> Manutencaos { get; set; }

    public virtual DbSet<RegraPerfil> RegraPerfils { get; set; }

    public virtual DbSet<TipoDispositivo> TipoDispositivos { get; set; }

    public virtual DbSet<TipoManutencao> TipoManutencaos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:ConexaoPadrao");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dispositivo>(entity =>
        {
            entity.HasKey(e => e.IdDispositivo).HasName("PK__Disposit__B1EDB8E4282523A2");

            entity.ToTable("Dispositivo");

            entity.Property(e => e.CriadoEm).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Nome).HasMaxLength(120);
            entity.Property(e => e.Observacoes).HasMaxLength(500);
            entity.Property(e => e.SituacaoOperacional)
                .HasMaxLength(30)
                .HasDefaultValue("Operando");

            entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.Dispositivos)
                .HasForeignKey(d => d.IdLocal)
                .HasConstraintName("FK_Dispositivo_LocalDispositivo");

            entity.HasOne(d => d.IdTipoDispositivoNavigation).WithMany(p => p.Dispositivos)
                .HasForeignKey(d => d.IdTipoDispositivo)
                .HasConstraintName("FK_Dispositivo_Tipo");
        });

        modelBuilder.Entity<LocalDispositivo>(entity =>
        {
            entity.HasKey(e => e.IdLocal).HasName("PK__LocalDis__C287B9BB2B7FDDB6");

            entity.ToTable("LocalDispositivo");

            entity.Property(e => e.Nome).HasMaxLength(120);
        });

        modelBuilder.Entity<Manutencao>(entity =>
        {
            entity.HasKey(e => e.IdManutencao).HasName("PK__Manutenc__CD0C6B2A10BAB309");

            entity.ToTable("Manutencao");

            entity.Property(e => e.Criado_em)
                .HasPrecision(0)
                .HasDefaultValueSql("(dateadd(hour,(-3),sysutcdatetime()))");
            entity.Property(e => e.Observacoes).HasMaxLength(500);
            entity.Property(e => e.Responsavel).HasMaxLength(120);
            entity.Property(e => e.StatusManutencao)
                .HasMaxLength(15)
                .HasDefaultValue("Agendada");

            entity.HasOne(d => d.Criado_porNavigation).WithMany(p => p.Manutencaos)
                .HasForeignKey(d => d.Criado_por)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Manutencao_Usuario");

            entity.HasOne(d => d.IdDispositivoNavigation).WithMany(p => p.Manutencaos)
                .HasForeignKey(d => d.IdDispositivo)
                .HasConstraintName("FK_Manutencao_Dispositivo");

            entity.HasOne(d => d.IdTipoManutencaoNavigation).WithMany(p => p.Manutencaos)
                .HasForeignKey(d => d.IdTipoManutencao)
                .HasConstraintName("FK_Manutencao_TipoManutencao");
        });

        modelBuilder.Entity<RegraPerfil>(entity =>
        {
            entity.HasKey(e => e.IdRegra).HasName("PK__RegraPer__E4F2CC2452759F2D");

            entity.ToTable("RegraPerfil");

            entity.HasIndex(e => e.Nome, "UQ__RegraPer__7D8FE3B24F69CF68").IsUnique();

            entity.Property(e => e.Nome).HasMaxLength(40);
        });

        modelBuilder.Entity<TipoDispositivo>(entity =>
        {
            entity.HasKey(e => e.IdTipoDispositivo).HasName("PK__TipoDisp__A9EEE648E344E19D");

            entity.ToTable("TipoDispositivo");

            entity.Property(e => e.Nome).HasMaxLength(80);
        });

        modelBuilder.Entity<TipoManutencao>(entity =>
        {
            entity.HasKey(e => e.IdTipoManutencao).HasName("PK__TipoManu__BE62BE4E20BDC0E0");

            entity.ToTable("TipoManutencao");

            entity.Property(e => e.Nome).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97A105F4C6");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D1053407323D7F").IsUnique();

            entity.Property(e => e.CriadoEm)
                .HasPrecision(0)
                .HasDefaultValueSql("(dateadd(hour,(-3),sysutcdatetime()))");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.NomeCompleto).HasMaxLength(200);
            entity.Property(e => e.NomeUsuario).HasMaxLength(80);
            entity.Property(e => e.Senha).HasMaxLength(32);

            entity.HasOne(d => d.Regra).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RegraId)
                .HasConstraintName("FK_Usuario_Regra");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
