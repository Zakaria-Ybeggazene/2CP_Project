﻿Public Class RecherchePromoViewModel
    Inherits WorkspaceViewModel

    'Fields
    Private _pvEnable As Boolean = False
    Private _statEnable As Boolean = False
    Public Property PvEnable() As Boolean
        Get
            Return _pvEnable
        End Get
        Set(ByVal value As Boolean)
            _pvEnable = value
            OnPropertyChanged("PvEnable")
        End Set
    End Property
    Public Property StatEnable() As Boolean
        Get
            Return _statEnable
        End Get
        Set(ByVal value As Boolean)
            _statEnable = value
            OnPropertyChanged("StatEnable")
        End Set
    End Property
    Private _niveau, _annee As String
    Private _resultat As Promotion
    Private _nbIns As Integer
    Private _pvOpacity As Decimal = 0.5
    Private _statOpacity As Decimal = 0.5
    Public Property PvOpacity() As Decimal
        Get
            Return _pvOpacity
        End Get
        Set(ByVal value As Decimal)
            _pvOpacity = value
            OnPropertyChanged("PvOpacity")
        End Set
    End Property
    Public Property StatOpacity() As Decimal
        Get
            Return _statOpacity
        End Get
        Set(ByVal value As Decimal)
            _statOpacity = value
            OnPropertyChanged("StatOpacity")
        End Set
    End Property
    Private _promotionViewModel As ViewModelBase
    'Recherche sub
    Public Sub recherche()
        If Annee = "" Or Annee = "Année" Or Niveau = "" Or Niveau = "Niveau" Then
            MsgBox("Vous devez spécifier l'année et le niveau", MsgBoxStyle.Information)
        Else
            Dim niv As Niveau
            If Niveau = "SI3 & SIQ3" Then
                niv = HistoESI.Niveau.CS3
            Else
                niv = Util.stringToNiveau(Niveau)
            End If
            Dim anneeCut As String = Annee.Substring(2)
            Mouse.OverrideCursor = Cursors.Wait
            If niv = HistoESI.Niveau.SI3 Or niv = HistoESI.Niveau.SIQ3 Or niv = HistoESI.Niveau.CS3 Then
                Resultat = Repository.recherche_promo_parcours(niv, anneeCut)
                If Resultat Is Nothing Then
                    PvOpacity = 0.5
                    PvEnable = False
                    StatOpacity = 0.5
                    StatEnable = False
                    reset()
                    MsgBox("Promotion introuvable", MsgBoxStyle.Information)
                Else
                    PvOpacity = 0.5
                    PvEnable = False
                    StatOpacity = 1
                    StatEnable = True
                    NombreInscrits = Resultat.NbInscrits.ToString
                    PromotionViewModel = New ClassementViewModel(CType(Resultat, PromotionParcours), _addEtudiantView)
                End If
            Else
                Resultat = Repository.recherche_promo(niv, anneeCut)
                If Resultat Is Nothing Then
                    PvOpacity = 0.5
                    PvEnable = False
                    StatOpacity = 0.5
                    StatEnable = False
                    reset()
                    MsgBox("Promotion introuvable", MsgBoxStyle.Information)
                Else
                    PvOpacity = 1
                    PvEnable = True
                    StatOpacity = 1
                    StatEnable = True
                    NombreInscrits = Resultat.NbInscrits.ToString
                    PromotionViewModel = New PromotionViewModel(CType(Resultat, PromotionAnnee), _addEtudiantView)
                End If
            End If

        End If
        Mouse.OverrideCursor = Nothing
    End Sub

    'Recherche command
    Public Property RechCommand As ICommand

    'Commande de réinitialisation
    Public Property ResetCommand As ICommand

    'Properties
    Public Property Annee() As String
        Get
            Return _annee
        End Get
        Set(ByVal value As String)
            _annee = value
            OnPropertyChanged("Annee")
        End Set
    End Property
    Public Property Niveau() As String
        Get
            Return _niveau
        End Get
        Set(ByVal value As String)
            _niveau = value
            OnPropertyChanged("Niveau")
        End Set
    End Property
    Public Property Resultat() As Promotion
        Get
            Return _resultat
        End Get
        Set(ByVal value As Promotion)
            _resultat = value
            OnPropertyChanged("Resultat")
        End Set
    End Property

    Public Property NombreInscrits As String
        Get
            Return "Nombre d'inscrits  :  " & _nbIns.ToString
        End Get
        Set(ByVal value As String)
            _nbIns = value
            OnPropertyChanged("NombreInscrits")
        End Set
    End Property
    Public Property PromotionViewModel As ViewModelBase
        Get
            Return _promotionViewModel
        End Get
        Set(ByVal value As ViewModelBase)
            _promotionViewModel = value
            OnPropertyChanged("PromotionViewModel")
        End Set
    End Property

    Private _addEtudiantView As Action(Of Object)

    'NEW SUB
    Public Sub New(ByVal displayName As String, ByRef addEtudiantView As Action(Of Object), ByVal addStatisticsView As Action(Of Object))
        MyBase.New(displayName)
        _addEtudiantView = addEtudiantView
        Me.RechCommand = New RelayCommand(AddressOf recherche)
        Me.ResetCommand = New RelayCommand(AddressOf reset)
        Me.ViewStatistics = New RelayCommand(addStatisticsView)
        Me.PvDelibCommand = New RelayCommand(AddressOf generatePV)
        PromotionViewModel = New NothingViewModel("/HistoESI;component/Images/Groupe 20.png")
    End Sub
    Private _viewStatistics As RelayCommand
    Public Property ViewStatistics As RelayCommand
        Get
            Return _viewStatistics
        End Get
        Set(ByVal value As RelayCommand)
            _viewStatistics = value
        End Set

    End Property
    Private _pvDelibCommand As ICommand
    Public Property PvDelibCommand As ICommand
        Get
            Return _pvDelibCommand
        End Get
        Set(ByVal value As ICommand)
            _pvDelibCommand = value
        End Set
    End Property

    'Procedure pour génerer le PV de délibération
    Public Sub generatePV(ByVal o As Object)
        Dim reportWindow As ReportWindow = New ReportWindow
        If Not _resultat Is Nothing Then
            Try
                reportWindow.Viewer.ViewerCore.ReportSource = CrystalReports.PvDeliberation(_resultat)
                reportWindow.Show()
            Catch e As Exception
                MsgBox("Le rapport n'a pas pu s'ouvrir", MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub reset()
        PromotionViewModel = New NothingViewModel("/HistoESI;component/Images/Groupe 20.png")
        Annee = "Année"
        Niveau = "Niveau"
        NombreInscrits = 0
        Resultat = Nothing
        PvOpacity = 0.5
        PvEnable = False
        StatOpacity = 0.5
        StatEnable = False

    End Sub

End Class