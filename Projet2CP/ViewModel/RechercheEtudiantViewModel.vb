﻿Imports System.Collections.ObjectModel

Public Class RechercheEtudiantViewModel
    Inherits WorkspaceViewModel

    Private _matricule, _nom, _prenom, _nomA, _prenomA, _lieuNais, _annee, _sexe, _wilayaNais As String
    Private _visibility1 As Visibility = Visibility.Hidden
    Private _visibility2 As Visibility
    Private _visibility3 As Visibility = Visibility.Hidden
    Private _dateNais As DateTime? = Nothing
    Private _resultats As List(Of Etudiant)
    Private v As RechercheEtudiantView

    Public Sub New(ByVal displayName As String, ByRef addEtudiantView As Action(Of Object))
        MyBase.New(displayName)
        v = New RechercheEtudiantView()
        Me.EtudiantTab = New RelayCommand(addEtudiantView)
    End Sub

    'Recherche Command Property
    Public _rechercheCommand As New RelayCommand(AddressOf recherche)
    Public _resetCommand As New RelayCommand(AddressOf Reset)
    Public ReadOnly Property RechercheCommand As ICommand
        Get
            Return _rechercheCommand
        End Get
    End Property
    Public ReadOnly Property ResetCommand As ICommand
        Get
            Return _resetCommand
        End Get
    End Property
    'Recherche Sub
    Public Sub recherche()
        If Sexe = "Masculin" Then
            _sexe = "1"
        ElseIf Sexe = "Feminin" Then
            _sexe = "2"
        Else
            _sexe = ""
        End If
        Dim _strDate As String
        If DateNais.Equals(Nothing) Then
            _strDate = ""
        Else
            _strDate = DateNais.Value.ToString("dd/MM/yyyy").Trim
            _strDate = _strDate.Remove(6, 2)
        End If
        Mouse.OverrideCursor = Cursors.Wait
        If Annee <> "" And Annee <> Nothing Then
            If Annee.Length = 4 And (Annee.Substring(0, 2) = "20" Or Annee.Substring(0, 2) = "19") Then
                Annee = Annee.Remove(0, 2)
            End If
        End If
        Resultats = Repository.recherche_etudiants(Matricule, Nom, Prenom, NomA, PrenomA, _strDate, Sexe, Annee, WilayaNais, LieuNais)
        If Resultats.Count = 0 Then
            Visibility2 = Visibility.Hidden
            Visibility1 = Visibility.Hidden
            Visibility3 = Visibility.Visible
        Else
            Visibility2 = Visibility.Hidden
            Visibility1 = Visibility.Visible
            Visibility3 = Visibility.Hidden
        End If

        Mouse.OverrideCursor = Nothing
    End Sub
    'Reset 
    Public Sub reset()
        Matricule = ""
        Nom = ""
        NomA = ""
        Prenom = ""
        PrenomA = ""
        LieuNais = ""
        WilayaNais = ""
        Annee = ""
        Visibility2 = Visibility.Visible
        Visibility1 = Visibility.Hidden
        Visibility3 = Visibility.Hidden
    End Sub
    'Recherche Properties

    Public Property Visibility1() As Visibility
        Get
            Return _visibility1
        End Get
        Set(ByVal value As Visibility)
            _visibility1 = value
            OnPropertyChanged("Visibility1")
        End Set
    End Property
    Public Property Visibility2() As Visibility
        Get
            Return _visibility2
        End Get
        Set(ByVal value As Visibility)
            _visibility2 = value
            OnPropertyChanged("Visibility2")
        End Set
    End Property
    Public Property Visibility3() As Visibility
        Get
            Return _visibility3
        End Get
        Set(ByVal value As Visibility)
            _visibility3 = value
            OnPropertyChanged("Visibility3")
        End Set
    End Property
    Public Property Matricule() As String
        Get
            Return _matricule
        End Get
        Set(ByVal value As String)
            _matricule = value
        End Set
    End Property
    Public Property Nom() As String
        Get
            Return _nom
        End Get
        Set(ByVal value As String)
            _nom = value
        End Set
    End Property
    Public Property Prenom() As String
        Get
            Return _prenom
        End Get
        Set(ByVal value As String)
            _prenom = value
        End Set
    End Property
    Public Property NomA() As String
        Get
            Return _nomA
        End Get
        Set(ByVal value As String)
            _nomA = value
        End Set
    End Property
    Public Property PrenomA() As String
        Get
            Return _prenomA
        End Get
        Set(ByVal value As String)
            _prenomA = value
        End Set
    End Property

    Public Property DateNais() As DateTime?
        Get
            Return _dateNais
        End Get
        Set(ByVal value As DateTime?)
            _dateNais = value
        End Set
    End Property

    Public Property LieuNais() As String
        Get
            Return _lieuNais
        End Get
        Set(ByVal value As String)
            _lieuNais = value
        End Set
    End Property
    Public Property WilayaNais() As String
        Get
            Return _wilayaNais
        End Get
        Set(ByVal value As String)
            _wilayaNais = value
        End Set
    End Property
    Public Property Sexe() As String
        Get
            Return _sexe
        End Get
        Set(ByVal value As String)
            _sexe = value
        End Set
    End Property

    Public Property Annee() As String
        Get
            Return _annee
        End Get
        Set(ByVal value As String)
            _annee = value
        End Set
    End Property
    Public Property Resultats As List(Of Etudiant)
        Get
            Return _resultats

        End Get
        Set(ByVal value As List(Of Etudiant))
            _resultats = value
            OnPropertyChanged("Resultats")
        End Set
    End Property

    Private _etudiantTab As ICommand
    Public Property EtudiantTab As ICommand
        Get
            Return _etudiantTab
        End Get
        Set(ByVal value As ICommand)
            _etudiantTab = value
        End Set
    End Property
End Class
