﻿Imports System.Data.OleDb

Public Class MigrationViewModel
    Inherits ViewModelBase

    Public Sub New(ByVal displayName As String, ByVal inscrit As String, ByVal note As String, ByVal matiere As String, ByVal rattrap As String)
        MyBase.DisplayName = displayName
        Me.inscrit = inscrit
        Me.note = note
        Me.matiere = matiere
        Me.rattrap = rattrap
    End Sub
    Private _percent As Integer = 0
    Public Property Percent
        Get
            Return _percent
        End Get
        Set(ByVal value)
            _percent = value
            OnPropertyChanged("percent")
        End Set
    End Property

    Public inscrit, note, matiere, rattrap, adminpwd, userpwd As String

    Public Sub migration(ByVal obj As Object)

        Try
            'the path where access database will be created here
            Dim dbPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\db.accdb"
            'access database
            Dim dbConnString As String
            Dim db As Object

            'Create new database
            db = CreateObject("Access.Application")
            db.NewCurrentDatabase(dbPath)
            db.quit()
            Percent = 25
            dbConnString = "provider=microsoft.ace.oledb.12.0;data source=" & dbPath & ""
            Dim connAccess As New System.Data.OleDb.OleDbConnection(dbConnString)
            Dim cmdAccess As New System.Data.OleDb.OleDbCommand()
            cmdAccess.Connection = connAccess
            connAccess.Open()

            ' ''Creating tables in the access database
            cmdAccess.CommandText = "create table ETUDIANT " _
                              & "(MATRICULE char(7),Matric_ins TEXT,NomEtud TEXT, Prenoms TEXT,NomEtudA TEXT,PrenomsA TEXT,DateNais TEXT,LieuNaisA TEXT, " _
                              & "LieuNais TEXT,WilayaNaisA TEXT,Adresse TEXT,Ville TEXT,Wilaya TEXT,CodPost TEXT,Sexe short,Fils_de TEXT,Et_de TEXT, " _
                              & "ANNEEBAC TEXT,SERIEBAC TEXT,MOYBAC TEXT,WILBAC TEXT ,primary key(MATRICULE));"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table PROMO(ANNEE char(2),OPTIIN char(3), ANETIN char(2),NbInscrits int,NbDoublant int,NbRattrap int, primary key(ANNEE,OPTIIN, ANETIN));"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table ETUDE (MATRICULE char(7),ANNEE char(2),OPTIIN char(3),ANETIN char(2),CycIN TEXT,NumGrp TEXT,NumScn TEXT,Moyenne decimal(4,2),RangIN TEXT," _
                                   & "MentIN TEXT,ElimIN TEXT,RatIN TEXT,DECIIN TEXT,ADM TEXT, primary key(MATRICULE,ANNEE,OPTIIN,ANETIN));"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table ETUDNOTE(MATRICULE TEXT,ANNEE TEXT,OPTIN TEXT,ANETIN TEXT,ComaMa TEXT,CycNO TEXT,NoJuNo TEXT,NoSyNo TEXT," _
                                        & "NoRaNo TEXT,ElimNo TEXT,RatrNo TEXT, primary key(MATRICULE,ANNEE,OPTIN,ANETIN,ComaMa));"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table RATTRAP (MATRICULE TEXT,ANNEE TEXT,OPTIRA TEXT,ANETRA TEXT,CycRA TEXT,MoyeRa TEXT,MentRa TEXT," _
                                    & "ElimRa TEXT,primary key(MATRICULE,ANNEE,OPTIRA,ANETRA));"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table MATIERE (COMAMA TEXT,OPTIMA TEXT,ANETMA TEXT,LibeMA TEXT,CoefMA TEXT,primary key(COMAMA,ANETMA,OPTIMA));"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table MOYMAT (ANNEE TEXT,OPTIMA TEXT,ANETMA TEXT,COMAMA TEXT,MoyenneMA TEXT,primary key(ANNEE,OPTIMA,ANETMA,COMAMA));"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table  CHAMPS_VIDES_INSCRIT (MATRICULE TEXT,ANNEE TEXT,OPTIIN TEXT,ANETIN TEXT)"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table  CHAMPS_VIDES_NOTE (MATRICULE TEXT,ANNEE TEXT,OPTIN TEXT,ANETIN TEXT,ComaMa TEXT)"
            cmdAccess.ExecuteNonQuery()

            cmdAccess.CommandText = "create table  AUTHENTIC (MotDePasse TEXT); "
            cmdAccess.ExecuteNonQuery()

            'Setting admin password
            cmdAccess.CommandText = "INSERT INTO AUTHENTIC(MotDePasse)" _
                                  & "VALUES ('" & adminpwd & "') ;"
            cmdAccess.ExecuteNonQuery()
            Percent = 50
            ''Excel files
            Dim connString As String = "provider=microsoft.ace.oledb.12.0;data source=" & inscrit & " ;extended properties=""excel 12.0;hdr=yes"""

            Dim conn As New System.Data.OleDb.OleDbConnection(connString)
            Dim cmd As New System.Data.OleDb.OleDbCommand()
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[CHAMPS_VIDES_INSCRIT] (MATRICULE,ANNEE ,OPTIIN , ANETIN ) SELECT MATRIN,ANSCIN ,OPTIIN ,ANETIN " _
                                    & "FROM [INSCRIT$] where MATRIN IS NULL OR ANSCIN IS NULL OR OPTIIN IS NULL OR ANETIN IS NULL;"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[ETUDIANT]" _
                            & "(MATRICULE ,Matric_ins ,NomEtud , Prenoms ,NomEtudA ,PrenomsA ,DateNais,LieuNaisA,Lieunais ,WilayaNaisA,Adresse ,Ville ,Wilaya, " _
                            & "CodPost ,Sexe ,Fils_de ,Et_de,ANNEEBAC,SERIEBAC,MOYBAC,WILBAC) SELECT MATRIN,max(MATRIC_INS) as 'MATRIC_INS'," _
                            & " max(NomEtud) as 'NomEtud',max(Prenoms) as 'Prenoms',max(NomEtudA) as 'NomEtudA' ,max(PrenomsA) as 'PrenomsA',max(DATENAIS) as 'DATENAIS'," _
                            & " max(LIEUNAISA) as 'LIEUNAISA',max(LIEUNAIS) as 'LIEUNAIS' ,max(WILNAISA) as 'WILNAISA',max(ADRESSE) as 'ADRESSE',max(VILLE) as 'VILLE', " _
                            & "max(WILAYA) as 'WILAYA',max(CODPOST) as 'CODPOST' ,min(SEXE) as 'SEXE',max(FILS_DE) as 'FILS_DE',max(ET_DE) as 'ET_DE',max(ANNEEBAC) as 'ANNEEBAC'," _
                            & "max(SERIEBAC) as 'SERIEBAC' ,max(MOYBAC) as 'MOYBAC',max(WILBAC) as 'WILBAC' FROM [INSCRIT$] WHERE MATRIN IS NOT NULL GROUP BY MATRIN;"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[PROMO] (ANNEE ,OPTIIN , ANETIN, NbInscrits) SELECT ANSCIN ,OPTIIN ,ANETIN, COUNT(*) AS 'NbInscrits' " _
                                    & "FROM [INSCRIT$] where ANSCIN IS NOT NULL AND OPTIIN IS NOT NULL AND ANETIN IS NOT NULL GROUP BY ANSCIN, OPTIIN,ANETIN ORDER BY ANSCIN;"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[ETUDE] (MATRICULE,ANNEE,OPTIIN ,ANETIN,CycIN ,NumGrp ,NumScn,Moyenne,RangIN ,MentIN,ElimIN,RatIN ,DECIIN,ADM)" _
                                     & " SELECT MATRIN, ANSCIN,OPTIIN,ANETIN,max(CYCLIN) as 'CYCLIN',max(NG) as 'NumG' , max(NS) as 'NS', max(MOYEIN) as 'MOYEIN',  " _
                                     & "max(RANGIN) as 'RANGIN',max(MENTIN) as 'MENTIN' , max(ELIMIN) as 'ELIMIN',max(RATRIN) as 'RATRIN' ,max(DECIIN) as 'DECIIN',max(ADM) as 'ADM' " _
                                     & " FROM [INSCRIT$] WHERE MATRIN IS NOT NULL AND ANSCIN IS NOT NULL AND OPTIIN IS NOT NULL AND ANETIN IS NOT NULL " _
                                     & " GROUP BY MATRIN, ANSCIN,OPTIIN,ANETIN;"
            cmd.ExecuteNonQuery()
            conn.Close()

            connString = "provider=microsoft.ace.oledb.12.0;data source=" & matiere & " ;extended properties=""excel 12.0;hdr=yes"""
            conn = New System.Data.OleDb.OleDbConnection(connString)
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[MATIERE] (COMAMA,OPTIMA,ANETMA,LibeMA ,CoefMA)" _
                             & " SELECT COMAMA,OPTIMA, ANETMA,max(LIBEMA) as 'LIBEMA',max(COEFMA) as 'COEFMA' FROM [MATIERE$]" _
                             & "WHERE ANETMA IS NOT NULL AND OPTIMA IS NOT NULL AND COMAMA IS NOT NULL GROUP BY COMAMA,OPTIMA,ANETMA;"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[MOYMAT] (ANNEE,OPTIMA ,ANETMA,COMAMA ,MoyenneMA)" _
                                    & " SELECT ANSCMA, OPTIMA, ANETMA, COMAMA,max(MOYMAT) as 'MOYMAT' FROM [MATIERE$] " _
                                    & "WHERE ANSCMA IS NOT NULL AND ANETMA IS NOT NULL AND OPTIMA IS NOT NULL AND COMAMA IS NOT NULL GROUP BY ANSCMA, OPTIMA, ANETMA, COMAMA ;"
            cmd.ExecuteNonQuery()
            conn.Close()
            Percent = 75
            connString = "provider=microsoft.ace.oledb.12.0;data source=" & note & ";extended properties=""excel 12.0;hdr=yes"""
            conn = New System.Data.OleDb.OleDbConnection(connString)
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[CHAMPS_VIDES_NOTE] (MATRICULE,ANNEE ,OPTIN , ANETIN,ComaMa ) SELECT MATRNO ,ANSCNO,OPTINO,ANETNO,COMANO " _
                                    & "FROM [NOTE$] where MATRNO IS NULL OR ANSCNO IS NULL OR OPTINO IS NULL OR ANETNO IS NULL OR COMANO IS NULL;"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[ETUDNOTE] (MATRICULE,ANNEE,OPTIN,ANETIN, ComaMa, CycNO, NoJuNo, NoSyNo,NoRaNo ,ElimNo ,RatrNo)" _
                               & "  SELECT MATRNO ,ANSCNO,OPTINO,ANETNO,COMANO, max(CYCLNO) as 'CYCLNO', max(NOJUNO) as 'NOJUNO' ,max(NOSYNO) as 'NOSYNO', max(NORANO) as 'NORANO'," _
                               & "max(ELIMNO) as 'ELIMNO' ,max(RATRNO) as 'RATRNO'  FROM [NOTE$] WHERE MATRNO IS NOT NULL AND ANSCNO IS NOT NULL AND ANETNO IS NOT NULL AND OPTINO IS NOT NULL And COMANO IS NOT NULL" _
                               & " GROUP BY MATRNO,ANSCNO,OPTINO,ANETNO,COMANO"
            cmd.ExecuteNonQuery()
            conn.Close()

            connString = "provider=microsoft.ace.oledb.12.0;data source=" & rattrap & ";extended properties=""excel 12.0;hdr=yes"""
            conn = New System.Data.OleDb.OleDbConnection(connString)
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "INSERT INTO [MS Access;Database=" & dbPath & "].[RATTRAP] (MATRICULE,ANNEE,OPTIRA,ANETRA,CycRA,MoyeRa,MentRa,ElimRa)" _
                               & "  SELECT MATRRA,ANSCRA,OPTIRA,ANETRA,max(CYCLRA) as 'CYCLRA',max(MOYERA) as 'MOYERA',max(MENTRA) as 'MENTRA',max(ELIMRA) as 'ELIMRA'" _
                               & "  FROM [RATRAP$] WHERE MATRRA Is Not NULL And ANSCRA IS NOT NULL And OPTIRA IS NOT NULL And ANETRA IS NOT NULL " _
                               & " GROUP BY MATRRA,ANSCRA,OPTIRA,ANETRA"
            cmd.ExecuteNonQuery()
            conn.Close()
            connAccess.Close()
            dbConnString = "provider=microsoft.ace.oledb.12.0;data source=" & dbPath & ";Mode=Share Exclusive"
            connAccess = New System.Data.OleDb.OleDbConnection(dbConnString)
            cmdAccess.Connection = connAccess
            connAccess.Open()
            cmdAccess.CommandText = "ALTER DATABASE PASSWORD " & Util.GetHash(userpwd).Substring(0, 14) & " NULL"
            cmdAccess.ExecuteNonQuery()
            connAccess.Close()
            Percent = 100
        Catch ex As Exception
            Dim dbExists As Boolean = System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\db.accdb")
            If dbExists Then
                Kill(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\db.accdb")
            End If
            MsgBox("Migration échouée " & vbCrLf & "Message de l'exception : " & ex.Message)
            Environment.Exit(1)
        End Try
    End Sub
End Class