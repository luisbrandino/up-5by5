USE DBAgency

CREATE TABLE [Deleted] (
          [Number] nvarchar(450) NOT NULL,
          [Cnpj] nvarchar(max) NOT NULL,
          [Restriction] bit NOT NULL,
          [AddressZipcode] nvarchar(max) NOT NULL,
          CONSTRAINT [PK_Deleted] PRIMARY KEY ([Number])
      );