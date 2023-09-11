# Gerenciamento de Fichas Médicas

Este projeto é um sistema de gerenciamento de fichas médicas desenvolvido em ASP.NET Core, destinado a clínicas médicas e profissionais de saúde. Ele permite que os médicos e administradores criem, visualizem, editem e excluam fichas médicas de pacientes.

# Instruções de Uso
- Clone o projeto em sua máquina local.
- Abra o projeto em um ambiente de desenvolvimento compatível (Visual Studio, Visual Studio Code, etc.).
- Configure a conexão com o banco de dados no arquivo appsettings.json.
- Execute as migrações para criar o banco de dados e as tabelas necessárias.
- Execute a aplicação e acesse-a em seu navegador.
- Cadastre novas fichas médicas, edite informações existentes e experimente as funcionalidades oferecidas pela aplicação.

## Funcionalidades

- Cadastro e edição de fichas médicas
- Associação de pacientes e médicos às fichas médicas
- Upload de fotos para identificação do paciente
- Formatação de texto enriquecido para notas médicas
- Visualização de lista de fichas médicas com filtros
- Interface amigável e responsiva

## Tecnologias Utilizadas

- ASP.NET Core MVC
- Entity Framework Core
- HTML, CSS, JavaScript
- Bootstrap
- Razor

## Banco De Dados
-- Tabela "Funcoes"
CREATE TABLE Funcoes (
    FuncaoID INT PRIMARY KEY IDENTITY(1,1),
    NomeFuncao NVARCHAR(50) NOT NULL
);

-- Tabela "Usuarios"
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    NomeUsuario NVARCHAR(50) NOT NULL,
    SenhaHash NVARCHAR(100) NOT NULL,
    FuncaoID INT ,
    FOREIGN KEY (FuncaoID) REFERENCES Funcoes(FuncaoID)
);

-- Tabela "FichasMedicas"
CREATE TABLE FichasMedicas (
    FichaID INT PRIMARY KEY IDENTITY(1,1),
    PacienteID INT,
    MedicoID INT,
    NomeCompleto NVARCHAR(100) NOT NULL,
    Foto NVARCHAR(200),
    CPF NVARCHAR(14) NOT NULL,
    Celular NVARCHAR(20) NOT NULL,
    Endereco NVARCHAR(200),
    TextoRico NVARCHAR(MAX),
    FOREIGN KEY (PacienteID) REFERENCES Usuarios(UsuarioID),
    FOREIGN KEY (MedicoID) REFERENCES Usuarios(UsuarioID)
);

-- Inserir Função Paciente
INSERT INTO Funcoes (NomeFuncao) VALUES ('Paciente');

-- Inserir Função Médico
INSERT INTO Funcoes (NomeFuncao) VALUES ('Médico');

## MER

![image](https://github.com/matheusleonor/FichaMedica/assets/37985264/1207a70c-34e0-419c-b1d8-126ed1c6502a)
