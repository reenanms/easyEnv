DECLARE @PATH VARCHAR(32) = '/backup'

DECLARE @FILE_BAK VARCHAR(32) = CONCAT(@PATH, +'/db.bak')
DECLARE @FILE_MDF VARCHAR(32) = CONCAT(@PATH, +'/db.mdf')
DECLARE @FILE_LDF VARCHAR(32) = CONCAT(@PATH, +'/db.ldf')

DECLARE @RESTORE_TABLE table
(
 LogicalName          nvarchar(128),
 PhysicalName         nvarchar(260),
 Type                 char(1),
 FileGroupName        nvarchar(128) NULL,
 Size                 numeric(20, 0),
 MaxSize              numeric(20, 0),
 FileId               int NULL,
 CreateLSN            numeric(25,0) NULL,
 DropLSN              numeric(25,0) NULL,
 UniqueFileId         uniqueidentifier NULL,
 readonlyLSN          numeric(25,0) NULL,
 readwriteLSN         numeric(25,0) NULL,
 BackupSizeInBytes    bigint NULL,
 SourceBlkSize        int NULL,
 FileGroupId          int NULL,
 LogGroupGuid         uniqueidentifier NULL,
 DifferentialBaseLsn  numeric(25,0) NULL,
 DifferentialBaseGuid uniqueidentifier NULL,
 IsReadOnly           bit NULL,
 IsPresent            bit NULL,
 TDEThumbprint	      varbinary(32) NULL,
 SnapshotURL	      nvarchar(360) NULL
)

INSERT INTO @RESTORE_TABLE
EXEC ('RESTORE FILELISTONLY FROM DISK = ''' + @FILE_BAK  + '''')

DECLARE @LOGICAL_NAME_MDF VARCHAR(32) = (SELECT TOP 1 LogicalName FROM @RESTORE_TABLE WHERE Type = 'D')
DECLARE @LOGICAL_NAME_LDF VARCHAR(32) = (SELECT TOP 1 LogicalName FROM @RESTORE_TABLE WHERE Type = 'L')

RESTORE DATABASE db
	FROM DISK = @FILE_BAK
WITH REPLACE,
	MOVE @LOGICAL_NAME_MDF TO @FILE_MDF,
	MOVE @LOGICAL_NAME_LDF TO @FILE_LDF