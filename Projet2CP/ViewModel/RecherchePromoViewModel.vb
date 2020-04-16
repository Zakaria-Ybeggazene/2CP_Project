﻿Public Class RecherchePromoViewModel
    Inherits WorkspaceViewModel

    'Fields
    Private _niveau, _annee As String
    Private _resultat As Promotion
    Private _listEtuds As List(Of Etudiant)
    Private _listMats As Dictionary(Of Matiere, Decimal)
    'Recherche sub
    Public Sub recherche()
        Try
            Resultat = Repository.recherche_promo(Niveau, Annee)
            ListeEtuds = Resultat.ListeEtudiants
            ListeMatieres = Resultat.ListeMatiere
        Catch e As Exception
        End Try
    End Sub

    'Recherche command
    Public _rechCommand As New RelayCommand(AddressOf recherche)
    Public ReadOnly Property RechCommand As ICommand
        Get
            Return _rechCommand
        End Get
    End Property

    'Properties
    Public Property Annee() As String
        Get
            Return _annee
        End Get
        Set(ByVal value As String)
            _annee = value
        End Set
    End Property
    Public Property Niveau() As String
        Get
            Return _niveau
        End Get
        Set(ByVal value As String)
            _niveau = value
        End Set
    End Property
    Public Property Resultat() As Promotion
        Get
            Return _resultat
        End Get
        Set(ByVal value As Promotion)
            _resultat = value
        End Set
    End Property
    Public Property ListeEtuds As List(Of Etudiant)
        Get
            Return _listEtuds

        End Get
        Set(ByVal value As List(Of Etudiant))
            _listEtuds = value
            OnPropertyChanged("ListeEtuds")
        End Set
    End Property
    Public Property ListeMatieres As Dictionary(Of Matiere, Decimal)
        Get
            Return _listMats

        End Get
        Set(ByVal value As Dictionary(Of Matiere, Decimal))
            _listMats = value
            OnPropertyChanged("ListeMatieres")
        End Set
    End Property





    'NEW SUB
    Public Sub New(ByVal displayName As String)
        MyBase.New(displayName)
        v = New RecherchePromoView()
    End Sub

    Private v As RecherchePromoView
End Class
