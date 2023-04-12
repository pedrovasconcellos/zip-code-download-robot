CREATE TABLE [dbo].[ZipeCode](
	[cep] [nvarchar](128) NOT NULL,
	[bairro] [nvarchar](max) NULL,
	[logradouro] [nvarchar](max) NULL,
	[cidade] [nvarchar](max) NULL,
	[estado] [nvarchar](max) NULL,
	[ddd] [smallint] NULL,
	[ibge] [int] NULL,
	[altitude] [decimal](18, 2) NULL,
	[latitude] [decimal](18, 2) NULL,
	[longitude] [decimal](18, 2) NULL,
	[datetime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[cep] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO