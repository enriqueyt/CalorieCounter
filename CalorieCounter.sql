

SET QUOTED_IDENTIFIER OFF;
GO
USE [CalorieCounter];
GO

-- Creating table 'tb_usuarioSet'
CREATE TABLE [cc].[tb_usuario] (
    [id_usuario]		int IDENTITY(1,1) NOT NULL,
    [usuario]			varchar(200)  NOT NULL,
    [usuarioCorreo]		varchar(200)  NULL,
    [usuarioFacebook]	varchar(200)  NULL,
    [usuarioTwiter]		varchar(200)  NULL,
    [contrasena]		varchar(200)  NOT NULL,
    [fechaRegistro]		datetime  NOT NULL,
    [id_cliente]		int  NOT NULL,
    [activo]			int  NOT NULL
);
GO

-- Creating primary key on [id_usuario] in table 'tb_usuarioSet'
ALTER TABLE [cc].[tb_usuario]
ADD CONSTRAINT [PK_tb_usuario] PRIMARY KEY CLUSTERED ([id_usuario] ASC);
GO

-- Creating table 'tb_perfilSet'
CREATE TABLE [cc].[tb_perfil] (
    [id_perfil]		int IDENTITY(1,1) NOT NULL,
    [perfil]		varchar(200)  NOT NULL,
    [descripcion]	varchar(512)  NULL,
    [fechaRegistro] datetime  NOT NULL,
    [activo]		int  NOT NULL
);
GO

-- Creating primary key on [id_perfil] in table 'tb_perfilSet'
ALTER TABLE [cc].[tb_perfil]
ADD CONSTRAINT [PK_tb_perfil] PRIMARY KEY CLUSTERED ([id_perfil] ASC);
GO


-- Creating table 'tb_usuarioPerfilSet'
CREATE TABLE [cc].[tb_usuarioPerfil] (
    [id_usuarioPerfil] int IDENTITY(1,1) NOT NULL,
    [id_usuario] int  NOT NULL,
    [id_perfil] int  NOT NULL
);
GO

-- Creating primary key on [id_usuarioPerfil] in table 'tb_usuarioPerfilSet'
ALTER TABLE [cc].[tb_usuarioPerfil]
ADD CONSTRAINT [PK_tb_usuarioPerfil] PRIMARY KEY CLUSTERED ([id_usuarioPerfil] ASC);
GO

-- Creating table 'tb_clienteSet'
CREATE TABLE [cc].[tb_cliente] (
    [id_cliente]	int IDENTITY(1,1) NOT NULL,
    [nombre]		varchar(50)  NULL,
    [apellido]		varchar(50)  NULL,
    [correo]		varchar(50)  NOT NULL,
    [tipoDocumento] varchar(2)  NULL,
    [documento]		int  NULL,
    [telefono]		varchar(30)  NOT NULL,
    [activo]		int  NOT NULL
);
GO

-- Creating primary key on [id_cliente] in table 'tb_clienteSet'
ALTER TABLE [cc].[tb_cliente]
ADD CONSTRAINT [PK_tb_cliente] PRIMARY KEY CLUSTERED ([id_cliente] ASC);
GO


-- Creating table 'tb_sesionSet'
CREATE TABLE [cc].[tb_sesion] (
    [id_sesion]		int IDENTITY(1,1) NOT NULL,
    [sesion]		varchar(100)  NOT NULL,
    [fechaInicio]	datetime  NOT NULL,
    [fechaUlOper]	datetime  NOT NULL,
    [activo]		int  NOT NULL,
    [id_usuario]	int  NOT NULL
);
GO

-- Creating table 'tb_alimento'
CREATE TABLE [cc].[tb_alimento] (
    [id_alimento] int IDENTITY(1,1) NOT NULL,
    [descripcion] nvarchar(max)  NULL,
    [fechaCreacion] datetime  NULL,
    [activo] int  NULL
   )
GO

-- Creating table 'tb_tipoAlimento'
CREATE TABLE [cc].[tb_tipoAlimento](
    [id_tipoAlimento] int IDENTITY(1,1) NOT NULL,
    [descripcion] nvarchar(max)  NULL,
    [id_alimento] int  NOT NULL,
    [activo] int  NULL
);
GO


-- Creating primary key on [id_alimento] in table 'tb_alimento'
ALTER TABLE [cc].[tb_alimento]
ADD CONSTRAINT [PK_tb_alimento]
    PRIMARY KEY CLUSTERED ([id_alimento] ASC);
GO

-- Creating primary key on [id_tipoAlimento] in table 'tb_tipoAlimento'
ALTER TABLE [cc].[tb_tipoAlimento]
ADD CONSTRAINT [PK_tb_tipoAlimento]
    PRIMARY KEY CLUSTERED ([id_tipoAlimento] ASC);
GO


ALTER TABLE [cc].[tb_tipoAlimento]
ADD  CONSTRAINT [FK_tb_alimentotb_tipoAlimento] FOREIGN KEY([id_alimento])
REFERENCES [cc].[tb_alimento]   ([id_alimento])
GO
ALTER TABLE [cc].[tb_tipoAlimento] CHECK CONSTRAINT [FK_tb_alimentotb_tipoAlimento]
GO


-- Creating primary key on [id_sesion] in table 'tb_sesionSet'
ALTER TABLE [cc].[tb_sesion]
ADD CONSTRAINT [PK_tb_sesion] PRIMARY KEY CLUSTERED ([id_sesion] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY 
-- --------------------------------------------------

-- Creating foreign key on [id_usuario] in table 'tb_usuarioPerfilSet'
ALTER TABLE [cc].[tb_usuarioPerfil]
ADD CONSTRAINT [FK_tb_usuairotb_usuarioPerfil] FOREIGN KEY ([id_usuario])
    REFERENCES [cc].[tb_usuario] ([id_usuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


-- Creating foreign key on [id_perfil] in table 'tb_usuarioPerfilSet'
ALTER TABLE [cc].[tb_usuarioPerfil]
ADD CONSTRAINT [FK_tb_perfiltb_usuarioPerfil] FOREIGN KEY ([id_perfil])
    REFERENCES [cc].[tb_perfil] ([id_perfil])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [id_usuario] in table 'tb_sesionSet'
ALTER TABLE [cc].[tb_sesion]
ADD CONSTRAINT [FK_tb_usuariotb_sesion] FOREIGN KEY ([id_usuario])
    REFERENCES [cc].[tb_usuario]([id_usuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


-- Creating foreign key on [tb_cliente_id_cliente] in table 'tb_usuarioSet'
ALTER TABLE [cc].[tb_usuario]
ADD CONSTRAINT [FK_tb_usuariotb_cliente] FOREIGN KEY ([id_cliente])
    REFERENCES [cc].[tb_cliente]([id_cliente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-----------------------------------------------------


select * from cc.tb_cliente

select * from cc.tb_sesion

select * from cc.tb_usuario
