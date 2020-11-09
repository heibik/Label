--
-- Aufkleber
--

USE db_lkant
GO

-------------------
-- Tabellen Löschen
-------------------


IF OBJECT_ID(N'AUFKLEBER.mod_mat',N'U')IS NOT NULL BEGIN
ALTER TABLE AUFKLEBER.mod_mat
DROP
CONSTRAINT fk_mod_mat_mod
ALTER TABLE AUFKLEBER.mod_mat
DROP
CONSTRAINT fk_mod_mat_mat
END
GO

IF OBJECT_ID(N'Aufkleber.Artikel_art',N'U')IS NOT NULL BEGIN
ALTER TABLE Aufkleber.Artikel_art
DROP
CONSTRAINT fk_art_mod
ALTER TABLE Aufkleber.Artikel_art
DROP
CONSTRAINT fk_art_fab
ALTER TABLE Aufkleber.Artikel_art
DROP
CONSTRAINT fk_art_gro
END
GO


IF OBJECT_ID(N'AUFKLEBER.Modell_mod',N'U')IS NOT NULL BEGIN
ALTER TABLE AUFKLEBER.Modell_mod
DROP
CONSTRAINT fk_mod_typ
END
GO



IF OBJECT_ID(N'AUFKLEBER.Artikel_art',N'U')IS NOT NULL BEGIN
DROP TABlE AUFKLEBER.Artikel_art 
END
GO

IF OBJECT_ID(N'AUFKLEBER.Modell_mod',N'U')IS NOT NULL BEGIN
DROP TABlE AUFKLEBER.Modell_mod
END
GO

IF OBJECT_ID(N'AUFKLEBER.Material_mat',N'U')IS NOT NULL BEGIN
DROP TABlE AUFKLEBER.Material_mat
END
GO

IF OBJECT_ID(N'AUFKLEBER.mod_mat',N'U')IS NOT NULL BEGIN
DROP TABlE AUFKLEBER.mod_mat
END
GO

IF OBJECT_ID(N'AUFKLEBER.Groesse_gro',N'U')IS NOT NULL BEGIN
DROP TABlE AUFKLEBER.Groesse_gro
END
GO

IF OBJECT_ID(N'AUFKLEBER.Farbe_fab',N'U')IS NOT NULL BEGIN
DROP TABlE AUFKLEBER.Farbe_fab
END
GO

IF OBJECT_ID(N'AUFKLEBER.Typ_typ',N'U')IS NOT NULL BEGIN
DROP TABlE AUFKLEBER.Typ_typ
END
GO

--DROP SCHEMA Aufkleber
--GO

IF OBJECT_ID(N'Aufkleber.Unternehmen_un',N'U')IS NOT NULL BEGIN
DROP TABLE Aufkleber.Unternehmen_un
END
GO

IF OBJECT_ID(N'Aufkleber.Entwickler_ent',N'U')IS NOT NULL BEGIN
DROP TABLE Aufkleber.Entwickler_ent
END
GO

IF OBJECT_ID(N'Aufkleber.LastDruck_lastd',N'U')IS NOT NULL BEGIN
DROP TABLE Aufkleber.LastDruck_lastd
END
GO

---------------------
-- Tabellen Erstellen
---------------------


--CREATE SCHEMA Aufkleber
--GO


CREATE TABLE Aufkleber.Artikel_art
(
	iId				int PRIMARY KEY IDENTITY(1,1),
	sBezeichnung	nvarchar(30) NULL,
	iArtNr			int NOT NULL,
	mod_iId			int NOT NULL,
	fab_iId			int NOT NULL,
	gro_iId			int NOT NULL
)
GO


CREATE TABLE Aufkleber.Farbe_fab
(
	iId				int IDENTITY(1,1) PRIMARY KEY,
	sBezeichnung	nvarchar(30) NOT NULL  UNIQUE
)


CREATE TABLE Aufkleber.Groesse_gro
(
	iId				int IDENTITY(1,1) PRIMARY KEY,
	rGroesse		decimal(5,2) NOT NULL  UNIQUE
)


CREATE TABLE Aufkleber.Modell_mod
(
	iId				int PRIMARY KEY IDENTITY(1,1),
	sBezeichnung	nvarchar(30) NOT NULL UNIQUE,
	sGeschlecht		nchar(1) CHECK(sGeschlecht = 'm' OR sGeschlecht = 'w' OR sGeschlecht = 'u') NOT NULL,
	bSymbolBild		varbinary(max) NULL,
	typ_iId			int NOT NULL,
)
GO

CREATE TABLE Aufkleber.Typ_typ
(
	iId				int PRIMARY KEY IDENTITY(1,1),
	sBezeichnung	nvarchar(30) NOT NULL UNIQUE
)



CREATE TABLE Aufkleber.Material_mat
(
	iId				int PRIMARY KEY IDENTITY(1,1),
	sNummer			nchar(10) UNIQUE NOT NULL,
	sBezeichnung	nvarchar(30) NOT NULL UNIQUE,
)
GO


CREATE TABLE Aufkleber.mod_mat
(
	mod_iId			int,
	mat_iId			int,

	CONSTRAINT pk_mod_mat PRIMARY KEY (mod_iId, mat_iId)
)
GO




CREATE TABLE Aufkleber.Entwickler_ent
(
	iId				int PRIMARY KEY IDENTITY(1,1),
	sVName			nvarchar(30),
	sNName			nvarchar(30) NOT NULL,
	sKeyAufgabe		nvarchar(30)
)
GO



CREATE TABLE Aufkleber.Unternehmen_un
(
	iId				int PRIMARY KEY IDENTITY(1,1),
	sName			nvarchar(30) NOT NULL,
	bLogo			varbinary(max),
	sSlogan			nvarchar(50)
)
GO


CREATE TABLE Aufkleber.LastDruck_lastd
(
	iId				int PRIMARY KEY IDENTITY(1,1),
	iFreiAb			int NOT NULL,
	sPapierFormat	nchar(10)
)
GO





ALTER TABLE Aufkleber.Artikel_art
	ADD
	CONSTRAINT fk_art_mod FOREIGN KEY(mod_iId) REFERENCES Aufkleber.Modell_mod(iId),
	CONSTRAINT fk_art_fab FOREIGN KEY(fab_iId) REFERENCES Aufkleber.Farbe_fab(iId),
	CONSTRAINT fk_art_gro FOREIGN KEY(gro_iId) REFERENCES Aufkleber.Groesse_gro(iId)
GO




ALTER TABLE Aufkleber.mod_mat
ADD
	CONSTRAINT fk_mod_mat_mod FOREIGN KEY(mod_iId) REFERENCES Aufkleber.Modell_mod(iId),
	CONSTRAINT fk_mod_mat_mat FOREIGN KEY(mat_iId) REFERENCES Aufkleber.Material_mat(iId)
GO


ALTER TABLE Aufkleber.Modell_mod
ADD
	CONSTRAINT fk_mod_typ FOREIGN KEY(typ_iId) REFERENCES Aufkleber.Typ_typ(iId)
GO




------------------
-- Tabellen Füllen
------------------


INSERT INTO Aufkleber.Farbe_fab (sBezeichnung)
VALUES
('Blau'),
('Rot'),
('Grün'),
('Dunkellilablassblau'),
('Violett'),
('Grau'),
('Schwarz'),
('Gelb')
GO


INSERT INTO Aufkleber.Groesse_gro (rGroesse)
VALUES
(24),
(40),
(41),
(42),
(43),
(44),
(45),
(46)
GO




INSERT INTO Aufkleber.Unternehmen_un (sName, bLogo, sSlogan)
VALUES
('Platzhalter', NULL, 'Made in Bavaria GERMANY')
GO


INSERT INTO Aufkleber.Entwickler_ent (sVName, sNName, sKeyAufgabe)
VALUES
('Lukas', 'Kant', 'Datenbank'),
('Christoph', 'Duhm', 'Entwicklung'),
('Christian', 'Oberst', 'Logo'),
('Phillip', 'Stürmer', 'Lastenheft'),
('Timo', 'Stein', 'Pflichtenheft')
GO


INSERT INTO Aufkleber.LastDruck_lastd (iFreiAb, sPapierFormat)
VALUES
(23, 'A4')
GO


INSERT INTO Aufkleber.Material_mat (sNummer, sBezeichnung)
VALUES
('E-LED', 'Echtleder'),
('STOFF', 'Stoff Mischgewebe'),
('PLA', 'Plastik'),
('F-LED', 'Falschleder'),
('HOLZ', 'Eichenholz')
GO

INSERT INTO Aufkleber.Typ_typ (sBezeichnung)
VALUES
('Sandalen'),
('Halbschuhe'),
('Trunschule'),
('Highheels'),
('Badelatschen'),
('Adiletten'),
('Stiefel')
GO


INSERT INTO Aufkleber.Modell_mod (sBezeichnung, sGeschlecht, bSymbolBild, typ_iId)
VALUES
('SupiFly', 'w', NULL, 1),
('FreshDesh', 'm', NULL, 3),
('Ghost 9', 'w', NULL, 1),
('SpeschlDeschl', 'w', NULL, 2),
('KlopfiDopfi', 'u', NULL, 2),
('SnikiDiki', 'm', NULL, 2)
GO


INSERT INTO Aufkleber.mod_mat (mod_iId, mat_iId)
VALUES
(1,3),
(1,2),
(2,4),
(2,5),
(3,3),
(4,1),
(4,2),
(5,3)
GO



DECLARE @mod_iId int;

DECLARE artikel_cursor CURSOR LOCAL STATIC
FOR
	SELECT iId FROM Aufkleber.Modell_mod;

OPEN artikel_cursor

FETCH NEXT FROM artikel_cursor INTO @mod_iId
WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO Aufkleber.Artikel_art (sBezeichnung, iArtNr, mod_iId, fab_iId, gro_iId)
	VALUES
	(NULL, 12345, @mod_iId, 1, 1),
	(NULL, 34335, @mod_iId, 2, 3),
	(NULL, 12245, @mod_iId, 3, 1),
	(NULL, 46465, @mod_iId, 4, 1),
	(NULL, 12345, @mod_iId, 5, 2),
	(NULL, 12345, @mod_iId, 3, 1),
	(NULL, 13254, @mod_iId, 5, 3),
	(NULL, 12345, @mod_iId, 1, 3),
	(NULL, 64634, @mod_iId, 2, 2),
	(NULL, 75755, @mod_iId, 3, 1),
	(NULL, 23345, @mod_iId, 2, 3)

	FETCH NEXT FROM artikel_cursor INTO @mod_iId
END

CLOSE artikel_cursor
DEALLOCATE artikel_cursor


