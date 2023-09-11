using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeFichasMedicas.Models;

public partial class HospitalContext : DbContext
{
    public HospitalContext()
    {
    }

    public HospitalContext(DbContextOptions<HospitalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FichasMedicas> FichasMedicas { get; set; }

    public virtual DbSet<Funcoes> Funcoes { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MATHEUS\\MSSQLSERVER2;Database=HOSPITAL;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FichasMedicas>(entity =>
        {
            entity.HasKey(e => e.FichaId).HasName("PK__FichasMe__0C37E5409554F3C6");

            entity.Property(e => e.FichaId).HasColumnName("FichaID");
            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .HasColumnName("CPF");
            entity.Property(e => e.Endereco).HasMaxLength(200);
            entity.Property(e => e.Foto).HasMaxLength(200);
            entity.Property(e => e.MedicoId).HasColumnName("MedicoID");
            entity.Property(e => e.NomeCompleto).HasMaxLength(100);
            entity.Property(e => e.PacienteId).HasColumnName("PacienteID");

            entity.HasOne(d => d.Medico).WithMany(p => p.FichasMedicaMedicos)
                .HasForeignKey(d => d.MedicoId)
                .HasConstraintName("FK__FichasMed__Medic__571DF1D5");

            entity.HasOne(d => d.Paciente).WithMany(p => p.FichasMedicaPacientes)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK__FichasMed__Pacie__5629CD9C");
        });

        modelBuilder.Entity<Funcoes>(entity =>
        {
            entity.HasKey(e => e.FuncaoId).HasName("PK__Funcoes__7111CEA17F0E2953");

            entity.Property(e => e.FuncaoId).HasColumnName("FuncaoID");
            entity.Property(e => e.NomeFuncao).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7982D0D7A42");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.FuncaoId).HasColumnName("FuncaoID");
            entity.Property(e => e.NomeUsuario).HasMaxLength(50);
            entity.Property(e => e.SenhaHash).HasMaxLength(100);

            entity.HasOne(d => d.Funcao).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.FuncaoId)
                .HasConstraintName("FK__Usuarios__Funcao__534D60F1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
