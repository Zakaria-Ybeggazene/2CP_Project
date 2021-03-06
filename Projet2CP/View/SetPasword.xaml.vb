﻿Imports System.Threading
Imports System.ComponentModel

Public Class SetPasword

    Private inscrit, note, mat, rat As String

    Public Sub New(ByVal inscritPath As String, ByVal notePath As String, ByVal matPath As String, ByVal ratPath As String)
        inscrit = inscritPath
        note = notePath
        mat = matPath
        rat = ratPath
        InitializeComponent()
    End Sub


    Private Sub userPassword_PasswordChanged(ByVal sender As System.Object, ByVal e As RoutedEventArgs)
        If userPassword.Password.Length > 0 Then
            userPasswordHint.Visibility = Windows.Visibility.Hidden
        Else
            userPasswordHint.Visibility = Windows.Visibility.Visible
        End If
        If ConfirmUserPassword.Password.Length <> 0 Then
            ConfirmUserPassword.Password = ""
        End If
        If userPassword.Password.Length < 4 And userPassword.Password.Length <> 0 Then
            shortpwdlabel1.Visibility = Windows.Visibility.Visible
            userValidLabel.Visibility = Windows.Visibility.Hidden
            ConfirmUserPassword.IsEnabled = False
        Else
            shortpwdlabel1.Visibility = Windows.Visibility.Hidden
            ConfirmUserPassword.IsEnabled = True
        End If
        If userPassword.Password.Length > 0 Then
            userPasswordHint.Visibility = Windows.Visibility.Hidden
        Else
            userPasswordHint.Visibility = Windows.Visibility.Visible
        End If
    End Sub
    Private Sub AdminPassword_PasswordChanged(ByVal sender As System.Object, ByVal e As RoutedEventArgs)
        If AdminPassword.Password.Length > 0 Then
            AdminPasswordHint.Visibility = Windows.Visibility.Hidden
        Else
            AdminPasswordHint.Visibility = Windows.Visibility.Visible
        End If
        If ConfirmAdminPassword.Password.Length <> 0 Then
            ConfirmAdminPassword.Password = ""
        End If
        If AdminPassword.Password.Length < 4 And AdminPassword.Password.Length <> 0 Then
            shortpwdlabel2.Visibility = Windows.Visibility.Visible
            adminValidLabel.Visibility = Windows.Visibility.Hidden
            ConfirmAdminPassword.IsEnabled = False
        Else
            shortpwdlabel2.Visibility = Windows.Visibility.Hidden
            ConfirmAdminPassword.IsEnabled = True
        End If
        If AdminPassword.Password.Length > 0 Then
            AdminPasswordHint.Visibility = Windows.Visibility.Hidden
        Else
            AdminPasswordHint.Visibility = Windows.Visibility.Visible
        End If
    End Sub
    Private Sub ConfirmUserPassword_PasswordChanged(ByVal sender As System.Object, ByVal e As RoutedEventArgs)
        userPassword.IsEnabled = False
        If shortpwdlabel2.Visibility = Windows.Visibility.Visible Then
            ConfirmAdminPassword.IsEnabled = False
        Else
            ConfirmAdminPassword.IsEnabled = True
        End If
        If ConfirmUserPassword.Password.Length = 0 And userPassword.Password.Length > 2 Then
            ConfirmUserPasswordHint.Visibility = Windows.Visibility.Visible
            userValidLabel.Visibility = Windows.Visibility.Hidden
        Else
            ConfirmUserPasswordHint.Visibility = Windows.Visibility.Hidden
            userValidLabel.Visibility = Windows.Visibility.Hidden
        End If
        If ConfirmUserPassword.Password.Length <> 0 And ConfirmUserPassword.Password <> userPassword.Password Then
            UserPasswordLabel.Visibility = Windows.Visibility.Visible
            userValidLabel.Visibility = Windows.Visibility.Hidden
        Else
            UserPasswordLabel.Visibility = Windows.Visibility.Hidden
            If ConfirmUserPassword.Password.Length <> 0 And ConfirmUserPassword.Password = userPassword.Password Then
                userValidLabel.Visibility = Windows.Visibility.Visible
            Else
                userValidLabel.Visibility = Windows.Visibility.Hidden
            End If
        End If
        If adminValidLabel.Visibility = Windows.Visibility.Visible And userValidLabel.Visibility = Windows.Visibility.Visible Then
            If AdminPassword.Password.Length <> 0 And userPassword.Password.Length <> 0 Then
                terminerButton.IsEnabled = True
                terminerButton.Opacity = 1
                ImportbarVerified.Visibility = Windows.Visibility.Visible
            Else
                terminerButton.IsEnabled = False
                terminerButton.Opacity = 0.5
                ImportbarVerified.Visibility = Windows.Visibility.Hidden
            End If

        Else
            terminerButton.IsEnabled = False
            terminerButton.Opacity = 0.5
            ImportbarVerified.Visibility = Windows.Visibility.Hidden
        End If
        userPassword.IsEnabled = True
    End Sub

    Private Sub ConfirmAdminPassword_PasswordChanged(ByVal sender As System.Object, ByVal e As RoutedEventArgs)
        AdminPassword.IsEnabled = False
        If ConfirmAdminPassword.Password.Length = 0 Then
            ConfirmAdminPasswordHint.Visibility = Windows.Visibility.Visible
            adminValidLabel.Visibility = Windows.Visibility.Hidden
        Else
            ConfirmAdminPasswordHint.Visibility = Windows.Visibility.Hidden
            adminValidLabel.Visibility = Windows.Visibility.Hidden
        End If
        If ConfirmAdminPassword.Password.Length <> 0 And ConfirmAdminPassword.Password <> AdminPassword.Password Then
            AdminPasswordLabel.Visibility = Windows.Visibility.Visible
            adminValidLabel.Visibility = Windows.Visibility.Hidden
        Else
            AdminPasswordLabel.Visibility = Windows.Visibility.Hidden
            If ConfirmAdminPassword.Password.Length <> 0 And ConfirmAdminPassword.Password = AdminPassword.Password Then
                adminValidLabel.Visibility = Windows.Visibility.Visible
            Else
                adminValidLabel.Visibility = Windows.Visibility.Hidden
            End If
        End If
        If adminValidLabel.Visibility = Windows.Visibility.Visible And userValidLabel.Visibility = Windows.Visibility.Visible Then
            terminerButton.IsEnabled = True
            terminerButton.Opacity = 1
            ImportbarVerified.Visibility = Windows.Visibility.Visible
        Else
            terminerButton.IsEnabled = False
            terminerButton.Opacity = 0.5
            ImportbarVerified.Visibility = Windows.Visibility.Hidden
        End If
        AdminPassword.IsEnabled = True
    End Sub

    Private Sub tb_KeyDown(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles ConfirmAdminPassword.PreviewKeyDown
        If (e.Key = Key.Enter) Then
            If userValidLabel.Visibility = Windows.Visibility.Visible And adminValidLabel.Visibility = Windows.Visibility.Visible Then
                Me.ForceCursor = True
                Mouse.OverrideCursor = Cursors.Wait
                CType(DataContext, MigrationViewModel).userpwd = userPassword.Password
                CType(DataContext, MigrationViewModel).adminpwd = AdminPassword.Password
                Dim thr As New Thread(AddressOf CType(DataContext, MigrationViewModel).migration)
                Dim start As Single
                start = DateAndTime.Timer
                'CType(DataContext, MigrationViewModel).migration(Nothing)
                thr.Start()
                thr.Join()
                MsgBox("Migration réalisée avec succès !" & vbCrLf & "Temps d'exécution : " & DateAndTime.Timer - start & " secondes")
                Dim loginWindow As LoginWindow = New LoginWindow()
                Mouse.OverrideCursor = Nothing
                Me.Close()
                loginWindow.Show()
            End If
        End If
    End Sub

    Private Sub terminerButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles terminerButton.Click
        If userValidLabel.Visibility = Windows.Visibility.Visible And adminValidLabel.Visibility = Windows.Visibility.Visible Then
            Me.ForceCursor = True
            Mouse.OverrideCursor = Cursors.Wait
            CType(DataContext, MigrationViewModel).userpwd = userPassword.Password
            CType(DataContext, MigrationViewModel).adminpwd = AdminPassword.Password
            Dim thr As New Thread(AddressOf CType(DataContext, MigrationViewModel).migration)
            Dim start As Single
            start = DateAndTime.Timer
            'CType(DataContext, MigrationViewModel).migration(Nothing)
            thr.Start()
            thr.Join()
            MsgBox("Migration réalisée avec succès !" & vbCrLf & "Temps d'exécution : " & DateAndTime.Timer - start & " secondes")
            Dim loginWindow As LoginWindow = New LoginWindow()
            Mouse.OverrideCursor = Nothing
            Me.Close()
            loginWindow.Show()
        End If
    End Sub
End Class